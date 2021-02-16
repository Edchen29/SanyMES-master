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
    public class ClearFinishOrderAction
    {
        private string ConnString { set; get; }
        private IJobExecutionContext Context { set; get; }

        public ClearFinishOrderAction(string _ConnString, IJobExecutionContext _Context)
        {
            ConnString = _ConnString;
            Context = _Context;
        }

        public void Execute()
        {
            DbHelp dbHelp = new DbHelp(ConnString);
            string sql = string.Format(@"SELECT * FROM dbo.order_header WHERE dbo.order_header.status = '{0}'; ", OrderStatus.完成);
            DataSet ds = dbHelp.SelectGet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //获取工单历史表,确定历史表中有此条数据
                    string orderhsql = string.Format(@"SELECT * FROM dbo.order_header_history WHERE dbo.order_header_history.code = '{0}'; ", dr["code"]);
                    DataSet ohds = dbHelp.SelectGet(orderhsql);
                    if (ohds.Tables[0].Rows.Count > 0)
                    {
                        string deldetail = string.Format(@"DELETE FROM dbo.order_detiail WHERE dbo.order_detiail.orderHeaderId = {0}; ",dr["id"]);
                        dbHelp.DataOperator(deldetail);

                        string delheader = string.Format(@"DELETE FROM dbo.order_header WHERE dbo.order_header.id = {0}; ", dr["id"]);
                        dbHelp.DataOperator(delheader);
                    }
                }
            }

          
        }

    }
}
