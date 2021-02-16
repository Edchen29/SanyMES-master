using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebApp;
using WebRepository;

namespace WebMvc
{
    [Area("base")]
    public class SysUserController : BaseController
    {
        private readonly SysUserApp _app;
        private readonly AuthStrategyContext _authStrategyContext;

        public SysUserController(IAuth authUtil, SysUserApp app) : base(authUtil)
        {
            _authStrategyContext = authUtil.GetCurrentUser();
            _app = app.SetLoginInfo(_loginInfo);
        }

        //
        // GET: /SysUser/
        [Authenticate]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 加载组织下面的所有用户
        /// </summary>
        [HttpPost]
        public string Load(PageReq request, int? orgId)
        {
            return JsonHelper.Instance.Serialize(_app.Load(request, orgId));
        }

        //添加用户
        [HttpPost]
        public string Add(SysUserView view)
        {
            try
            {
                _app.Add(view);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        //修改用户
        [HttpPost]
        public string Update(SysUserView view)
        {
            try
            {
                _app.Update(view);
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

        #region 获取权限数据

        /// <summary>
        /// 获取用户可访问的账号
        /// </summary>
        public string GetAccessedUsers()
        {
            IEnumerable<SysUserView> users = _app.Load(new PageReq(), null).data;
            var result = new Dictionary<int, object>();
            foreach (var user in users)
            {
                var item = new
                {
                    Account = user.Account,
                    RealName = user.Name,
                    id = user.Id,
                    text = user.Name,
                    value = user.Account,
                    parentId = "0",
                    showcheck = true,
                    img = "fa fa-user",
                };
                result.Add(user.Id.Value, item);
            }

            return JsonHelper.Instance.Serialize(result);
        }
        #endregion

        public ActionResult ChangePassword()
        {
            return View();
        }

        //修改个人密码
        [HttpPost]
        public string ChangeUserPassword(string OldPassword, string Password)
        {
            try
            {
                _app.ChangeUserPassword(OldPassword, Password, _authStrategyContext.User);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        //修改个人密码
        [HttpPost]
        public string ResetPassword(SysUser user)
        {
            try
            {
                _app.ResetPassword(user);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }
    }
}