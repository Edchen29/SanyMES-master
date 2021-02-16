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
	/// 工位表
	/// </summary>
    
    public partial class StationApp
    {
        private IUnitWork _unitWork;
        public IRepository<Station> _app;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public StationApp(IUnitWork unitWork, IRepository<Station> repository, IHostingEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _app = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public StationApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, Station entity)
        {
            return _app.Load(pageRequest, entity);
        }

        public void Ins(Station entity)
        {
            entity.LineId = _unitWork.FindSingle<Line>(u => u.LineCode.Equals(entity.LineCode)).Id;
            _app.Add(entity);
        }

        public void Upd(Station entity)
        {
            entity.LineId = _unitWork.FindSingle<Line>(u => u.LineCode.Equals(entity.LineCode)).Id;
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public Station FindSingle(Expression<Func<Station, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<Station> Find(Expression<Func<Station, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<Station> exp = imp.ConvertToModel<Station>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    Station e = exp[i];
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

        public TableData ExportData(Station entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(Station entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<Station>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<Station> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
    }
}

