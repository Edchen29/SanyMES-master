using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApp;
using WebRepository;

namespace WebMvc.Areas.Api.Controllers
{
    /// <summary>
    /// 设备运行信息反馈接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentInfoController : ControllerBase
    {
        private readonly EquipmentInfoApp _app;

        /// <summary>
        /// 控制器构造函数
        /// </summary>
        /// <param name="unitWork">注入工作单元</param>
        /// <param name="auth">注入授权信息</param>
        /// <param name="context">EF上下文</param>
        public EquipmentInfoController(IUnitWork unitWork, IAuth auth, BaseDBContext context)
        {
            _app = new EquipmentInfoApp(unitWork, auth, context);
        }

        /// <summary>
        /// 设备运行节点反馈接口
        /// </summary>
        /// <param name="equipmentWorkNodeModel">设备运行数据</param>
        /// <returns></returns>
        [HttpPost("EquipmentWorkNode")]
        [ServiceFilter(typeof(InterfaceLogFilter))]
        public string EquipmentWorkNode([FromBody]EquipmentWorkNodeModel equipmentWorkNodeModel)
        {
            return JsonHelper.Instance.Serialize(_app.EquipmentWorkNodeApp(equipmentWorkNodeModel));
        }

    }
}