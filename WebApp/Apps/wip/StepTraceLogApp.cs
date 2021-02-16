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
	/// 工序跟踪日志主表
	/// </summary>
    
    public partial class StepTraceLogApp
    {
        private IUnitWork _unitWork;
        public IRepository<StepTraceLog> _app;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public StepTraceLogApp(IUnitWork unitWork, IRepository<StepTraceLog> repository, IHostingEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _app = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public StepTraceLogApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, StepTraceLog entity)
        {
            return _app.Load(pageRequest, entity);
        }

        public void Ins(StepTraceLog entity)
        {
            if (string.IsNullOrEmpty(entity.ProductId.ToString()))
            {
                entity.ProductId = _unitWork.FindSingle<ProductHeader>(u => u.Code.Equals(entity.ProductCode)).Id;
            }
            if (string.IsNullOrEmpty(entity.ProductCode))
            {
                entity.ProductCode = _unitWork.FindSingle<ProductHeader>(u => u.Id.Equals(entity.ProductId)).Code;
            }
            _app.Add(entity);
        }

        public void Upd(StepTraceLog entity)
        {
            if (string.IsNullOrEmpty(entity.ProductId.ToString()))
            {
                entity.ProductId = _unitWork.FindSingle<ProductHeader>(u => u.Code.Equals(entity.ProductCode)).Id;
            }
            if (string.IsNullOrEmpty(entity.ProductCode))
            {
                entity.ProductCode = _unitWork.FindSingle<ProductHeader>(u => u.Id.Equals(entity.ProductId)).Code;
            }
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public StepTraceLog FindSingle(Expression<Func<StepTraceLog, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<StepTraceLog> Find(Expression<Func<StepTraceLog, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<StepTraceLog> exp = imp.ConvertToModel<StepTraceLog>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    StepTraceLog e = exp[i];
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

        public TableData ExportData(StepTraceLog entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(StepTraceLog entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<StepTraceLog>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<StepTraceLog> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
    }
}

