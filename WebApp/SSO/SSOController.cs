// ***********************************************************************
// <summary>
// 基础控制器
// 继承该控制器可以防止未登录查看
// 继承该控制器后，如果想访问控制器中存在，但模块配置里面没有的Action（如：Home/Git），请使用AnonymousAttribute
// </summary>
// ***********************************************************************

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebRepository;

namespace WebApp
{
    public class SSOController : Controller
    {
        public const string Token = "Token";

        protected LoginInfo _loginInfo;
        protected IAuth _authUtil;

        public SSOController(IAuth authUtil)
        {
            _authUtil = authUtil;
            AuthStrategyContext authStrategyContext = _authUtil.GetCurrentUser();
            if (authStrategyContext != null)
            {
                _loginInfo = new LoginInfo
                {
                    Id = authStrategyContext.User.Id,
                    Account = authStrategyContext.User.Account,
                    Name = authStrategyContext.User.Name,
                };
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string userToken = "";
            //Token by QueryString
            var request = filterContext.HttpContext.Request;
            if (request.Cookies[Token] != null)  //从Cookie读取Token
            {
                userToken = request.Cookies[Token];
            }

            if (string.IsNullOrEmpty(userToken))
            {
                //直接登录
                filterContext.Result = LoginResult("");
                return;
            }
            //验证
            if (_authUtil.CheckLogin(userToken) == false)
            {
                //会话丢失，跳转到登录页面
                filterContext.Result = LoginResult("");
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        public virtual ActionResult LoginResult(string username)
        {
            return new RedirectResult("/Login/Index");
        }
    }
}