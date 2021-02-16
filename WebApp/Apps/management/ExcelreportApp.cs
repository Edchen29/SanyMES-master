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
	/// EXCEL报表
	/// </summary>
    
    public partial class ExcelreportApp
    {
        private IUnitWork _unitWork;
        public IRepository<Excelreport> _app;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public ExcelreportApp(IUnitWork unitWork, IRepository<Excelreport> repository, IHostingEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _app = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public ExcelreportApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, Excelreport entity)
        {
            return _app.Load(pageRequest, entity);
        }

        public void Ins(Excelreport entity)
        {
            string sql = entity.Sql;
            
            if (sql.ToLower().IndexOf("delete") >= 0
                || sql.ToLower().IndexOf("update") >= 0
                || sql.ToLower().IndexOf("insert") >= 0
                || sql.ToLower().IndexOf(" table ") >= 0
                || sql.ToLower().IndexOf(" database ") >= 0
                || sql.ToLower().IndexOf(" procedure ") >= 0
                || sql.ToLower().IndexOf(" trigger ") >= 0
                || sql.ToLower().IndexOf(" view ") >= 0)
            {
                throw new Exception("SELECT '语句不合法, 报表只能输入SELECT语句！' 发生错误 ");
            }
            _app.Add(entity);
        }

        public void Upd(Excelreport entity)
        {
            string sql = entity.Sql;

            if (sql.ToLower().IndexOf("delete") >= 0
                || sql.ToLower().IndexOf("update") >= 0
                || sql.ToLower().IndexOf("insert") >= 0
                || sql.ToLower().IndexOf(" table ") >= 0
                || sql.ToLower().IndexOf(" database ") >= 0
                || sql.ToLower().IndexOf(" procedure ") >= 0
                || sql.ToLower().IndexOf(" trigger ") >= 0
                || sql.ToLower().IndexOf(" view ") >= 0)
            {
                throw new Exception("SELECT '语句不合法, 报表只能输入SELECT语句！' 发生错误 ");
            }

            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public Excelreport FindSingle(Expression<Func<Excelreport, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<Excelreport> Find(Expression<Func<Excelreport, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<Excelreport> exp = imp.ConvertToModel<Excelreport>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    Excelreport e = exp[i];
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

        public TableData ExportData(Excelreport entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(Excelreport entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<Excelreport>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<Excelreport> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
    }
}

