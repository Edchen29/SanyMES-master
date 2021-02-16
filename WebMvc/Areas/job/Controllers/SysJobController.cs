using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using WebApp;
using WebRepository;

namespace WebMvc
{
    /// <summary>
	/// 定时任务调度表
	/// </summary>
    [Area("job")]
    public class SysJobController : BaseController
    {
        private readonly IScheduler _sched;
        private readonly SysJobApp _app;

        public SysJobController(IAuth authUtil, SysJobApp app, IScheduler sched) : base(authUtil)
        {
            _app = app.SetLoginInfo(_loginInfo);
            _sched = sched;
        }

        #region 视图功能
        /// <summary>
        /// 默认视图Action
        /// </summary>
        /// <returns></returns>
        [Authenticate]
        [ServiceFilter(typeof(OperLogFilter))]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 加载及分页查询
        /// </summary>
        /// <param name="pageRequest">表单请求信息</param>
        /// <param name="entity">请求条件实例</param>
        /// <returns></returns>
        [HttpPost]
        public string Load(PageReq pageRequest, SysJob entity)
        {
            return JsonHelper.Instance.Serialize(_app.Load(pageRequest, entity));
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="Table_entity">新增实例</param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(OperLogFilter))]
        public string Ins(SysJob Table_entity)
        {
            try
            {
                if (string.IsNullOrEmpty(Table_entity.MethodParams))
                {
                    throw new Exception("参数不能为空!");
                }

                if (string.IsNullOrEmpty(Table_entity.CronExpression))
                {
                    throw new Exception("cron表达式不能为空!");
                }

                if (Table_entity.Status == null)
                {
                    Table_entity.Status = "0";
                }
                if (Table_entity.Status == "0")
                {
                    CronExpression cronExpression = new CronExpression(Table_entity.CronExpression);
                    Table_entity.NextFireTime = cronExpression.GetNextValidTimeAfter(DateTime.Now).Value.ToLocalTime().DateTime;
                }

                Table_entity.JobGroup = Table_entity.JobName;
                _app.Ins(Table_entity);

                AddJob(Table_entity);
            }
            catch (Exception ex)
            {
                Result.Status = false;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="Table_entity">修改实例</param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(OperLogFilter))]
        public string Upd(SysJob Table_entity)
        {
            try
            {
                if (string.IsNullOrEmpty(Table_entity.MethodParams))
                {
                    throw new Exception("参数不能为空!");
                }

                if (string.IsNullOrEmpty(Table_entity.CronExpression))
                {
                    throw new Exception("cron表达式不能为空!");
                }

                if (Table_entity.Status == "0")
                {
                    CronExpression cronExpression = new CronExpression(Table_entity.CronExpression);
                    Table_entity.NextFireTime = cronExpression.GetNextValidTimeAfter(DateTime.Now).Value.ToLocalTime().DateTime;
                }

                _app.Upd(Table_entity);

                if (Table_entity.Status == "0")
                {
                    UpdateJob(Table_entity);
                }   
            }
            catch (Exception ex)
            {
                Result.Status = false;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        /// <summary>
        /// 暂停计划/启用计划
        /// </summary>
        /// <param name="Table_entity">暂停计划/启用计划</param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(OperLogFilter))]
        public string PauseOrResume(SysJob Table_entity)
        {
            try
            {
                if (Table_entity.Status == "0")
                {
                    Table_entity.Status = "1";
                    Upd(Table_entity);

                    DeleteJob(Table_entity);
                }
                else
                {
                    Table_entity.Status = "0";
                    Upd(Table_entity);

                    AddJob(Table_entity);
                }
            }
            catch (Exception ex)
            {
                Result.Status = false;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        [HttpPost]
        [ServiceFilter(typeof(OperLogFilter))]
        public string DelByIds(int[] ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    SysJob Table_entity = _app.FindSingle(u => u.Id == item);
                    _app.DelByIds(new int[] { item});
                    DeleteJob(Table_entity);
                }
            }
            catch (Exception ex)
            {
                Result.Status = false;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }
        #endregion

        #region 自定义方法

        #region 增加计划
        /// <summary>
        /// 新增定时器任务
        /// </summary>
        /// <param name="Table_entity">任务实体</param>
        private void AddJob(SysJob Table_entity)
        {
            try
            {
                var jobDataMap = new JobDataMap();
                string methodParams = Table_entity.MethodParams;
                JObject json = null;
                json = JObject.Parse(methodParams);
                if (json != null)
                {
                    foreach (var item in json)
                    {
                        jobDataMap.Add(item.Key, item.Value.ToString());
                    }
                }

                string methodName = Table_entity.MethodName ?? "";
                IJobDetail job = null;
                if (methodName.ToLower().Equals("ClearLogJob".ToLower()))
                {
                    job = JobBuilder.Create<ClearLogJob>().WithIdentity(Table_entity.JobName).UsingJobData(jobDataMap).Build();
                }
                else if (methodName.ToLower().Equals("GetCurrentStockJob".ToLower()))
                {
                    job = JobBuilder.Create<GetCurrentStockJob>().WithIdentity(Table_entity.JobName).UsingJobData(jobDataMap).Build();
                }
                else if (methodName.ToLower().Equals("GetICSInventoryJob".ToLower()))
                {
                    job = JobBuilder.Create<GetICSInventoryJob>().WithIdentity(Table_entity.JobName).UsingJobData(jobDataMap).Build();
                }
                else if (methodName.ToLower().Equals("AGVJob".ToLower()))
                {
                    job = JobBuilder.Create<AGVJob>().WithIdentity(Table_entity.JobName).UsingJobData(jobDataMap).Build();
                }else if (methodName.ToLower().Equals("OrderAlertJob".ToLower()))
                {
                    job = JobBuilder.Create<OrderAlertJob>().WithIdentity(Table_entity.JobName).UsingJobData(jobDataMap).Build();
                }else if (methodName.ToLower().Equals("CreateMaterialDemandJob".ToLower()))
                {
                    job = JobBuilder.Create<CreateMaterialDemandJob>().WithIdentity(Table_entity.JobName).UsingJobData(jobDataMap).Build();
                }else if (methodName.ToLower().Equals("CreateDistributeTaskJob".ToLower()))
                {
                    job = JobBuilder.Create<CreateDistributeTaskJob>().WithIdentity(Table_entity.JobName).UsingJobData(jobDataMap).Build();
                }else if (methodName.ToLower().Equals("SendAGVTaskJob".ToLower()))
                {
                    job = JobBuilder.Create<SendAGVTaskJob>().WithIdentity(Table_entity.JobName).UsingJobData(jobDataMap).Build();
                }
                else if (methodName.ToLower().Equals("ClearFinishOrderJob".ToLower()))
                {
                    job = JobBuilder.Create<ClearFinishOrderJob>().WithIdentity(Table_entity.JobName).UsingJobData(jobDataMap).Build();
                }
                else if (methodName.ToLower().Equals("ReaderInterfaceTableJob".ToLower()))
                {
                    job = JobBuilder.Create<ReaderInterfaceTableJob>().WithIdentity(Table_entity.JobName).UsingJobData(jobDataMap).Build();
                }

                if (job != null)
                {
                    var trigger = TriggerBuilder.Create().WithIdentity(Table_entity.JobName).WithCronSchedule(Table_entity.CronExpression).Build();
                    _sched.ScheduleJob(job, trigger).Wait();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region 删除计划
        /// <summary>
        /// 删除定时器任务
        /// </summary>
        /// <param name="Table_entity">任务实体</param>
        private void DeleteJob(SysJob Table_entity)
        {
            try
            {
                _sched.PauseTrigger(new TriggerKey(Table_entity.JobName)).Wait();
                _sched.UnscheduleJob(new TriggerKey(Table_entity.JobName)).Wait();
                _sched.DeleteJob(new JobKey(Table_entity.JobName)).Wait();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region 修改计划
        /// <summary>
        /// 修改定时器任务
        /// </summary>
        /// <param name="Table_entity">任务实体</param>
        private void UpdateJob(SysJob Table_entity)
        {
            DeleteJob(Table_entity);
            AddJob(Table_entity);
        }
        #endregion

        #endregion
    }
}