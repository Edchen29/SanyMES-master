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
	/// AGV监控
	/// </summary>
    
    public partial class AgvMonitorApp
    {
        private IUnitWork _unitWork;
        public IRepository<AgvMonitor> _app;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public AgvMonitorApp(IUnitWork unitWork, IRepository<AgvMonitor> repository, IHostingEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _app = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public AgvMonitorApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, AgvMonitor entity)
        {
            return _app.Load(pageRequest, entity);
        }

        public void Ins(AgvMonitor entity)
        {
            _app.Add(entity);
        }

        public void Upd(AgvMonitor entity)
        {
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public AgvMonitor FindSingle(Expression<Func<AgvMonitor, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<AgvMonitor> Find(Expression<Func<AgvMonitor, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<AgvMonitor> exp = imp.ConvertToModel<AgvMonitor>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    AgvMonitor e = exp[i];
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

        public TableData ExportData(AgvMonitor entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(AgvMonitor entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<AgvMonitor>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<AgvMonitor> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
    }
}

