using System;
using System.Collections.Generic;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApp;
using WebRepository;

namespace WebMvc.Areas.Api.Controllers
{
    /// <summary>
    /// 数据查询接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class IInventoryController : ControllerBase
    {
        private readonly IInventoryApp _app;

        /// <summary>
        /// 控制器构造函数
        /// </summary>
        /// <param name="unitWork">注入工作单元</param>
        /// <param name="auth">注入授权信息</param>
        /// <param name="context">EF上下文</param>
        public IInventoryController(IUnitWork unitWork, IAuth auth, BaseDBContext context)
        {
            _app = new IInventoryApp(unitWork, auth, context);
        }

        /// <summary>
        /// 库存查询接口
        /// </summary>
        /// <param name="inventoryModel">查询条件</param>
        /// <returns></returns>
        [HttpPost("GetCurrentStock")]
        [ServiceFilter(typeof(InterfaceLogFilter))]
        public string GetCurrentStock([FromBody]InventoryModel inventoryModel)
        {
            return JsonHelper.Instance.Serialize(_app.GetCurrentStock(inventoryModel));
        }
        /// <summary>
        /// 不良代号接口
        /// </summary>
        /// <param name="defectCodeModel">查询条件</param>
        /// <returns></returns>
        [HttpPost("GetDefectCode")]
        [ServiceFilter(typeof(InterfaceLogFilter))]
        public string GetDefectCode([FromBody]DefectCodeModel defectCodeModel)
        {
            return JsonHelper.Instance.Serialize(_app.GetDefectCodeApp(defectCodeModel));
        }
        /// <summary>
        /// 人员判定数统计接口
        /// </summary>
        /// <param name="repair">查询条件</param>
        /// <returns></returns>
        [HttpPost("GetCheckCount")]
        [ServiceFilter(typeof(InterfaceLogFilter))]
        public string GetCheckCount([FromBody]Repair repair)
        {
            return JsonHelper.Instance.Serialize(_app.GetCheckCountApp(repair));
        }
    }
}