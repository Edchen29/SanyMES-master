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
	/// 产品表
	/// </summary>
    
    public partial class ProductHeaderApp
    {
        private IUnitWork _unitWork;
        public IRepository<ProductHeader> _app;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public ProductHeaderApp(IUnitWork unitWork, IRepository<ProductHeader> repository, IHostingEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _app = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public ProductHeaderApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, ProductHeader entity)
        {
            return _app.Load(pageRequest, entity);
        }
        public TableData DistinctLoad(PageReq pageRequest, ProductHeader entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<ProductHeader>.GetExpressions(entity));
            data = data.GroupBy(p => new { p.MachineType }).Select(g => g.First());

            GetData(data, result, pageRequest);
            result.count = data.Count();

            return result;
        }
        public void Ins(ProductHeader entity)
        {
            _app.Add(entity);
        }

        public void Upd(ProductHeader entity)
        {
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public ProductHeader FindSingle(Expression<Func<ProductHeader, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<ProductHeader> Find(Expression<Func<ProductHeader, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<ProductHeader> exp = imp.ConvertToModel<ProductHeader>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    ProductHeader e = exp[i];
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

        public TableData ExportData(ProductHeader entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(ProductHeader entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<ProductHeader>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<ProductHeader> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
    }
}

