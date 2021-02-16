using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebApp;
using WebRepository;

namespace WebMvc
{
    /// <summary>
	/// 产品对应线体表
	/// </summary>
    [Area("configure")]
    public class ProductDetailController : BaseController
    {
        private readonly ProductDetailApp _app;
        
        public ProductDetailController(IAuth authUtil, ProductDetailApp app) : base(authUtil)
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
        public string Load(PageReq pageRequest, ProductDetail entity)
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
        public string Ins(ProductDetail Table_entity)
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
        public string Upd(ProductDetail Table_entity)
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
        public string Export(ProductDetail entity)
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
            List<ProductDetail> listProductDetail = new List<ProductDetail>();
            ProductDetail entity = _app.FindSingle(u => u.Id > 0);
            if (entity != null)
            {
                listProductDetail.Add(entity);
            }
            else
            {
                listProductDetail.Add(new ProductDetail());
            }
 
            result.data = listProductDetail;
            result.count = listProductDetail.Count;

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

        #endregion
    }
}