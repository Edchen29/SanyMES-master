using System;
using System.Collections.Generic;
using System.Linq;
using WebRepository;

namespace WebApp
{
    public class SysModuleApp : WebApp<SysModule>
    {
        private SysRevelanceApp _revelanceApp;

        public SysModuleApp(IUnitWork unitWork, IRepository<SysModule> repository
        , SysRevelanceApp app) : base(unitWork, repository)
        {
            _revelanceApp = app;
        }

        public SysModuleApp SetLoginInfo(LoginInfo loginInfo)
        {
            Repository._loginInfo = loginInfo;
            return this;
        }

        public void Add(SysModule model)
        {
            ChangeModuleCascade(model);
            Repository.Add(model);
        }

        public void Update(SysModule model)
        {
            ChangeModuleCascade(model);
            Repository.Update(model);
        }

        #region 用户/角色分配模块

        /// <summary>
        /// 加载特定用户的模块
        /// TODO:这里会加载用户及用户角色的所有模块，“为用户分配模块”功能会给人一种混乱的感觉，但可以接受
        /// </summary>
        /// <param name="userId">The user unique identifier.</param>
        public IEnumerable<SysModule> LoadForUser(int userId)
        {
            var roleIds = _revelanceApp.Get(Define.USERROLE, true, userId);
            var moduleIds = UnitWork.Find<SysRelevance>(
                u =>
                    (u.FirstId == userId && u.RelKey == Define.USERMODULE) ||
                    (u.RelKey == Define.ROLEMODULE && roleIds.Contains(u.FirstId.Value))).Select(u => u.SecondId);
            return UnitWork.Find<SysModule>(u => moduleIds.Contains(u.Id)).OrderBy(u => u.SortNo);
        }

        /// <summary>
        /// 根据某用户ID获取可访问某模块的菜单项
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<SysModuleElement> LoadMenusForUser(int moduleId, int userId)
        {
            var elementIds = _revelanceApp.Get(Define.USERELEMENT, true, userId);
            return UnitWork.Find<SysModuleElement>(u => elementIds.Contains(u.Id.Value) && u.ModuleId == moduleId);
        }

        /// <summary>
        /// 加载特定角色的模块
        /// </summary>
        /// <param name="roleId">The role unique identifier.</param>
        public IEnumerable<SysModule> LoadForRole(int roleId)
        {
            var moduleIds = UnitWork.Find<SysRelevance>(u => u.FirstId == roleId && u.RelKey == Define.ROLEMODULE)
                .Select(u => u.SecondId);
            return UnitWork.Find<SysModule>(u => moduleIds.Contains(u.Id)).OrderBy(u => u.SortNo);
        }

        public IEnumerable<SysModuleElement> LoadMenusForRole(int moduleId, int roleId)
        {
            var elementIds = _revelanceApp.Get(Define.ROLEELEMENT, true, roleId);
            return UnitWork.Find<SysModuleElement>(u => elementIds.Contains(u.Id.Value) && u.ModuleId == moduleId);

        }
        #endregion 用户/角色分配模块


        #region 菜单操作
        /// <summary>
        /// 删除指定的菜单
        /// </summary>
        /// <param name="ids"></param>
        public void DelMenu(int[] ids)
        {
            UnitWork.Delete<SysModuleElement>(u => ids.Contains(u.Id.Value));
            UnitWork.Save();
        }

        public void AddMenu(SysModuleElement model)
        {
            model.CreateBy = Repository._loginInfo.Account;
            model.CreateTime = DateTime.Now;

            UnitWork.Add(model);
            UnitWork.Save();
        }
        #endregion

        public void UpdateMenu(SysModuleElement model)
        {
            model.UpdateBy = Repository._loginInfo.Account;
            model.UpdateTime = DateTime.Now;
            UnitWork.Update<SysModuleElement>(model);
            UnitWork.Save();
        }
    }
}