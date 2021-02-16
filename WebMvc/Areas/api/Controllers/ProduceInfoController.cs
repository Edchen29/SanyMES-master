using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApp;
using WebRepository;

namespace WebMvc.Areas.Api.Controllers
{
    /// <summary>
    /// 生产管理接口(MES调用主动推送：MES ==> 中控系统)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProduceInfoController : ControllerBase
    {
        private readonly ProduceInfoApp _app;

        /// <summary>
        /// 控制器构造函数
        /// </summary>
        /// <param name="unitWork">注入工作单元</param>
        /// <param name="auth">注入授权信息</param>
        /// <param name="context">EF上下文</param>
        public ProduceInfoController(IUnitWork unitWork, IAuth auth, BaseDBContext context)
        {
            _app = new ProduceInfoApp(unitWork, auth, context);
        }

        /// <summary>
        /// 订单信息同步接口
        /// </summary>
        /// <param name="interfaceorder">生产工单数据列表</param>
        /// <returns></returns>
        [HttpPost("TransmitOrder")]
        [ServiceFilter(typeof(InterfaceLogFilter))]
        public string TransmitOrder([FromBody]InterfaceOrderModel interfaceorder)
        {
            return JsonHelper.Instance.Serialize(_app.InsertOrder(interfaceorder));
        }

        /// <summary>
        /// 生产MBom信息同步接口
        /// </summary>
        /// <param name="interfacembom">生产MBom数据列表</param>
        /// <returns></returns>
        [HttpPost("TransmitMBom")]
        [ServiceFilter(typeof(InterfaceLogFilter))]
        public string TransmitMBom([FromBody]InterfaceMBomModel interfacembom)
        {
            return JsonHelper.Instance.Serialize(_app.InsertMBom(interfacembom));
        }
        /// <summary>
        /// 订单修正接口
        /// </summary>
        /// <param name="revisemodel">修正数据列表</param>
        /// <returns></returns>
        [HttpPost("OrderRevise")]
        [ServiceFilter(typeof(InterfaceLogFilter))]
        public string OrderRevise([FromBody]ReviseModel revisemodel)
        {
            return JsonHelper.Instance.Serialize(_app.OrderReviseApp(revisemodel));
        }
        /// <summary>
        /// 质检判定接口
        /// </summary>
        /// <param name="qcCheckModel">修正数据列表</param>
        /// <returns></returns>
        [HttpPost("QCCheck")]
        [ServiceFilter(typeof(InterfaceLogFilter))]
        public string QCCheck([FromBody]QCCheckModel qcCheckModel)
        {
            return JsonHelper.Instance.Serialize(_app.QCCheckApp(qcCheckModel));
        }
        /// <summary>
        /// 配料结果回传接口
        /// </summary>
        /// <param name="materialDemandConfirm">配料回传信息</param>
        /// <returns></returns>
        [HttpPost("StockConfirm")]
        [ServiceFilter(typeof(InterfaceLogFilter))]
        public string StockConfirm([FromBody]MaterialDemandConfirmModel materialDemandConfirm)
        {
            return JsonHelper.Instance.Serialize(_app.StockConfirmApp(materialDemandConfirm));
        }
    }
}