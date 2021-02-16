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
using WebApp;

namespace WebMvc
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class SendAGVTaskAction
    {
        private string ConnString { set; get; }
        private IJobExecutionContext Context { set; get; }

        public SendAGVTaskAction(string _ConnString, IJobExecutionContext _Context)
        {
            ConnString = _ConnString;
            Context = _Context;
        }

        public void Execute()
        {
            DbHelp dbHelp = new DbHelp(ConnString);
            string sql = string.Format(@"SELECT * FROM [dbo].[material_distribute_task_header] WHERE [dbo].[material_distribute_task_header].status = {0} order by id; ", AGVTaskState.任务生成);
            DataSet ds = dbHelp.SelectGet(sql);
            List<MaterialDistributeTaskHeader> tasklist = new List<MaterialDistributeTaskHeader>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                MaterialDistributeTaskHeader taskmodel = new MaterialDistributeTaskHeader();
                taskmodel.TaskNo = dr["taskNo"].ToString();
                taskmodel.ProductCode = dr["productCode"].ToString();
                taskmodel.ContainerType = dr["containerType"].ToString();
                taskmodel.NeedStation = dr["needStation"].ToString();
                taskmodel.LocationCode = dr["locationCode"].ToString();
                taskmodel.NeedTime = DateTime.Parse(dr["needTime"].ToString());
                taskmodel.CallType = dr["callType"].ToString();
                tasklist.Add(taskmodel);
            }
            if (tasklist.Count == ds.Tables[0].Rows.Count)
            {
                foreach (MaterialDistributeTaskHeader taskhd in tasklist)
                {
                    string splitsql = string.Format(@"SELECT * FROM [dbo].[material_distribute_task_header] WHERE [dbo].[material_distribute_task_header].status != {0} and [dbo].[material_distribute_task_header].status != {1} and [locationCode] = '{2}'; ", AGVTaskState.任务完成, AGVTaskState.任务生成, taskhd.LocationCode);
                    DataSet splitds = dbHelp.SelectGet(splitsql);
                    if (splitds.Tables[0].Rows.Count < 1)
                    {
                        //调用AGV接口下发配料任务
                        ApiRequest apiRequest = new ApiRequest("AGV", true);
                        WCSResponse<MaterialDistributeTaskHeader> _WCSResponse = apiRequest.Post<WCSResponse<MaterialDistributeTaskHeader>>(JsonHelper.Instance.Serialize(taskhd), "AGVInfo/AGVTaskTest", "AGV任务下发");
                        if (_WCSResponse.Code == 200)
                        {
                            string upssql = string.Format("update [dbo].[material_distribute_task_header] set [dbo].[material_distribute_task_header].status = {1}, [dbo].[material_distribute_task_header].[updateTime] = '{2}', [dbo].[material_distribute_task_header].[updateBy] = '{3}' WHERE  [dbo].[material_distribute_task_header].[taskNo] = '{0}';", taskhd.TaskNo, AGVTaskState.任务下发, DateTime.Now, "AutoJob");
                            dbHelp.DataOperator(upssql);
                            //string emptysql = string.Format(@"SELECT * FROM dbo.location where dbo.location.code = '{0}'; ", taskhd.LocationCode);
                            //DataSet emptyds = dbHelp.SelectGet(emptysql);
                            //if (string.IsNullOrEmpty(emptyds.Tables[0].Rows[0]["containerCode"].ToString()))
                            //{
                            //    string upssql = string.Format("update [dbo].[material_distribute_task_header] set [dbo].[material_distribute_task_header].status = {1}, [dbo].[material_distribute_task_header].[updateTime] = '{2}', [dbo].[material_distribute_task_header].[updateBy] = '{3}' WHERE  [dbo].[material_distribute_task_header].[taskNo] = '{0}';", taskhd.TaskNo, AGVTaskState.配送物料开始,DateTime.Now,"AutoJob");
                            //    dbHelp.DataOperator(upssql);
                            //}
                            //else
                            //{
                            //    string upssql = string.Format("update [dbo].[material_distribute_task_header] set [dbo].[material_distribute_task_header].status = {1}, [dbo].[material_distribute_task_header].[updateTime] = '{2}', [dbo].[material_distribute_task_header].[updateBy] = '{3}'  WHERE  [dbo].[material_distribute_task_header].[taskNo] = '{0}';", taskhd.TaskNo, AGVTaskState.回收料框, DateTime.Now, "AutoJob");
                            //    dbHelp.DataOperator(upssql);
                            //}
                        }
                    }
                    
                }
            }
          
        }

    }
}
