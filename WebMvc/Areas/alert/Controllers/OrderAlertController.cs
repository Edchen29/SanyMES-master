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
	/// 订单预警表
	/// </summary>
    [Area("alert")]
    public class OrderAlertController : BaseController
    {
        private readonly OrderAlertApp _app;
        
        public OrderAlertController(IAuth authUtil, OrderAlertApp app) : base(authUtil)
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
        public string Load(PageReq pageRequest, OrderAlert entity)
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
        public string Ins(OrderAlert Table_entity)
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
        public string Upd(OrderAlert Table_entity)
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
        public string Export(OrderAlert entity)
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
            List<OrderAlert> listOrderAlert = new List<OrderAlert>();
            OrderAlert entity = _app.FindSingle(u => u.Id > 0);
            if (entity != null)
            {
                listOrderAlert.Add(entity);
            }
            else
            {
                listOrderAlert.Add(new OrderAlert());
            }
 
            result.data = listOrderAlert;
            result.count = listOrderAlert.Count;

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
        /// <summary>
        /// 更新指定预警信息是否语音预警
        /// </summary>
        /// <param name="Id">Id</param>
        /// <param name="IsSpeak">是否语音预警</param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(OperLogFilter))]
        public string Update(int Id, bool IsSpeak)
        {
            try
            {
                _app.Update(Id, IsSpeak);
            }
            catch (Exception ex)
            {
                Result.Status = false;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }
        #endregion
    }
}