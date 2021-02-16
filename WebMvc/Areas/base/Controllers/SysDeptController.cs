using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using WebApp;
using WebRepository;

namespace WebMvc
{
    [Area("base")]
    public class SysDeptController : BaseController
    {
        private SysDeptApp _app;
        public SysDeptController(IAuth authUtil, SysDeptApp app) : base(authUtil)
        {
            _app = app.SetLoginInfo(_loginInfo);
        }

        //
        // GET: /SysDept/
        [Authenticate]
        public ActionResult Index()
        {
            return View();
        }

        //添加组织提交
        [HttpPost]
        public string Add(SysDept org)
        {
            try
            {
                _app.Add(org);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        //编辑
        [HttpPost]
        public string Update(SysDept org)
        {
            try
            {
                _app.Update(org);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        /// <summary>
        /// 删除指定ID的组织
        /// </summary>
        /// <returns>System.String.</returns>
        [HttpPost]
        public string Delete(int[] ids)
        {
            try
            {
                _app.Del(ids);
            }
            catch (Exception e)
            {
                Result.Code = 500;
                Result.Message = e.InnerException?.Message ?? e.Message;
            }

            return JsonHelper.Instance.Serialize(Result);
        }

        public string LoadForUser(int firstId)
        {
            var orgs = _app.LoadForUser(firstId);
            return JsonHelper.Instance.Serialize(orgs);
        }

        public string LoadForRole(int firstId)
        {
            var orgs = _app.LoadForRole(firstId);
            return JsonHelper.Instance.Serialize(orgs);
        }
    }
}