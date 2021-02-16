using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using WebApp;
using WebRepository;

namespace WebMvc.Areas.Api.Controllers
{
    /// <summary>
    /// 系统登录
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private string _appKey = "hhweb";

        private IAuth _authUtil;

        public LoginController(IAuth authUtil)
        {
            _authUtil = authUtil;
        }

        /// <summary>
        /// 登入接口
        /// </summary>
        /// <param name="loginEntity">用户登录信息</param>
        /// <returns></returns>
        [HttpPost("Login")]
        public LoginResponse Login(LoginEntity loginEntity)
        {
            var resp = new LoginResponse();

            try
            {
                if (loginEntity == null)
                    throw new Exception("请输入用户名及密码！");

                var result = _authUtil.Login(_appKey, loginEntity.username, loginEntity.password);
                if (result.Code == 200)
                {
                    Response.Cookies.Append("Token", result.Token);
                    resp.Token = result.Token;
                }
                else
                {
                    resp.Code = 500;
                    resp.Message = result.Message;
                }
            }
            catch (Exception e)
            {
                resp.Code = 500;
                resp.Message = e.Message;
            }

            //return JsonHelper.Instance.Serialize(resp);
            return resp;
        }

        /// <summary>
        /// 登出
        /// </summary>
        [HttpPost("Logout")]
        public void Logout()
        {
            _authUtil.Logout();
        }

        /// <summary>
        /// 心跳接口，用于延长cookie有效期
        /// </summary>
        [HttpPost("heartbeat")]
        public void heartbeat()
        {
            _authUtil.CheckLogin();
        }
    }
}