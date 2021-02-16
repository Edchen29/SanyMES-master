/*
 * 登录解析
 * 处理登录逻辑，验证客户段提交的账号密码，保存登录信息
 */
using Infrastructure;
using System;
using WebRepository;

namespace WebApp
{
    public class LoginParse
    {
        private ICacheContext _cacheContext;
        public IUnitWork _unitWork;
        public LoginParse(ICacheContext cacheContext, IUnitWork unitWork)
        {
            _cacheContext = cacheContext;
            _unitWork = unitWork;
        }

        public LoginResult Do(PassportLoginRequest model)
        {
            var result = new LoginResult();
            try
            {
                model.Trim();
                //获取应用信息
                var appInfo = _unitWork.FindSingle<SysInfo>(u => u.AppKey == model.AppKey);
                if (appInfo == null)
                {
                    throw new Exception("应用不存在");
                }
                else
                {
                    if (Encryption.Decrypt(appInfo.AppSecret) != "hhweb2.0")
                    {
                        throw new Exception("应用密钥不正确！");
                    }
                }

                //获取用户信息
                var userInfo = _unitWork.FindSingle<SysUser>(u => u.Account == model.Account);

                if (userInfo == null)
                {
                    throw new Exception("用户不存在");
                }
                if (Encryption.Decrypt(userInfo.Password) != model.Password)
                {
                    throw new Exception("密码错误");
                }

                var currentSession = new UserAuthSession
                {
                    Account = model.Account,
                    Name = userInfo.Name,
                    Token = Guid.NewGuid().ToString().GetHashCode().ToString("x"),
                    AppKey = model.AppKey,
                    CreateTime = DateTime.Now,
                };

                //创建Session
                _cacheContext.Set(currentSession.Token, currentSession, DateTime.Now.AddDays(10));
                result.Code = 200;
                result.ReturnUrl = appInfo.ReturnUrl;
                result.Token = currentSession.Token;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}