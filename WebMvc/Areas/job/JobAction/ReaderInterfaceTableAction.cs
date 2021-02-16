using System;
using System.Collections.Generic;
using Infrastructure;
using System.Data;
using Quartz;
using WebRepository;
using System.Data.SqlClient;

namespace WebMvc
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class ReaderInterfaceTableAction
    {
        private string ConnString { set; get; }
        private IJobExecutionContext Context { set; get; }
        public string InterfaceConnString { set; get; }
        public ReaderInterfaceTableAction(string _ConnString, IJobExecutionContext _Context)
        {
            ConnString = _ConnString;
            Context = _Context;
        }

        public void Execute()
        {
            var config = AppSettingsJson.GetAppSettings();
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(config.GetSection("ConnectionStrings:InterfaceDBContext").Value);
            sqlConnectionStringBuilder.Password = Encryption.Decrypt(sqlConnectionStringBuilder.Password);
            InterfaceConnString = sqlConnectionStringBuilder.ConnectionString;
            DbHelp InterfaceDBHelp = new DbHelp(InterfaceConnString);

            DbHelp dbHelp = new DbHelp(ConnString);
            string  sql = string.Format(@"SELECT * FROM [dbo].[interface_order_header] where dbo.interface_order_header.transferMark = {0}; ", 0);
            DataSet ds = InterfaceDBHelp.SelectGet(sql);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string insql = string.Format(@"INSERT INTO [dbo].[interface_order_header] ([code],[machineType],[productCode],[partMaterialCode],[planQty],[createTime],[createBy])
                VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','Job'); ", dr["code"].ToString(),dr["machineType"].ToString(), dr["productCode"].ToString(), dr["partMaterialCode"].ToString(), dr["planQty"].ToString(), DateTime.Now);
                dbHelp.DataOperator(insql);

                string upsql = string.Format(@"UPDATE [dbo].[interface_order_header] set [transferMark]={0},[updatetime]='{1}',[updateBy]='Job' WHERE [id]='{2}'; ", 1, DateTime.Now, dr["id"].ToString());
                InterfaceDBHelp.DataOperator(upsql);

            }

        }
    }
}
