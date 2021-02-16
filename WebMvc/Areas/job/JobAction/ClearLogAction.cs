using System;
using Quartz;
using WebRepository;

namespace WebMvc
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class ClearLogAction
    {
        private string ConnString { set; get; }
        private IJobExecutionContext Context { set; get; }

        public ClearLogAction(string _ConnString, IJobExecutionContext _Context)
        {
            ConnString = _ConnString;
            Context = _Context;
        }

        public void Execute()
        {
            string sql = "";
            string LogName = Context.JobDetail.JobDataMap.GetString("LogName");
            int Days = Context.JobDetail.JobDataMap.GetInt("Days");

            DbHelp dbHelp = new DbHelp(ConnString);
            #region 执行任务语句
            sql = string.Format("DELETE FROM [dbo].[sys_oper_log] WHERE createTime < '{0}';", DateTime.Now.AddDays(-1 * double.Parse(Days.ToString())));
            dbHelp.DataOperator(sql);

            sql = string.Format("DELETE FROM [dbo].[sys_job_log] WHERE createTime < '{0}';", DateTime.Now.AddDays(-1 * double.Parse(Days.ToString())));
            dbHelp.DataOperator(sql);
            #endregion
        }
    }
}
