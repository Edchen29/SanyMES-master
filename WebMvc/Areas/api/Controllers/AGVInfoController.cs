using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApp;
using WebRepository;

namespace WebMvc.Areas.Api.Controllers
{
    /// <summary>
    /// 物料配送AGV接口(AGV调用：AGV ==> 中控系统)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AGVInfoController : ControllerBase
    {
        private readonly AGVInfoApp _app;

        /// <summary>
        /// 控制器构造函数
        /// </summary>
        /// <param name="unitWork">注入工作单元</param>
        /// <param name="auth">注入授权信息</param>
        /// <param name="context">EF上下文</param>
        public AGVInfoController(IUnitWork unitWork, IAuth auth, BaseDBContext context)
        {
            _app = new AGVInfoApp(unitWork, auth, context);
        }

        /// <summary>
        /// AGV任务节点反馈接口
        /// </summary>
        /// <param name="taskNodeModel">任务运行数据</param>
        /// <returns></returns>
        [HttpPost("AGVTaskNode")]
        [ServiceFilter(typeof(InterfaceLogFilter))]
        public string AGVTaskNode([FromBody]TaskNodeModel taskNodeModel)
        {
            return JsonHelper.Instance.Serialize(_app.AGVTaskNodeApp(taskNodeModel));
        }
        /// <summary>
        /// AGV运行信息上传1. AGV的路径轨迹上传2. AGV 的状态信息（空闲、忙、充电等）上传
        /// </summary>
        /// <param name="agvMonitor">AGV回传数据</param>
        /// <returns></returns>
        [HttpPost("AGVStateInfoUpload")]
        [ServiceFilter(typeof(InterfaceLogFilter))]
        public string AGVStateInfoUpload([FromBody]AgvMonitor agvMonitor)
        {
            return JsonHelper.Instance.Serialize(_app.AGVStateInfoUploadApp(agvMonitor));
        }
        /// <summary>
        /// AGV任务下发测试接口
        /// </summary>
        /// <param name="sendTaskModel">下发配送任务数据</param>
        /// <returns></returns>
        [HttpPost("AGVTaskTest")]
        [ServiceFilter(typeof(InterfaceLogFilter))]
        public string AGVTaskTest([FromBody]SendTaskModel sendTaskModel)
        {
            return JsonHelper.Instance.Serialize(_app.AGVTaskTestApp(sendTaskModel));
        }

    }
}