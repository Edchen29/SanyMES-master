using System;
using System.Collections.Generic;
using System.Linq;
using WebRepository;

namespace WebApp
{
    public class SysRoleApp : WebApp<SysRole>
    {
        private SysRevelanceApp _revelanceApp;

        private IAuth _auth;

        public SysRoleApp(IUnitWork unitWork, IRepository<SysRole> repository,
            SysRevelanceApp app, IAuth auth) : base(unitWork, repository)
        {
            _revelanceApp = app;
            _auth = auth;
        }

        public SysRoleApp SetLoginInfo(LoginInfo loginInfo)
        {
            Repository._loginInfo = loginInfo;
            return this;
        }

        /// <summary>
        /// 加载当前登录用户可访问的一个部门及子部门全部角色
        /// </summary>
        public TableData Load(PageReq pageRequest, int? orgId)
        {
            var loginUser = _auth.GetCurrentUser();

            string cascadeId = ".0.";
            if (orgId != null)
            {
                var org = loginUser.Orgs.SingleOrDefault(u => u.Id == orgId);
                cascadeId = org.CascadeId;
            }

            var ids = loginUser.Orgs.Where(u => u.CascadeId.Contains(cascadeId)).Select(u => u.Id.Value).ToArray();
            var roleIds = _revelanceApp.Get(Define.ROLEORG, false, ids);

            //SQL2008不支持分页的语法
            //var data = UnitWork.Find<Role>(u => roleIds.Contains(u.Id)).ToList();
            //var roles = data
            //    .OrderBy(u => u.Name)
            //    .Skip((pageRequest.page - 1) * pageRequest.limit)
            //    .Take(pageRequest.limit);

            var roles = UnitWork.Find<SysRole>(u => roleIds.Contains(u.Id.Value))
                .OrderBy(u => u.Name)
                .Skip((pageRequest.page - 1) * pageRequest.limit)
                .Take(pageRequest.limit);

            var records = Repository.GetCount(u => roleIds.Contains(u.Id.Value));

            var roleViews = new List<SysRoleView>();
            foreach (var role in roles.ToList())
            {
                SysRoleView uv = role;
                var orgs = LoadByRole(role.Id.Value);
                uv.Organizations = string.Join(",", orgs.Select(u => u.Name).ToList());
                uv.OrganizationIds = string.Join(",", orgs.Select(u => u.Id).ToList());
                roleViews.Add(uv);
            }

            return new TableData
            {
                count = records,
                data = roleViews,
            };
        }

        /// <summary>
        /// 加载角色的所有机构
        /// </summary>
        public IEnumerable<SysDept> LoadByRole(int roleId)
        {
            var result = from userorg in UnitWork.Find<SysRelevance>(null)
                         join org in UnitWork.Find<SysDept>(null) on userorg.SecondId equals org.Id
                         where userorg.FirstId == roleId && userorg.RelKey == Define.ROLEORG
                         select org;
            return result;
        }


        public void Add(SysRoleView obj)
        {
            if (string.IsNullOrEmpty(obj.OrganizationIds))
            {
                throw new Exception("请为角色分配机构");
            }

            if (UnitWork.IsExist<SysRole>(u => u.Name == obj.Name))
            {
                throw new Exception("角色已存在");
            }
            SysRole role = obj;
            role.CreateTime = DateTime.Now;
            Repository.Add(role);
            obj.Id = role.Id;   //要把保存后的ID存入view

            UpdateRole(obj);
        }

        public void Update(SysRoleView obj)
        {
            if (string.IsNullOrEmpty(obj.OrganizationIds))
                throw new Exception("请为角色分配机构");
            SysRole role = obj;

            Repository.Update(u => u.Id == obj.Id, u => new SysRole
            {
                Name = role.Name,
                Status = role.Status,
                UpdateBy = Repository._loginInfo.Account,
                UpdateTime = DateTime.Now
            });

            UpdateRole(obj);
        }

        /// <summary>
        /// 更新相应的多对多关系
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateRole(SysRoleView obj)
        {
            int[] orgIds = Array.ConvertAll(obj.OrganizationIds.Split(','), int.Parse);
            _revelanceApp.DeleteBy(Define.ROLEORG, obj.Id.Value);
            _revelanceApp.AddRelevance(Define.ROLEORG, orgIds.ToLookup(u => obj.Id.Value));
        }
    }
}