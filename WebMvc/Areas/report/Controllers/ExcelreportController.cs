using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApp;
using WebRepository;

namespace WebMvc
{
    /// <summary>
	/// ExcelReport
	/// </summary>
    [Area("report")]
    public class ExcelReportController : BaseController
    {
        private readonly ExcelReportApp _app;

        /// <summary>
        /// 控制器构造函数
        /// </summary>
        /// <param name="sqlWork">注入Sql工作单元</param>
        /// <param name="unitWork">注入工作单元</param>
        /// <param name="authUtil">注入授权信息</param>
        public ExcelReportController(ISqlWork sqlWork, IUnitWork unitWork, IAuth authUtil) : base(authUtil)
        {
            _app = new ExcelReportApp(sqlWork, unitWork);
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
        /// 获取报表结果
        /// </summary>
        /// <param name="ReportId">报表编号</param>
        /// <param name="Filter">筛选条件</param>
        /// <returns></returns>
        public string QueryData(int ReportId, string Filter)
        {
            return JsonHelper.Instance.Serialize(_app.QueryData(ReportId, Filter));
        }
        #endregion
    }
}