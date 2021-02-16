// ***********************************************************************
// <summary>
// 普通用户授权策略
// </summary>
// ***********************************************************************

using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using WebRepository;

namespace WebApp
{
    /// <summary>
    /// 普通用户授权策略
    /// </summary>
    public class NormalAuthStrategy : WebApp<SysUser>, IAuthStrategy
    {

        protected SysUser _user;

        private List<int> _userRoleIds;    //用户角色

        public List<SysModuleView> Modules
        {
            get
            {
                var moduleIds = UnitWork.Find<SysRelevance>(
                    u =>
                        (u.FirstId == _user.Id && u.RelKey == Define.USERMODULE) ||
                        (u.RelKey == Define.ROLEMODULE && _userRoleIds.Contains(u.FirstId.Value))).Select(u => u.SecondId);

                var modules = (from module in UnitWork.Find<SysModule>(u => moduleIds.Contains(u.Id))
                               select new SysModuleView
                               {
                                   Name = module.Name,
                                   Code = module.Code,
                                   CascadeId = module.CascadeId,
                                   Id = module.Id.Value,
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

                var elementIds = UnitWork.Find<SysRelevance>(
                    u =>
                        (u.FirstId == _user.Id && u.RelKey == Define.USERELEMENT) ||
                        (u.RelKey == Define.ROLEELEMENT && _userRoleIds.Contains(u.FirstId.Value))).Select(u => u.SecondId);
                var usermoduleelements = UnitWork.Find<SysModuleElement>(u => elementIds.Contains(u.Id));

                foreach (var module in modules)
                {
                    module.Elements = usermoduleelements.Where(u => u.ModuleId == module.Id).OrderBy(u => u.Sort).ToList();
                }

                return modules;
            }
        }

        public List<SysRole> Roles
        {
            get { return UnitWork.Find<SysRole>(u => _userRoleIds.Contains(u.Id.Value)).ToList(); }
        }

        public List<SysDept> Orgs
        {
            get
            {
                var orgids = UnitWork.Find<SysRelevance>(
                    u =>
                        (u.FirstId == _user.Id && u.RelKey == Define.USERORG) ||
                        (u.RelKey == Define.ROLEORG && _userRoleIds.Contains(u.FirstId.Value))).Select(u => u.SecondId);
                return UnitWork.Find<SysDept>(u => orgids.Contains(u.Id)).ToList();
            }
        }

        public SysUser User
        {
            get { return _user; }
            set
            {
                _user = value;
                _userRoleIds = UnitWork.Find<SysRelevance>(u => u.FirstId == _user.Id && u.RelKey == Define.USERROLE).Select(u => u.SecondId.Value).ToList();
            }
        }

        public NormalAuthStrategy(IUnitWork unitWork, IRepository<SysUser> repository) : base(unitWork, repository)
        {
        }
    }
}