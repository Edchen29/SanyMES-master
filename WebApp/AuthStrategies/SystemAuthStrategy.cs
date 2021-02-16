// ***********************************************************************
// <summary>
// 超级管理员授权策略
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using WebRepository;

namespace WebApp
{
    /// <summary>
    /// 领域服务
    /// <para>超级管理员权限</para>
    /// <para>超级管理员使用guid.empty为ID，可以根据需要修改</para>
    /// </summary>
    public class SystemAuthStrategy : WebApp<SysUser>, IAuthStrategy
    {
        protected SysUser _user;

        public List<SysModuleView> Modules
        {
            get {
                var modules = (from module in UnitWork.Find<SysModule>(null)
                    select new SysModuleView
                    {
                        Name = module.Name,
                        Id = module.Id.Value,
                        CascadeId = module.CascadeId,
                        Code = module.Code,
                        IconName = module.IconName,
                        Url = module.Url,
                        ParentId = module.ParentId,
                        ParentName = module.ParentName,
                        SortNo = module.SortNo.Value,
                        IsShow = module.IsShow.Value,
                        CreateBy = module.CreateBy,
                        CreateTime = module.CreateTime,
                        UpdateBy = module.UpdateBy,
                        UpdateTime = module.UpdateTime
                    }).OrderBy(u => u.SortNo).ToList();

                foreach (var module in modules)
                {
                    module.Elements = UnitWork.Find<SysModuleElement>(u => u.ModuleId == module.Id).OrderBy(u => u.Sort).ToList();
                }

                return modules;
            }
        }

        public List<SysRole> Roles
        {
            get { return UnitWork.Find<SysRole>(null).ToList(); }
        }

        public List<SysDept> Orgs
        {
            get { return UnitWork.Find<SysDept>(null).ToList(); }
        }

        public SysUser User
        {
            get { return _user; }
            set   //禁止外部设置
            {
                throw new Exception("超级管理员，禁止设置用户");
            }  
        }

        public SystemAuthStrategy(IUnitWork unitWork, IRepository<SysUser> repository) : base(unitWork, repository)
        {
            _user = unitWork.FindSingle<SysUser>(u => u.Account.Equals("System"));
        }
    }
}