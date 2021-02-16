// ***********************************************************************
// <summary>
// 授权策略上下文，一个典型的策略模式
// 根据用户账号的不同，采用不同的授权模式，以后可以扩展更多的授权方式
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using WebRepository;

namespace WebApp
{
    /// <summary>
    ///  授权策略上下文，一个典型的策略模式
    /// </summary>
    public class AuthStrategyContext
    {
        private readonly IAuthStrategy _strategy;

        public SysUser User
        {
            get { return _strategy.User; }
        }

        public List<SysModuleView> Modules
        {
            get { return _strategy.Modules; }
        }

        public List<SysRole> Roles
        {
            get { return _strategy.Roles; }
        }

        public List<SysDept> Orgs
        {
            get { return _strategy.Orgs; }
        }

        public AuthStrategyContext(IAuthStrategy strategy)
        {
            this._strategy = strategy;
        }
    }
}
