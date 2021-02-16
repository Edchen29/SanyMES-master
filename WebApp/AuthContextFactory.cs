// ***********************************************************************
// <summary>
// 用户权限策略工厂
//</summary>
// ***********************************************************************


using WebRepository;

namespace WebApp
{
    /// <summary>
    ///  加载用户所有可访问的资源/机构/模块
    /// </summary>
    public class AuthContextFactory
    {
        private SystemAuthStrategy _systemAuth;
        private NormalAuthStrategy _normalAuthStrategy;
        private readonly IRepository<SysUser> _app;

        public AuthContextFactory(SystemAuthStrategy sysStrategy
            , NormalAuthStrategy normalAuthStrategy
            , IRepository<SysUser> app)
        {
            _systemAuth = sysStrategy;
            _normalAuthStrategy = normalAuthStrategy;
            _app = app;
        }

        public AuthStrategyContext GetAuthStrategyContext(string username)
        {
            IAuthStrategy service = null;
            if (username == "System")
            {
                service = _systemAuth;
            }
            else
            {
                service = _normalAuthStrategy;
                service.User = _app.FindSingle(u => u.Account == username);
            }

         return new AuthStrategyContext(service);
        }
    }
}