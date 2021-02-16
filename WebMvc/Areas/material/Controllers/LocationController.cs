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
	/// 库位表
	/// </summary>
    [Area("material")]
    public class LocationController : BaseController
    {
        private readonly LocationApp _app;
        
        public LocationController(IAuth authUtil, LocationApp app) : base(authUtil)
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
        public string Load(PageReq pageRequest, Location entity)
        {
            return JsonHelper.Instance.Serialize(_app.Load(pageRequest, entity));
        }
        [HttpPost]
        public string FindStockPlace(PageReq pageRequest, Location entity)
        {
            return JsonHelper.Instance.Serialize(_app.FindStockPlace(pageRequest, entity));
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
        public string Ins(Location Table_entity)
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
        public string Upd(Location Table_entity)
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
        public string Export(Location entity)
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
            List<Location> listLocation = new List<Location>();
            Location entity = _app.FindSingle(u => u.Id > 0);
            if (entity != null)
            {
                listLocation.Add(entity);
            }
            else
            {
                listLocation.Add(new Location());
            }
 
            result.data = listLocation;
            result.count = listLocation.Count;

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
        /// 仓位启用停用
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        [HttpPost]
        public string IsStope(int[] ids, bool stop)
        {
            return JsonHelper.Instance.Serialize(_app.IsStopApp(ids, stop));
        }

        /// <summary>
        /// 批量增加仓位
        /// </summary>
        /// <param name="Type">库位类型</param>
        /// <param name="Row">总行数</param>
        /// <param name="Line">总列数</param>
        /// <param name="Layer">总层数</param>
        /// <param name="Grid">总格数</param>
        /// <param name="Roadway">巷道</param>
        /// <param name="Height">高度上限</param>
        /// <param name="Weight">总量上限</param>
        /// <returns></returns>
        [HttpPost]
        public Response BtchAdd(string BLine, string Type, int Row, int Line, int Layer, int Grid, int Roadway, decimal Height, decimal Weight)
        {
            return _app.BtchAdd(BLine,Type, Row, Line, Layer, Grid, Roadway, Height, Weight);
        }
        #endregion
    }
}