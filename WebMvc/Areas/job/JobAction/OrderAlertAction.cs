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
    public class OrderAlertAction
    {
        private string ConnString { set; get; }
        private IJobExecutionContext Context { set; get; }

        public OrderAlertAction(string _ConnString, IJobExecutionContext _Context)
        {
            ConnString = _ConnString;
            Context = _Context;
        }

        public void Execute()
        {
            DbHelp dbHelp = new DbHelp(ConnString);
            string  sql = string.Format(@"SELECT dbo.order_header.code,dbo.order_header.productCode,dbo.order_header.partMaterialCode,dbo.order_header.status,
                          (SELECT ISNULL(dbo.order_detiail.serialNumber, '') FROM dbo.order_detiail WHERE dbo.order_header.code=dbo.order_detiail.orderCode) AS serialNumber
                           FROM dbo.order_header WHERE dbo.order_header.status <> '{0}' AND dbo.order_header.status <> '{1}' AND dbo.order_header.status <> '{2}' AND dbo.order_header.planEndTime < GETDATE()
                           ORDER BY dbo.order_header.code ASC,dbo.order_header.planStartTime ASC; ", OrderStatus.完成,OrderStatus.已冻结,OrderStatus.已撤消);
            DataSet ds = dbHelp.SelectGet(sql);
            List<OrderAlert> oalist = new List<OrderAlert>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                OrderAlert orderAlert = new OrderAlert();
                orderAlert.OrderCode = dr["code"].ToString();
                orderAlert.ProductCode = dr["productCode"].ToString();
                orderAlert.PartMaterialCode = dr["partMaterialCode"].ToString();
                orderAlert.SerialNumber = dr["serialNumber"].ToString();
                orderAlert.AlertMsg = "已逾期未按计划时间完成生产";
                orderAlert.Flag = 0;
                orderAlert.IsSpeak = true;
                orderAlert.Status = dr["status"].ToString();
                oalist.Add(orderAlert);
            }
            if (oalist.Count == ds.Tables[0].Rows.Count)
            {
                foreach (OrderAlert oalt in oalist)
                {
                  string  sqls = string.Format(@"SELECT top 1 orderCode FROM dbo.order_alert WHERE dbo.order_alert.orderCode = '{0}' AND Flag=0; ", oalt.OrderCode);
                    DataSet dsalert = dbHelp.SelectGet(sqls);
                    if (dsalert.Tables[0].Rows.Count < 1)
                    {
                       string insql = string.Format(@"INSERT INTO [dbo].[order_alert] ([orderCode],[productCode],[partMaterialCode],[serialNumber],[status],[AlertMsg],[Flag],[IsSpeak],[createTime],[createBy])
                       VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','Job'); ", oalt.OrderCode, oalt.ProductCode, oalt.PartMaterialCode, oalt.SerialNumber, oalt.Status, oalt.AlertMsg, oalt.Flag, oalt.IsSpeak, DateTime.Now);
                        dbHelp.DataOperator(insql);

                    }
                }
            }
            string donesql = string.Format(@"SELECT dbo.order_header.code,dbo.order_header.productCode,dbo.order_header.partMaterialCode,dbo.order_header.status,
                             (SELECT ISNULL(dbo.order_detiail.serialNumber, '') FROM dbo.order_detiail WHERE dbo.order_header.code=dbo.order_detiail.orderCode) AS serialNumber
                              FROM dbo.order_header WHERE (dbo.order_header.status = '{0}' OR dbo.order_header.status='{1}' OR dbo.order_header.status='{2}') AND dbo.order_header.planEndTime < GETDATE()
                              ORDER BY dbo.order_header.code ASC,dbo.order_header.planStartTime ASC; ", OrderStatus.完成, OrderStatus.已冻结, OrderStatus.已撤消);
            DataSet doneds = dbHelp.SelectGet(donesql);
            List<OrderAlert> orderAlerts = new List<OrderAlert>();
            foreach (DataRow dr in doneds.Tables[0].Rows)
            {
                OrderAlert orderAlert = new OrderAlert();
                orderAlert.OrderCode = dr["code"].ToString();
                //orderAlert.ProductCode = dr["productCode"].ToString();
                //orderAlert.PartMaterialCode = dr["partMaterialCode"].ToString();
                //orderAlert.SerialNumber = dr["serialNumber"].ToString();
                orderAlert.Flag = 1;
                orderAlert.IsSpeak = false;
                orderAlert.Status = dr["status"].ToString();
                orderAlerts.Add(orderAlert);
            }
            if (orderAlerts.Count == ds.Tables[0].Rows.Count)
            {
                foreach (OrderAlert oalt in orderAlerts)
                {
                    string sqls = string.Format(@"SELECT top 1 orderCode FROM dbo.order_alert WHERE dbo.order_alert.orderCode = '{0}' AND Flag=0; ", oalt.OrderCode);
                    DataSet dsalert = dbHelp.SelectGet(sqls);
                    if (dsalert.Tables[0].Rows.Count > 0)
                    {
                        string upsql = string.Format(@"UPDATE [dbo].[order_alert] set [status]='{0}', [Flag]='{1}',[IsSpeak]='{2}',[updatetime]='{4}',[updateBy]='Job' WHERE [orderCode]='{3}'; ", oalt.Status, oalt.Flag, oalt.IsSpeak, oalt.OrderCode,DateTime.Now);
                        dbHelp.DataOperator(upsql);

                    }
                }
            }
        }
    }
}
