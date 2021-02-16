using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using HslCommunication.Enthernet;
using Infrastructure;
using Quartz;
using WebRepository;

namespace WebMvc
{
    public class JobContainer
    {
        public static NetPushServer pushServer = null;

        private IJobExecutionContext Context { set; get; }
        public string ConnString { set; get; }
        public string JobName { set; get; }
        public string JobGroup { set; get; }
        public string MethodName { set; get; }
        public string MethodParams { set; get; }
        public string JobMessage { set; get; }
        public string ExceptionInfo { set; get; }
        public DateTime? LastFireTime { set; get; }
        public DateTime? NextFireTime { set; get; }

        private Stopwatch stopwatch = new Stopwatch();

        public JobContainer(IJobExecutionContext _Context)
        {
            var config = AppSettingsJson.GetAppSettings();

            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(config.GetSection("ConnectionStrings:BaseDBContext").Value);
            sqlConnectionStringBuilder.Password = Encryption.Decrypt(sqlConnectionStringBuilder.Password);
            ConnString = sqlConnectionStringBuilder.ConnectionString;

            stopwatch.Start();
            Context = _Context;

            JobName = JobGroup = MethodName = MethodParams = JobMessage = ExceptionInfo = "";
            JobName = Context.JobDetail.Key.Name;
            InitContainer();
        }

        public void InitContainer()
        {
            string sql = "";

            DbHelp dbHelp = new DbHelp(ConnString);

            try
            {
                sql = string.Format("SELECT [jobName] ,[jobGroup] ,[methodName] ,[methodParams] ,[cronExpression] FROM [dbo].[sys_job] WHERE jobName = '{0}';", JobName);
                DataSet dataSet = dbHelp.SelectGet(sql);
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    JobGroup = dataSet.Tables[0].Rows[0]["jobGroup"].ToString();
                    MethodName = dataSet.Tables[0].Rows[0]["methodName"].ToString();
                    MethodParams = dataSet.Tables[0].Rows[0]["methodParams"].ToString();
                }

                LastFireTime = TimeZoneInfo.ConvertTimeFromUtc(Context.FireTimeUtc.DateTime, TimeZoneInfo.Local);
                NextFireTime = TimeZoneInfo.ConvertTimeFromUtc(Context.NextFireTimeUtc.Value.DateTime, TimeZoneInfo.Local);
            }
            catch (Exception ex)
            {
                ExceptionInfo = ex.Message;
            }
        }

        public void LoggerJob()
        {
            string sql = "";
            DbHelp dbHelp = new DbHelp(ConnString);

            #region 更新任务时间
            sql = string.Format("UPDATE [dbo].[sys_job] SET lastFireTime = '{0}', nextFireTime = '{1}' WHERE jobName = '{2}';", LastFireTime, NextFireTime, JobName);
            dbHelp.DataOperator(sql);
            #endregion

            #region 记录任务日志
            stopwatch.Stop();
            JobMessage = "总共耗时:" + stopwatch.Elapsed.TotalMilliseconds.ToString() + " 毫秒";

            JobName = JobName.Replace("'", "''");
            JobGroup = JobGroup.Replace("'", "''");
            MethodName = MethodName.Replace("'", "''");
            MethodParams = MethodParams.Replace("'", "''");
            JobMessage = JobMessage.Replace("'", "''");
            ExceptionInfo = ExceptionInfo.Replace("'", "''");

            sql = string.Format(@"INSERT INTO [dbo].[sys_job_log]
                        ([jobName]
                        ,[jobGroup]
                        ,[methodName]
                        ,[methodParams]
                        ,[jobMessage]
                        ,[exceptionInfo]
                        ,[createTime]
                        ,[createBy])
                    VALUES
                        ('{0}'
                        ,'{1}'
                        ,'{2}'
                        ,'{3}'
                        ,'{4}'
                        ,'{5}'
                        , GETDATE()
                        , 'System')", JobName, JobGroup, MethodName, MethodParams, JobMessage, ExceptionInfo);
            dbHelp.DataOperator(sql);
            #endregion
        }

        public static void InitBroadCaster()
        {
            if (pushServer == null)
            {
                pushServer = new NetPushServer();
                pushServer.ServerStart(23467);
            }
        }
    }
}
