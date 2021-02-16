using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebApp;
using WebRepository;

namespace WebMvc
{
    [Area("base")]
    public class SysRoleController : BaseController
    {
        private readonly SysRoleApp _app;
        private readonly SysRevelanceApp _sysRevelanceApp;
        public SysRoleController(IAuth authUtil, SysRevelanceApp sysRevelanceApp, SysRoleApp app) : base(authUtil)
        {
            _sysRevelanceApp = sysRevelanceApp.SetLoginInfo(_loginInfo);
            _app = app.SetLoginInfo(_loginInfo);
        }
        //
        // GET: /SysUser/
        [Authenticate]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Assign()
        {
            return View();
        }

        //添加或修改组织
        [HttpPost]
        public string Add(SysRoleView obj)
        {
            try
            {
                _app.Add(obj);

            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        //添加或修改组织
        [HttpPost]
        public string Update(SysRoleView obj)
        {
            try
            {
                _app.Update(obj);

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

        /// <summary>
        /// 加载用户的角色
        /// </summary>
        public string LoadForUser(int userId)
        {
            try
            {
                var result = new Response<List<int>>
                {
                    Result = _sysRevelanceApp.Get(Define.USERROLE, true, userId)
                };
                return JsonHelper.Instance.Serialize(result);
            }
            catch (Exception e)
            {
                Result.Code = 500;
                Result.Message = e.InnerException?.Message ?? e.Message;
            }

            return JsonHelper.Instance.Serialize(Result);
        }

        /// <summary>
        /// 加载组织下面的所有用户
        /// </summary>
        [HttpPost]
        public string Load(PageReq pageRequest, int? orgId)
        {
            return JsonHelper.Instance.Serialize(_app.Load(pageRequest, orgId));
        }
    }
}