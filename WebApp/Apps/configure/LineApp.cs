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
	/// 线体表
	/// </summary>
    
    public partial class LineApp
    {
        private IUnitWork _unitWork;
        public IRepository<Line> _app;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public LineApp(IUnitWork unitWork, IRepository<Line> repository, IHostingEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _app = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public LineApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, Line entity)
        {
            return _app.Load(pageRequest, entity);
        }

        public void Ins(Line entity)
        {
            entity.WorkshopId = _unitWork.FindSingle<Workshop>(u => u.Code.Equals(entity.WorkshopCode)).Id;
            _app.Add(entity);
        }

        public void Upd(Line entity)
        {
            entity.WorkshopId = _unitWork.FindSingle<Workshop>(u => u.Code.Equals(entity.WorkshopCode)).Id;
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public Line FindSingle(Expression<Func<Line, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<Line> Find(Expression<Func<Line, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<Line> exp = imp.ConvertToModel<Line>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    Line e = exp[i];
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

        public TableData ExportData(Line entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(Line entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<Line>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<Line> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
    }
}

