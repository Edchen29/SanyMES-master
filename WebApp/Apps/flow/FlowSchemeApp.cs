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
	/// 工作流模板信息表
	/// </summary>
    
    public partial class FlowSchemeApp
    {
        private IUnitWork __unitWork;
        public IRepository<FlowScheme> _app;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public FlowSchemeApp(IUnitWork _unitWork, IRepository<FlowScheme> repository, IHostingEnvironment hostingEnvironment)
        {
            __unitWork = _unitWork;
            _app = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public FlowSchemeApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, FlowScheme entity)
        {
            return _app.Load(pageRequest, entity);
        }

        public void Ins(FlowScheme entity)
        {
            if (entity.ProductId==null)
            {
                entity.ProductId = 0;
            }
            if (entity.DeleteMark == null)
            {
                entity.DeleteMark = 0;
            }
            if (entity.Disabled == null)
            {
                entity.Disabled = 0;
            }
            _app.Add(entity);
        }

        public void Upd(FlowScheme entity)
        {
            if (entity.ProductId == null)
            {
                entity.ProductId = 0;
            }
            if (entity.DeleteMark == null)
            {
                entity.DeleteMark = 0;
            }
            if (entity.Disabled == null)
            {
                entity.Disabled = 0;
            }
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public FlowScheme FindSingle(Expression<Func<FlowScheme, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<FlowScheme> Find(Expression<Func<FlowScheme, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<FlowScheme> exp = imp.ConvertToModel<FlowScheme>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    FlowScheme e = exp[i];
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

        public TableData ExportData(FlowScheme entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(FlowScheme entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<FlowScheme>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<FlowScheme> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }

        public FlowScheme Get(string id)
        {
            var result = new TableData();
            FlowScheme data = __unitWork.FindSingle<FlowScheme>(u => u.Id == int.Parse(id));
            result.data = data;
            return result.data;
        }
    }
}

