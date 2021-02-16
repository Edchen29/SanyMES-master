using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApp;
using WebRepository;

namespace WebMvc.Areas.Api.Controllers
{
    /// <summary>
    /// 基础数据同步接口(中控系统被动推送 MES ==> 中控系统)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BasicDataController : ControllerBase
    {
        private readonly BasicDataApp _app;

        /// <summary>
        /// 控制器构造函数
        /// </summary>
        /// <param name="unitWork">注入工作单元</param>
        /// <param name="auth">注入授权信息</param>
        /// <param name="context">EF上下文</param>
        public BasicDataController(IUnitWork unitWork, IAuth auth, BaseDBContext context)
        {
            _app = new BasicDataApp(unitWork, auth, context);
        }

        /// <summary>
        /// 物料添加公共接口
        /// </summary>
        /// <param name="Materials">物料数据列表</param>
        /// <returns></returns>
        [HttpPost("Material")]
        [ServiceFilter(typeof(InterfaceLogFilter))]
        public string Material([FromBody]MaterialsModel Materials)
        {
            return JsonHelper.Instance.Serialize(_app.Material(Materials));
        }
        /// <summary>
        /// 产品信息同步接口
        /// </summary>
        /// <param name="interfaceproduct">产品数据列表</param>
        /// <returns></returns>
        [HttpPost("TransmitProduct")]
        [ServiceFilter(typeof(InterfaceLogFilter))]
        public string TransmitProduct([FromBody]InterfaceProductModel interfaceproduct)
        {
            return JsonHelper.Instance.Serialize(_app.InsertProduct(interfaceproduct));
        }
    }
}