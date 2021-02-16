using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebApp;
using WebRepository;

namespace WebMvc
{
    /// <summary>
	/// 库位监控
	/// </summary>
    [Area("monitor")]
    public class LocationMonitorController : BaseController
    {
        private readonly LocationMonitorApp _app;

        public LocationMonitorController(IAuth authUtil, LocationMonitorApp app) : base(authUtil)
        {
            _app = app;
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


        #region 自定义方法
        /// <summary>
        /// 获取巷道
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetRoadWays()
        {
            return JsonHelper.Instance.Serialize(_app.GetRoadWays());
        }

        /// <summary>
        /// 获取巷道设定
        /// </summary>
        /// <param name="roadway">巷道</param>
        /// <returns></returns>
        [HttpPost]
        public string GetRoadWayConfig(int roadway)
        {
            return JsonHelper.Instance.Serialize(_app.GetRoadWayConfig(roadway));
        }

        /// <summary>
        /// 获取库位信息
        /// </summary>
        /// <param name="roadway">巷道</param>
        /// <returns></returns>
        [HttpPost]
        public string GetLocations(int roadway)
        {
            return JsonHelper.Instance.Serialize(_app.GetLocations(roadway));
        }
        #endregion
    }
}