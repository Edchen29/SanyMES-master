using System;
using System.Collections.Generic;
using System.Linq;
using WebRepository;

namespace WebApp
{
    public class SysDeptApp : WebApp<SysDept>
    {
        public SysDeptApp(IUnitWork unitWork, IRepository<SysDept> repository) : base(unitWork, repository)
        {
        }

        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="org">The org.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.Exception">未能找到该组织的父节点信息</exception>
        public int Add(SysDept org)
        {
            ChangeModuleCascade(org);

            Repository.Add(org);

            return org.Id.Value;
        }

        public SysDeptApp SetLoginInfo(LoginInfo loginInfo)
        {
            Repository._loginInfo = loginInfo;
            return this;
        }

        public int Update(SysDept org)
        {
            ChangeModuleCascade(org);

            //获取旧的的CascadeId
            var cascadeId = Repository.FindSingle(o => o.Id == org.Id).CascadeId;
            //根据CascadeId查询子部门
            var orgs = Repository.Find(u => u.CascadeId.Contains(cascadeId) && u.Id != org.Id)
            .OrderBy(u => u.CascadeId).ToList();

            //更新操作
            Repository.Update(org);

            //更新子部门的CascadeId
            foreach (var a in orgs)
            {
                ChangeModuleCascade(a);
                Repository.Update(a);
            }

            Repository.Save();

            return org.Id.Value;
        }

        /// <summary>
        /// 删除指定ID的部门及其所有子部门
        /// </summary>
        public void Del(int[] ids)
        {
            var delOrg = Repository.Find(u => ids.Contains(u.Id.Value)).ToList();
            foreach (var org in delOrg)
            {
                Repository.Delete(u => u.CascadeId.Contains(org.CascadeId));
            }
        }

        /// <summary>
        /// 加载特定用户的角色
        /// TODO:这里会加载用户及用户角色的所有角色，“为用户分配角色”功能会给人一种混乱的感觉，但可以接受
        /// </summary>
        /// <param name="userId">The user unique identifier.</param>
        public List<SysDept> LoadForUser(int userId)
        {
            //用户角色
            var userRoleIds =
                UnitWork.Find<SysRelevance>(u => u.FirstId == userId && u.RelKey == Define.USERROLE).Select(u => u.SecondId).ToList();

            //用户角色与自己分配到的角色ID
            var moduleIds =
                UnitWork.Find<SysRelevance>(
                    u =>
                        (u.FirstId == userId && u.RelKey == Define.USERORG) ||
                        (u.RelKey == Define.ROLEORG && userRoleIds.Contains(u.FirstId))).Select(u => u.SecondId).ToList();

            if (!moduleIds.Any()) return new List<SysDept>();
            return UnitWork.Find<SysDept>(u => moduleIds.Contains(u.Id)).ToList();
        }

        /// <summary>
        /// 加载特定角色的角色
        /// </summary>
        /// <param name="roleId">The role unique identifier.</param>
        public List<SysDept> LoadForRole(int roleId)
        {
            var moduleIds =
                UnitWork.Find<SysRelevance>(u => u.FirstId == roleId && u.RelKey == Define.ROLEORG)
                    .Select(u => u.SecondId)
                    .ToList();
            if (!moduleIds.Any()) return new List<SysDept>();
            return UnitWork.Find<SysDept>(u => moduleIds.Contains(u.Id)).ToList();
        }
    }
}