using System;
using System.Collections.Generic;
using Infrastructure;
using System.Data;
using Quartz;
using WebRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Z.EntityFramework.Plus;
using System.Data.SqlClient;

namespace WebMvc
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class CreateDistributeTaskAction
    {
        private string ConnString { set; get; }
        private IJobExecutionContext Context { set; get; }

        public CreateDistributeTaskAction(string _ConnString, IJobExecutionContext _Context)
        {
            ConnString = _ConnString;
            Context = _Context;
        }

        public void Execute()
        {
            DbHelp dbHelp = new DbHelp(ConnString);
            string sql = string.Format(@"SELECT [id],[needStation],[locationCode],[callTime],[callType] FROM [dbo].[material_call_header] WHERE [dbo].[material_call_header].[status] = '{0}'; ", CallStatus.已准备);
            DataSet ds = dbHelp.SelectGet(sql);
            var ccode = "";
            List<MaterialCallHeader> chlist = new List<MaterialCallHeader>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                MaterialCallHeader callheader = new MaterialCallHeader();
                callheader.Id = int.Parse(dr["id"].ToString());
                callheader.NeedStation = dr["needStation"].ToString();
                callheader.LocationCode = dr["locationCode"].ToString();
                callheader.CallTime = DateTime.Parse(dr["callTime"].ToString());
                callheader.CallType =dr["callType"].ToString();
                chlist.Add(callheader);
            }
            if (chlist.Count == ds.Tables[0].Rows.Count)
            {
                foreach (MaterialCallHeader callhd in chlist)
                {
                    string sqlmdth = string.Format(@"SELECT * FROM [dbo].[material_distribute_task_header] WHERE [dbo].[material_distribute_task_header].materialCallId = {0}; ", callhd.Id);
                    DataSet materialdthd = dbHelp.SelectGet(sqlmdth);
                    if (materialdthd.Tables[0].Rows.Count < 1)
                    {   //读取叫料明细数据
                        string calldtsql = string.Format("SELECT * FROM dbo.material_call_detail where dbo.material_call_detail.callHeaderId = {0}; ", callhd.Id);
                        DataSet calldtds = dbHelp.SelectGet(calldtsql);
                        //取料框ABC类别
                        string ABCsql = string.Format("SELECT * FROM dbo.location where dbo.location.code = '{0}'; ", callhd.LocationCode);
                        DataSet ABCds = dbHelp.SelectGet(ABCsql);
                        if (!string.IsNullOrEmpty(ABCds.Tables[0].Rows[0]["ContainerCode"].ToString()))
                        {
                            ccode = ABCds.Tables[0].Rows[0]["ContainerCode"].ToString();
                        }
                        string taskNo = "";
                        if (callhd.CallType==CallType.上料)
                        {
                            string product = "";
                            string productid = "";
                            string confirm = MaterialConfirm.已确认.ToString();
                            if (ABCds.Tables[0].Rows[0]["type"].ToString()!="C")
                            {
                                product = calldtds.Tables[0].Rows[0]["productCode"].ToString();
                                productid = calldtds.Tables[0].Rows[0]["productId"].ToString();
                                confirm = MaterialConfirm.未确认.ToString();
                            }
                            if (ABCds.Tables[0].Rows[0]["Status"].ToString()== LocationStatus.空仓位)
                            {//第一次叫料，直接生成配送任务
                                SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@TASKTYPE", AGVTaskNo.工位叫料), new SqlParameter("@NEW_ID", SqlDbType.VarChar, 32) };
                                parameters[1].Direction = ParameterDirection.Output;
                                taskNo = dbHelp.ProcOutput("Proc_GetTaskNo", parameters, "@NEW_ID").ToString();//生成任务号
                                //写入物料配送任务主表
                                string intaskhdsql = string.Format(@"INSERT INTO [dbo].[material_distribute_task_header] ([taskNo],[materialCallId],[productCode],[containerType],[userCode],[needStation],[locationCode],[containerCode],[needTime],[status],[createTime],[createBy],[callType],[startPlace],[EndPlace],[productId],[materialConfirm])
                                    VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}'); ",
                                            taskNo, callhd.Id, product, ABCds.Tables[0].Rows[0]["type"].ToString(), "AutoJob", callhd.NeedStation, callhd.LocationCode, "", callhd.CallTime, "1", DateTime.Now, "AutoJob", TaskType.上料配送,"", callhd.LocationCode, productid, confirm);
                                dbHelp.DataOperator(intaskhdsql);
                            }
                            else
                            {//先生成取空料框任务
                                SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@TASKTYPE", AGVTaskNo.取空料框), new SqlParameter("@NEW_ID", SqlDbType.VarChar, 32) };
                                parameters[1].Direction = ParameterDirection.Output;
                                string emptyTaskNo = dbHelp.ProcOutput("Proc_GetTaskNo", parameters, "@NEW_ID").ToString();//生成任务号
                                //写入物料配送任务主表
                                string intaskhdsql = string.Format(@"INSERT INTO [dbo].[material_distribute_task_header] ([taskNo],[materialCallId],[productCode],[containerType],[userCode],[needStation],[locationCode],[containerCode],[needTime],[status],[createTime],[createBy],[callType],[startPlace],[EndPlace],[materialConfirm])
                                    VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}'); ",
                                            emptyTaskNo, callhd.Id, "", ABCds.Tables[0].Rows[0]["type"].ToString(), "AutoJob", callhd.NeedStation, callhd.LocationCode, ccode, callhd.CallTime, "1", DateTime.Now, "AutoJob", TaskType.回收料框, callhd.LocationCode,"", MaterialConfirm.已确认);
                                dbHelp.DataOperator(intaskhdsql);
                             //再生成配送任务
                                SqlParameter[] gparameters = new SqlParameter[] { new SqlParameter("@TASKTYPE", AGVTaskNo.工位叫料), new SqlParameter("@NEW_ID", SqlDbType.VarChar, 32) };
                                gparameters[1].Direction = ParameterDirection.Output;
                                taskNo = dbHelp.ProcOutput("Proc_GetTaskNo", gparameters, "@NEW_ID").ToString();//生成任务号
                                //写入物料配送任务主表
                                string gintaskhdsql = string.Format(@"INSERT INTO [dbo].[material_distribute_task_header] ([taskNo],[materialCallId],[productCode],[containerType],[userCode],[needStation],[locationCode],[containerCode],[needTime],[status],[createTime],[createBy],[callType],[startPlace],[EndPlace],[productId],[materialConfirm])
                                    VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}'); ",
                                            taskNo, callhd.Id, product, ABCds.Tables[0].Rows[0]["type"].ToString(), "AutoJob", callhd.NeedStation, callhd.LocationCode, "", callhd.CallTime, "1", DateTime.Now, "AutoJob", TaskType.上料配送,"", callhd.LocationCode, productid, confirm);
                                dbHelp.DataOperator(gintaskhdsql);
                            }
                            //写入配送明细表逻辑
                            if (calldtds.Tables[0].Rows.Count > 0)
                            {
                                //遍历叫料明细数据
                                foreach (DataRow item in calldtds.Tables[0].Rows)
                                {
                                    //获取写入的配送头表信息
                                    string dtaskheadersql = string.Format(@"SELECT * FROM [dbo].[material_distribute_task_header] WHERE [dbo].[material_distribute_task_header].taskNo = '{0}'; ", taskNo);
                                    DataSet dtaskheaderds = dbHelp.SelectGet(dtaskheadersql);
                                    if (dtaskheaderds.Tables[0].Rows.Count > 0)
                                    {
                                        //获取物料需求表数据
                                        string materialmdsql = string.Format(@"SELECT * FROM dbo.material_demand where dbo.material_demand.orderCode = '{0}'; ", item["orderCode"].ToString());
                                        DataSet materialmdds = dbHelp.SelectGet(materialmdsql);
                                        if (materialmdds.Tables[0].Rows.Count > 0)
                                        {
                                            //遍历物料需求数据
                                            foreach (DataRow mditem in materialmdds.Tables[0].Rows)
                                            {
                                                if (mditem["classABC"].ToString() == ABCds.Tables[0].Rows[0]["type"].ToString())
                                                {
                                                    string intaskdtsql = string.Format(@"INSERT INTO [dbo].[material_distribute_task_detail] ([materialDistributeTaskHeaderId],[orderCode],[materialCode],[serialNumber],[qty],[userCode],[createTime],[createBy])
                                         VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'); ",
                                                    dtaskheaderds.Tables[0].Rows[0]["id"].ToString(), mditem["orderCode"].ToString(), mditem["materialCode"].ToString(), item["serialNumber"].ToString(), mditem["distributeQty"].ToString(), "AutoJob", DateTime.Now, "AutoJob");
                                                    dbHelp.DataOperator(intaskdtsql);
                                                }

                                            }

                                        }

                                    }

                                }
                            }

                        }
                        else if (callhd.CallType == CallType.下料)
                        {
                            if (ABCds.Tables[0].Rows[0]["Status"].ToString() == LocationStatus.空仓位)
                            {//第一次呼叫下料，先补空料框
                                SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@TASKTYPE", AGVTaskNo.补给空框), new SqlParameter("@NEW_ID", SqlDbType.VarChar, 32) };
                                parameters[1].Direction = ParameterDirection.Output;
                                taskNo = dbHelp.ProcOutput("Proc_GetTaskNo", parameters, "@NEW_ID").ToString();//生成任务号
                                                                                                               //写入物料配送任务主表
                                string intaskhdsql = string.Format(@"INSERT INTO [dbo].[material_distribute_task_header] ([taskNo],[materialCallId],[productCode],[containerType],[userCode],[needStation],[locationCode],[containerCode],[needTime],[status],[createTime],[createBy],[callType],[startPlace],[EndPlace],[materialConfirm])
                                    VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}'); ",
                                            taskNo, callhd.Id, "", ABCds.Tables[0].Rows[0]["type"].ToString(), "AutoJob", callhd.NeedStation, callhd.LocationCode, "", callhd.CallTime, "1", DateTime.Now, "AutoJob", TaskType.补给料框,"", callhd.LocationCode, MaterialConfirm.已确认);
                                dbHelp.DataOperator(intaskhdsql);
                            }
                            else
                            {//生成下料任务
                                SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@TASKTYPE", AGVTaskNo.成品下料), new SqlParameter("@NEW_ID", SqlDbType.VarChar, 32) };
                                parameters[1].Direction = ParameterDirection.Output;
                                taskNo = dbHelp.ProcOutput("Proc_GetTaskNo", parameters, "@NEW_ID").ToString();//生成任务号
                                //写入物料配送任务主表
                                string intaskhdsql = string.Format(@"INSERT INTO [dbo].[material_distribute_task_header] ([taskNo],[materialCallId],[productCode],[containerType],[userCode],[needStation],[locationCode],[containerCode],[needTime],[status],[createTime],[createBy],[callType],[startPlace],[EndPlace],[materialConfirm])
                                    VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}'); ",
                                            taskNo, callhd.Id, "", ABCds.Tables[0].Rows[0]["type"].ToString(), "AutoJob", callhd.NeedStation, callhd.LocationCode, ccode, callhd.CallTime, "1", DateTime.Now, "AutoJob", TaskType.下料取件, callhd.LocationCode,"", MaterialConfirm.已确认);
                                dbHelp.DataOperator(intaskhdsql);

                                //下料完成后，补给空料框任务
                                SqlParameter[] bparameters = new SqlParameter[] { new SqlParameter("@TASKTYPE", AGVTaskNo.补给空框), new SqlParameter("@NEW_ID", SqlDbType.VarChar, 32) };
                                bparameters[1].Direction = ParameterDirection.Output;
                                taskNo = dbHelp.ProcOutput("Proc_GetTaskNo", bparameters, "@NEW_ID").ToString();//生成任务号
                                //写入物料配送任务主表
                                string bintaskhdsql = string.Format(@"INSERT INTO [dbo].[material_distribute_task_header] ([taskNo],[materialCallId],[productCode],[containerType],[userCode],[needStation],[locationCode],[containerCode],[needTime],[status],[createTime],[createBy],[callType],[startPlace],[EndPlace],[materialConfirm])
                                    VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}'); ",
                                            taskNo, callhd.Id, "", ABCds.Tables[0].Rows[0]["type"].ToString(), "AutoJob", callhd.NeedStation, callhd.LocationCode, "", callhd.CallTime, "1", DateTime.Now, "AutoJob", TaskType.补给料框,"", callhd.LocationCode, MaterialConfirm.已确认);
                                dbHelp.DataOperator(bintaskhdsql);
                            }

                        }

                    }


                    
                }
            }
          
        }

        //public string GetTaskNo(string TaskType, int SeqLength = 4)
        //{
        //    string Value = "1";
        //    SysCount sysCount = _context.Set<SysCount>().AsNoTracking().FirstOrDefault(u => u.Type == TaskType);
        //    if (sysCount == null)
        //    {
        //        Value = TaskType + DateTime.Now.ToString("yyyyMMdd") + Value.PadLeft(SeqLength, '0');
        //        sysCount = new SysCount { Type = TaskType, Value = Value };
        //        _context.Set<SysCount>().Add(sysCount);
        //        _context.SaveChanges();
        //    }
        //    else
        //    {
        //        string Date = sysCount.Value.Substring(TaskType.Length, 8);

        //        if (Date == DateTime.Now.ToString("yyyyMMdd"))
        //        {
        //            Value = Date + (int.Parse(sysCount.Value.Substring(sysCount.Value.Length - 4, 4)) + 1).ToString().PadLeft(SeqLength, '0');
        //        }
        //        else
        //        {
        //            Value = DateTime.Now.ToString("yyyyMMdd") + Value.PadLeft(SeqLength, '0');
        //        }
        //        Value = TaskType + Value;
        //        _context.Set<SysCount>().Where(u => u.Type == TaskType).Update(u => new SysCount { Value = Value });
        //        _context.SaveChanges();
        //    }
        //    return Value;
        //}
    }
}
