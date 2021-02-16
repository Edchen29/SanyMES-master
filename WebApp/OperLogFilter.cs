using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Diagnostics;
using WebRepository;

namespace WebApp
{
    public class OperLogFilter : ActionFilterAttribute
    {
        private string ActionArguments { get; set; }
        private Stopwatch Stopwatch { get; set; }

        protected readonly IRepository<SysOperLog> _app;

        public OperLogFilter(IRepository<SysOperLog> app, IAuth authUtil)
        {
            _app = app;
            AuthStrategyContext authStrategyContext = authUtil.GetCurrentUser();
            if (authStrategyContext != null)
            {
                _app._loginInfo = new LoginInfo
                {
                    Id = authStrategyContext.User.Id,
                    Account = authStrategyContext.User.Account,
                    Name = authStrategyContext.User.Name,
                };
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            ActionArguments = Newtonsoft.Json.JsonConvert.SerializeObject(filterContext.ActionArguments);

            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            Stopwatch.Stop();

            IHeaderDictionary headersDictionary = context.HttpContext.Request.Headers;
            string request = headersDictionary[HeaderNames.UserAgent].ToString();

            string url = context.HttpContext.Request.Host + context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;

            var description =
                (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor;

            var actionName = description.ActionName;

            Dictionary<string, string> OperType = new Dictionary<string, string>();
            OperType.Add("Index", "查看");
            OperType.Add("Ins", "增加");
            OperType.Add("Upd", "更新");
            OperType.Add("DelByIds", "删除");

            var otype = "";

            if (OperType.ContainsKey(actionName))
            {
                otype = OperType[actionName];
            }
            else
            {
                otype = "其他";
            }
                    
            string method = context.HttpContext.Request.Method;

            string Name = _app._loginInfo.Name;
            string Ip = context.HttpContext.Connection.RemoteIpAddress.ToString();

            string parameter = ActionArguments.Replace("\r", "").Replace("\n", "");
            dynamic result = (context.Result == null ? null : (context.Result.GetType().Name == "EmptyResult" ? new { Value = "无返回结果" } : context.Result as dynamic));

            string response = "在返回结果前发生了异常";
            try
            {
                if (result != null)
                {
                    response = Newtonsoft.Json.JsonConvert.SerializeObject(result.Value).Replace("\r", "").Replace("\n", "");
                }
            }
            catch (System.Exception)
            {
                response = "";
            }

            _app.Add(new SysOperLog()
            {
                Url = url,
                OperType = otype,
                Method = method,
                Request = request,
                Parameter = parameter,
                Response = response,
                TotalMilliseconds = Stopwatch.Elapsed.TotalMilliseconds,
                LogTime = System.DateTime.Now,
                Name = Name,
                Ip = Ip
            }); ;
        }
    }
}
