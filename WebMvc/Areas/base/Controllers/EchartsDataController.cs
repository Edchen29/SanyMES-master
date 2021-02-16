using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using WebApp;
using WebRepository;

namespace WebMvc
{
    [Area("base")]
    public class EchartsDataController : Controller
    {
        private readonly EchartsDataApp _app;
        private Response Result = new Response();

        public EchartsDataController(IUnitWork unitWork)
        {
            _app = new EchartsDataApp(unitWork);
        }

        ///// <summary>
        ///// 获取库位利用率
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public string GetWareCellOption()
        //{
        //    try
        //    {
        //        return JsonHelper.Instance.Serialize(_app.GetWareCellCount());
        //    }
        //    catch (Exception ex)
        //    {
        //        Result.Status = false;
        //        Result.Message = ex.Message;
        //    }
        //    return JsonHelper.Instance.Serialize(Result);
        //}

        ///// <summary>
        ///// 最近7天每日收发货量
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public string GetWeekTaskInfo()
        //{

        //    try
        //    {
        //        return JsonHelper.Instance.Serialize(_app.GetWeekTask());
        //    }
        //    catch (Exception ex)
        //    {
        //        Result.Status = false;
        //        Result.Message = ex.Message;
        //    }
        //    return JsonHelper.Instance.Serialize(Result);
        //}

        ///// <summary>
        ///// 待维修信息
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public string GetRepairInfo()
        //{

        //    try
        //    {
        //        return JsonHelper.Instance.Serialize(_app.GetWaitRepair());
        //    }
        //    catch (Exception ex)
        //    {
        //        Result.Status = false;
        //        Result.Message = ex.Message;
        //    }
        //    return JsonHelper.Instance.Serialize(Result);
        //}

        ///// <summary>
        ///// 库存概况
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public string GetMaterielInfo()
        //{

        //    try
        //    {
        //        return JsonHelper.Instance.Serialize(_app.GetMaterielByGroup());

        //    }
        //    catch (Exception ex)
        //    {
        //        Result.Status = false;
        //        Result.Message = ex.Message;
        //    }
        //    return JsonHelper.Instance.Serialize(Result);
        //}
        /// <summary>
        /// 生产岛产品统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetProductInfo()
        {

            try
            {
                return JsonHelper.Instance.Serialize(_app.GetProduct());

            }
            catch (Exception ex)
            {
                Result.Status = false;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }
        /// <summary>
        /// 每日产能统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetOutPutInfo()
        {

            try
            {
                return JsonHelper.Instance.Serialize(_app.GetOutPut());

            }
            catch (Exception ex)
            {
                Result.Status = false;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }
        /// <summary>
        /// 工位效率分析
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetStationInfo()
        {

            try
            {
                return JsonHelper.Instance.Serialize(_app.GetStation());

            }
            catch (Exception ex)
            {
                Result.Status = false;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }
        /// <summary>
        /// 上料配送任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetDistributionInfo()
        {

            try
            {
                return JsonHelper.Instance.Serialize(_app.GetDistribution());

            }
            catch (Exception ex)
            {
                Result.Status = false;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }
    }
}