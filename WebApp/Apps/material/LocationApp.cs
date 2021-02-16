using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebRepository;

namespace WebApp
{
    /// <summary>
	/// 库位表
	/// </summary>
    
    public partial class LocationApp
    {
        private IUnitWork _unitWork;
        public IRepository<Location> _app;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public LocationApp(IUnitWork unitWork, IRepository<Location> repository, IHostingEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _app = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public LocationApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, Location entity)
        {
            return _app.Load(pageRequest, entity);
        }
        public TableData FindStockPlace(PageReq pageRequest, Location entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<Location>.GetExpressions(entity));
            data = data.Where(u => u.Type == "S");

            GetData(data, result, pageRequest);
            result.count = data.Count();

            return result;
        }
        public void Ins(Location entity)
        {
            if (!string.IsNullOrEmpty(entity.LineCode))
            {
                entity.LineId = _unitWork.FindSingle<Line>(u => u.LineCode.Equals(entity.LineCode)).Id;
            }
            _app.Add(entity);
        }

        public void Upd(Location entity)
        {
            if (!string.IsNullOrEmpty(entity.LineCode))
            {
                entity.LineId = _unitWork.FindSingle<Line>(u => u.LineCode.Equals(entity.LineCode)).Id;
            }
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public Location FindSingle(Expression<Func<Location, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<Location> Find(Expression<Func<Location, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<Location> exp = imp.ConvertToModel<Location>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    Location e = exp[i];
                    e.Id = null;
                    _app.Add(e);
                }
                catch (Exception ex)
                {
                    sErrorMsg += "第" + (i + 2) + "行:" + ex.Message + "<br>";
                    result.Message = sErrorMsg;
                    break;
                }
            }
            if (sErrorMsg.Equals(string.Empty))
            {
                if (exp.Count == 0)
                {
                    sErrorMsg += "没有发现有效数据, 请确定模板是否正确， 或是否有填充数据！";
                    result.Message = sErrorMsg;
                }
                else
                {
                    result.Message = "导入完成";
                }
            }
            else
            {
                result.Status = false;
                result.Message = result.Message;
            }
            return result;
        }

        public TableData ExportData(Location entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(Location entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<Location>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<Location> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
        public TableData IsStopApp(int[] ids, bool stop)
        {
            TableData tab = new TableData();
            Location location = new Location();
            try
            {
                foreach (int id in ids)
                {
                    location = _unitWork.Find<Location>(n => n.Id == id).FirstOrDefault();
                    location.IsStop = stop;
                    _unitWork.Update(location);
                }
                tab.code = 200;
            }
            catch (Exception ex)
            {
                tab.code = 300;
                tab.msg = ex.Message;
            }
            return tab;
        }

        //批量创建
        public Response BtchAdd(string BLine,string Type, int Row, int Line, int Layer, int Grid, int Roadway, decimal Height, decimal Weight)
        {
            Response res = new Response();
            try
            {
                _unitWork.ExecuteSql(string.Format("EXECUTE dbo.P_Create_Location '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}'", BLine, Type, Row, Line, Layer, Grid, Roadway, "", Height, Weight));
                return res;
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                return res;
            }
        }
    }
}

