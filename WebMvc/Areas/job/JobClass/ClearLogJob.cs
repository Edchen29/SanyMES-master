using System;
using Quartz;
using Task = System.Threading.Tasks.Task;

namespace WebMvc
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class ClearLogJob : IJob
    {
        public virtual Task Execute(IJobExecutionContext context)
        {
            JobContainer jobContainer = new JobContainer(context);
            try
            {
                #region 执行任务语句
                ClearLogAction clearLogAction = new ClearLogAction(jobContainer.ConnString, context);
                clearLogAction.Execute();
                #endregion
            }
            catch (Exception ex)
            {
                jobContainer.ExceptionInfo = ex.Message;
            }

            jobContainer.LoggerJob();

            return Task.CompletedTask;
        }
    }
}
