using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebApp;
using WebRepository;

namespace WebMvc
{
    [Area("base")]
    public class SysModuleController : BaseController
    {
        private SysModuleApp _app;
        public SysModuleController(IAuth authUtil, SysModuleApp app) : base(authUtil)
        {
            _app = app.SetLoginInfo(_loginInfo);
        }

        // GET: /SysModule/
        [Authenticate]
        public ActionResult Index()
        {
            return View();
        }

        [Authenticate]
        public ActionResult Assign()
        {
            return View();
        }

        /// <summary>
        /// 加载特定用户的模块
        /// </summary>
        /// <param name="firstId">The user identifier.</param>
        /// <returns>System.String.</returns>
        public string LoadForUser(int firstId)
        {
            var modules = _app.LoadForUser(firstId);
            return JsonHelper.Instance.Serialize(modules);
        }
        /// <summary>
        /// 根据某用户ID获取可访问某模块的菜单项
        /// </summary>
        /// <returns></returns>
        public string LoadMenusForUser(int moduleId, int firstId)
        {
            var menus = _app.LoadMenusForUser(moduleId, firstId);
            return JsonHelper.Instance.Serialize(menus);
        }

        /// <summary>
        /// 加载角色模块
        /// </summary>
        /// <param name="firstId">The role identifier.</param>
        /// <returns>System.String.</returns>
        public string LoadForRole(int firstId)
        {
            var modules = _app.LoadForRole(firstId);
            return JsonHelper.Instance.Serialize(modules);
        }

        /// <summary>
        /// 根据某角色ID获取可访问某模块的菜单项
        /// </summary>
        /// <returns></returns>
        public string LoadMenusForRole(int moduleId, int firstId)
        {
            var menus = _app.LoadMenusForRole(moduleId, firstId);
            return JsonHelper.Instance.Serialize(menus);
        }

        /// <summary>
        /// 获取发起页面的菜单权限
        /// </summary>
        /// <returns>System.String.</returns>
        public string LoadAuthorizedMenus(string modulecode, string AreaMenus)
        {
            var user = _authUtil.GetCurrentUser();
            var module = user.Modules.Find(u => u.Code == modulecode);
            if (module != null)
            {
                var moduleMenus = module.Elements.ToList().FindAll(u => u.AreaMenus == AreaMenus);
                return JsonHelper.Instance.Serialize(moduleMenus);
            }

            return "";
        }

        #region 添加编辑模块

        //添加模块
        [HttpPost]
        public string Add(SysModule model)
        {
            try
            {
                model.Url = model.Url ?? "";
                _app.Add(model);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        //修改模块
        [HttpPost]
        public string Update(SysModule model)
        {
            try
            {
                model.Url = model.Url ?? "";
                _app.Update(model);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        [HttpPost]
        public string Delete(int[] ids)
        {
            try
            {
                _app.Delete(ids);
            }
            catch (Exception e)
            {
                Result.Code = 500;
                Result.Message = e.InnerException?.Message ?? e.Message;
            }

            return JsonHelper.Instance.Serialize(Result);
        }

        #endregion 添加编辑模块
        /// <summary>
        /// 加载当前用户可访问模块的菜单
        /// </summary>
        /// <param name="moduleId">The module identifier.</param>
        /// <returns>System.String.</returns>
        public string LoadMenus(int moduleId)
        {
            var user = _authUtil.GetCurrentUser();
            var data = new TableData();

            if (moduleId != 0)
            {
                var module = user.Modules.SingleOrDefault(u => u.Id == moduleId);
                if (module != null)
                {
                    data.data = module.Elements;
                    data.count = module.Elements.Count();
                }
            }
            
            return JsonHelper.Instance.Serialize(data);
        }

        //添加菜单
        [HttpPost]
        public string AddMenu(SysModuleElement model)
        {
            try
            {
                model.CreateTime = DateTime.Now;
                _app.AddMenu(model);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        //添加菜单
        [HttpPost]
        public string UpdateMenu(SysModuleElement model)
        {
            try
            {
                _app.UpdateMenu(model);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }


        /// <summary>
        /// 删除菜单
        /// </summary>
        [HttpPost]
        public string DelMenu(params int[] ids)
        {
            try
            {
                _app.DelMenu(ids);
            }
            catch (Exception e)
            {
                Result.Code = 500;
                Result.Message = e.InnerException?.Message ?? e.Message;
            }

            return JsonHelper.Instance.Serialize(Result);
        }
    }
}