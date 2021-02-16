using System;
using System.Linq;
using WebRepository;

namespace WebApp
{
    public class ApiApp
    {
        protected IUnitWork _unitWork;
        protected IAuth _authUtil;
        protected BaseDBContext _context;
        protected LoginInfo _loginInfo;

        public ApiApp(IUnitWork unitWork, IAuth auth, BaseDBContext context)
        {
            _unitWork = unitWork;
            _authUtil = auth;
            _context = context;

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
            else
            {
                _loginInfo = new LoginInfo
                {
                    Id = 0,
                    Account = "Guest",
                    Name = "匿名",
                };
            }
        }

        public bool CheckLogin()
        {
            try
            {
                if (_authUtil.CheckLogin())
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }
}
