using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp
{
    /// <summary>
    /// 采用Attribute的方式验证登录
    /// </summary>
    public class SSOAuthAttribute : ActionFilterAttribute
    {
        public const string Token = "Token";

        private IAuth _auth;

        public SSOAuthAttribute(IAuth auth)
        {
            _auth = auth;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var token = "";

            //Token by QueryString
            var request = filterContext.HttpContext.Request;
            if (!string.IsNullOrEmpty(request.Query[Token]))
            {
                token = request.Query[Token];
                
                filterContext.HttpContext.Response.Cookies.Append(Token, token);
            }
            else if (request.Cookies[Token] != null)  //从Cookie读取Token
            {
                token = request.Cookies[Token];
            }

            if (string.IsNullOrEmpty(token))
            {
                //直接登录
                filterContext.Result = LoginResult("");
                return;
            }
            else
            {
                //验证
                if (_auth.CheckLogin(token) == false)
                {
                    //会话丢失，跳转到登录页面
                    filterContext.Result = LoginResult("");
                    return;
                }
            }

            //base.OnActionExecuting(filterContext);
        }

        public virtual ActionResult LoginResult(string username)
        {
            return new RedirectResult("/Login/Index");
        }
    }
}
