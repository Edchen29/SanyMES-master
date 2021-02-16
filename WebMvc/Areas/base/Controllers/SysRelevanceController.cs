using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using WebApp;

namespace WebMvc
{
    [Area("base")]
    public class SysRelevanceController : BaseController
    {
        private readonly SysRevelanceApp _app;

        public SysRelevanceController(IAuth authUtil, SysRevelanceApp app) : base(authUtil)
        {
            _app = app.SetLoginInfo(_loginInfo);
        }

        [HttpPost]
        public string Assign(string type, int firstId, int[] secIds)
        {
            try
            {
                _app.Assign(type, firstId, secIds);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }
        [HttpPost]
        public string UnAssign(string type, int firstId, int[] secIds)
        {
            try
            {
                _app.UnAssign(type, firstId, secIds);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }


    }
}