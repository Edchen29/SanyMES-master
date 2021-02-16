using System;
using Quartz;
using Task = System.Threading.Tasks.Task;

namespace WebMvc
{
    //[DisallowConcurrentExecution]
    public class AGVJob : IJob
    {
        public virtual Task Execute(IJobExecutionContext context)
        {
            JobContainer jobContainer = new JobContainer(context);
            try
            {
                #region 执行任务语句
                AGVAction aGVAction = new AGVAction(jobContainer.ConnString, context);
                aGVAction.Execute(jobContainer);
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
