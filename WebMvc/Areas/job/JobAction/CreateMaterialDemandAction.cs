using System;
using System.Collections.Generic;
using Infrastructure;
using System.Data;
using Quartz;
using WebRepository;

namespace WebMvc
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class CreateMaterialDemandAction
    {
        private string ConnString { set; get; }
        private IJobExecutionContext Context { set; get; }

        public CreateMaterialDemandAction(string _ConnString, IJobExecutionContext _Context)
        {
            ConnString = _ConnString;
            Context = _Context;
        }

        public void Execute()
        {
            DbHelp dbHelp = new DbHelp(ConnString);
            string sql = string.Format(@"SELECT dbo.order_header.code,dbo.order_header.productCode,dbo.order_header.partMaterialCode,dbo.order_header.planQty FROM dbo.order_header WHERE dbo.order_header.status = '{0}'; ", OrderStatus.已准备);
            DataSet ds = dbHelp.SelectGet(sql);

            List<OrderHeader> ohlist = new List<OrderHeader>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                OrderHeader orderheader = new OrderHeader();
                orderheader.Code = dr["code"].ToString();
                orderheader.ProductCode = dr["productCode"].ToString();
                if (string.IsNullOrEmpty(dr["partMaterialCode"].ToString()))
                {
                    orderheader.PartMaterialCode = dr["productCode"].ToString();
                }
                else
                {
                    orderheader.PartMaterialCode = dr["partMaterialCode"].ToString();
                }
                orderheader.PlanQty = decimal.Parse(dr["planQty"].ToString());
                ohlist.Add(orderheader);
            }
            if (ohlist.Count == ds.Tables[0].Rows.Count)
            {
                foreach (OrderHeader oh in ohlist)
                {

                    string sqlmd = string.Format(@"SELECT * FROM dbo.material_demand where dbo.material_demand.orderCode = '{0}'; ", oh.Code);
                    DataSet materialdemand = dbHelp.SelectGet(sqlmd);
                    if (materialdemand.Tables[0].Rows.Count < 1)
                    {
                        string sqls = string.Format(@"SELECT * FROM dbo.mbom_header where dbo.mbom_header.productCode= '{0}'; ", oh.ProductCode);
                        DataSet mbomh = dbHelp.SelectGet(sqls);
                        if (mbomh.Tables[0].Rows.Count > 0)
                        {
                            string sqlmbd = string.Format(@"SELECT * FROM dbo.mbom_detail where dbo.mbom_detail.mbomHeaderId = {0}; ",int.Parse(mbomh.Tables[0].Rows[0]["id"].ToString()));
                            DataSet mbomdt = dbHelp.SelectGet(sqlmbd);
                            if (mbomdt.Tables[0].Rows.Count > 0)
                            {

                                for (int i = 0; i < mbomdt.Tables[0].Rows.Count; i++)
                                {
                                    string materialsql = string.Format("SELECT dbo.material.classABC FROM dbo.material where dbo.material.code = '{0}'; ", mbomdt.Tables[0].Rows[i]["materialCode"].ToString());
                                    DataSet mateialds = dbHelp.SelectGet(materialsql);
                                    decimal? DamandQty = oh.PlanQty * decimal.Parse(mbomdt.Tables[0].Rows[i]["BaseQty"].ToString());


                                    string insql = string.Format(@"INSERT INTO [dbo].[material_demand] ([orderCode],[productCode],[partMaterialCode],[materialCode],[damandQty],[distributeQty],[status],[classABC],[createTime],[createBy])
                                    VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}'); ", 
                                    oh.Code, oh.ProductCode, oh.PartMaterialCode, mbomdt.Tables[0].Rows[i]["materialCode"].ToString(), DamandQty, decimal.Parse(mbomdt.Tables[0].Rows[i]["baseQty"].ToString()), "ready", mateialds.Tables[0].Rows[0]["classABC"].ToString(), DateTime.Now ,"AutoJob");
                                    dbHelp.DataOperator(insql);

                                }

                            }

                        }
                    }


                    
                }
            }
          
        }
    }
}
