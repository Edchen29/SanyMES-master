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
	/// 制造BOM
	/// </summary>
    
    public partial class MbomHeaderApp
    {
        private IUnitWork _unitWork;
        public IRepository<MbomHeader> _app;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public MbomHeaderApp(IUnitWork unitWork, IRepository<MbomHeader> repository, IHostingEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _app = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public MbomHeaderApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, MbomHeader entity)
        {
            return _app.Load(pageRequest, entity);
        }

        public void Ins(MbomHeader entity)
        {
            entity.ProductId = _unitWork.FindSingle<ProductHeader>(u => u.Code.Equals(entity.ProductCode)).Id;
            _app.Add(entity);
        }

        public void Upd(MbomHeader entity)
        {
            entity.ProductId = _unitWork.FindSingle<ProductHeader>(u => u.Code.Equals(entity.ProductCode)).Id;
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public MbomHeader FindSingle(Expression<Func<MbomHeader, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<MbomHeader> Find(Expression<Func<MbomHeader, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<MbomHeader> exp = imp.ConvertToModel<MbomHeader>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    MbomHeader e = exp[i];
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

        public TableData ExportData(MbomHeader entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(MbomHeader entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<MbomHeader>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<MbomHeader> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
    }
}

