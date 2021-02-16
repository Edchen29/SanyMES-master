using System;
using System.Collections.Generic;
using System.Diagnostics;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using WebRepository;

namespace WebApp
{
    public class InterfaceLogFilter : ActionFilterAttribute
    {
        private string ActionArguments { get; set; }
        private Stopwatch Stopwatch { get; set; }

        protected readonly IRepository<SysInterfaceLog> _app;

        public InterfaceLogFilter(IRepository<SysInterfaceLog> app, IAuth authUtil)
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
            else
            {
                _app._loginInfo = new LoginInfo
                {
                    Id = 0,
                    Account = "Guest",
                    Name = "匿名",
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
            string Browser = headersDictionary[HeaderNames.UserAgent].ToString();
            string Server = context.HttpContext.Request.Host.ToString();
            string Path = context.HttpContext.Request.Path.ToString();
            string QueryString = context.HttpContext.Request.QueryString.ToString();
            var description = (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor;
            string ApiGroup = description.ControllerName;
            string ActionName = description.ActionName;
            string Method = context.HttpContext.Request.Method;
            string Name = _app._loginInfo.Name;
            string Ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
            string Request = ActionArguments.Replace("\r", "").Replace("\n", "");
            dynamic dresult = (context.Result == null ? null : (context.Result.GetType().Name == "EmptyResult" ? new { Value = "无返回结果" } : context.Result as dynamic));

            string Response = "在返回结果前发生了异常";
            try
            {
                if (dresult != null)
                {
                    Response = FormatResponse(Newtonsoft.Json.JsonConvert.SerializeObject(dresult.Value));
                }
            }
            catch (Exception)
            {
                Response = "";
            }

            bool bResult = GetResponseResult(Response);
            string Result = "成功";
            int Flag = 1;
            if (!bResult)
            {
                Result = "失败";
                Flag = GetReSendFlag(ActionName);
            }

            _app.Add(new SysInterfaceLog()
            {
                Type = "接收",
                System = "WMS",
                Method = Method,
                Server = Server,
                Path = Path,
                ActionName = ActionName,
                QueryString = QueryString,
                ApiGroup = ApiGroup,
                Request = Request,
                Response = Response,
                TotalMilliseconds = Stopwatch.Elapsed.TotalMilliseconds,
                LogTime = DateTime.Now,
                Name = Name,
                Ip = Ip,
                Browser = Browser,
                Result = Result,
                Flag = Flag,
            }); ;
        }

        private bool GetResponseResult(string response)
        {
            try
            {
                Response Res = JsonHelper.Instance.Deserialize<Response>(response);
                if (Res.Code != 200)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private int GetReSendFlag(string actionName)
        {
            List<string> actionNames = new List<string>();
            actionNames.Add("SetStackerStatus".ToLower());
            if (actionNames.Contains(actionName.ToLower()))
            {
                return 0;
            }
            return 1;
        }

        private string FormatResponse(string response)
        {
            response = response.Replace("\r", "").Replace("\n", "");
            response = response.Replace("\"\\\"{", "{").Replace("}\\\"\"", "}").Replace("\\\\\\", "");
            response = response.Replace("\"{", "{").Replace("}\"", "}").Replace("\\\"", "\"");

            return response;
        }
    }
}
