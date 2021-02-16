using System;
using Quartz;
using Task = System.Threading.Tasks.Task;

namespace WebMvc
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class CreateMaterialDemandJob : IJob
    {
        public virtual Task Execute(IJobExecutionContext context)
        {
            JobContainer jobContainer = new JobContainer(context);
            try
            {
                #region 执行任务语句
                CreateMaterialDemandAction createMaterialDemandAction = new CreateMaterialDemandAction(jobContainer.ConnString, context);
                createMaterialDemandAction.Execute();
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
