// ***********************************************************************
// <summary>
// 授权策略接口
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using WebRepository;

namespace WebApp
{
    public interface IAuthStrategy
    {
        List<SysModuleView> Modules { get; }

        List<SysRole> Roles { get; }

        List<SysDept> Orgs { get; }

        SysUser User
        {
            get; set;
        }
    }
}