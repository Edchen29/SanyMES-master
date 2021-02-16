using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using WebApp;
using WebRepository;

namespace WebMvc
{
    /// <summary>
	/// 物料配送任务表
	/// </summary>
    [Area("distribution")]
    public class MaterialDistributeTaskHeaderController : BaseController
    {
        private readonly MaterialDistributeTaskHeaderApp _app;
        
        public dynamic result;
        public MaterialDistributeTaskHeaderController(IAuth authUtil, MaterialDistributeTaskHeaderApp app) : base(authUtil)
        {
            _app = app.SetLoginInfo(_loginInfo);
        }

        #region 视图功能
        /// <summary>
        /// 默认视图Action
        /// </summary>
        /// <returns></returns>
        [Authenticate]
        [ServiceFilter(typeof(OperLogFilter))]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 加载及分页查询
        /// </summary>
        /// <param name="pageRequest">表单请求信息</param>
        /// <param name="entity">请求条件实例</param>
        /// <returns></returns>
        [HttpPost]
        public string Load(PageReq pageRequest, MaterialDistributeTaskHeader entity)
        {
            return JsonHelper.Instance.Serialize(_app.Load(pageRequest, entity));
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="Table_entity">新增实例</param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(OperLogFilter))]
        public string Ins(MaterialDistributeTaskHeader Table_entity)
        {
            try
            {
                _app.Ins(Table_entity);
            }
            catch (Exception ex)
            {
                Result.Status = false;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="Table_entity">修改实例</param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(OperLogFilter))]
        public string Upd(MaterialDistributeTaskHeader Table_entity)
        {
            try
            {
                _app.Upd(Table_entity);
            }
            catch (Exception ex)
            {
                Result.Status = false;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        [HttpPost]
        [ServiceFilter(typeof(OperLogFilter))]
        public string DelByIds(int[] ids)
        {
            try
            {
                _app.DelByIds(ids);
            }
            catch (Exception ex)
            {
                Result.Status = false;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }
        #endregion

        #region 导出数据
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="entity">请求条件实例</param>
        /// <returns></returns>
        [HttpPost]
        public string Export(MaterialDistributeTaskHeader entity)
        {
            return JsonHelper.Instance.Serialize(_app.ExportData(entity));
        }
        #endregion

        #region 导出模板
        /// <summary>
        /// 导出模板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetTemplate()
        {
            var result = new TableData();
            List<MaterialDistributeTaskHeader> listMaterialDistributeTaskHeader = new List<MaterialDistributeTaskHeader>();
            MaterialDistributeTaskHeader entity = _app.FindSingle(u => u.Id > 0);
            if (entity != null)
            {
                listMaterialDistributeTaskHeader.Add(entity);
            }
            else
            {
                listMaterialDistributeTaskHeader.Add(new MaterialDistributeTaskHeader());
            }
 
            result.data = listMaterialDistributeTaskHeader;
            result.count = listMaterialDistributeTaskHeader.Count;

            return JsonHelper.Instance.Serialize(result);
        }
        #endregion

        #region 导入数据
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="excelfile">表单提交的文件信息</param>
        /// <returns></returns>
        [HttpPost]
        public string Import(IFormFile excelfile)
        {
            try
            {
                Response result = _app.ImportIn(excelfile);
                if (!result.Status)
                {
                    Result.Status = false;
                    Result.Message = result.Message;
                }
            }
            catch (Exception ex)
            {
                Result.Status = false;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }
        #endregion

        #region 自定义方法
        [HttpPost]
        public string SendAGVTaskAPI(List<MaterialDistributeTaskHeader> senddata,string url)
        {
            return JsonHelper.Instance.Serialize(_app.SendAGVTaskApp(senddata, url));
        }

        [HttpPost]
        public string LoginTest(string url)
        {
            return JsonHelper.Instance.Serialize(_app.LoginTestApp(url));
        }
        //取消任务
        [HttpPost]
        [ServiceFilter(typeof(OperLogFilter))]
        public string TaskCancel(List<MaterialDistributeTaskHeader> mdthlist)
        {
            return JsonHelper.Instance.Serialize(_app.TaskCancelApp(mdthlist));
        }
        //备料确认
        [HttpPost]
        [ServiceFilter(typeof(OperLogFilter))]
        public string MaterialConfirm(int headerid,string containercode, string stockplace)
        {
            return JsonHelper.Instance.Serialize(_app.MaterialConfirmApp(headerid, containercode, stockplace));
        }
        //修改备料明细数据
        [HttpPost]
        [ServiceFilter(typeof(OperLogFilter))]
        public string UpdateDetail(MaterialDistributeTaskDetail entity)
        {
            return JsonHelper.Instance.Serialize(_app.UpdateDetailApp(entity));
        }
        #endregion
    }
}