using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApp;
using WebRepository;

namespace WebMvc
{
    /// <summary>
	/// 操作日志
	/// </summary>
    [Area("monitor")]
    public class EquipmentMonitorController : BaseController
    {

        private readonly EquipmentMonitorApp _app;

        public EquipmentMonitorController(IAuth authUtil, EquipmentMonitorApp app) : base(authUtil)
        {
            _app = app.SetLoginInfo(_loginInfo);
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
        public string Load(PageReq pageRequest, EquipmentMonitor entity)
        {
            return JsonHelper.Instance.Serialize(_app.Load(pageRequest, entity));
        }
        #endregion
        #region 自定义方法

        #endregion
    }
}