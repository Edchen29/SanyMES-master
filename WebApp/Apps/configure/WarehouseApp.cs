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
	/// 线边库表
	/// </summary>
    
    public partial class WarehouseApp
    {
        private IUnitWork _unitWork;
        public IRepository<Warehouse> _app;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public WarehouseApp(IUnitWork unitWork, IRepository<Warehouse> repository, IHostingEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _app = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public WarehouseApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, Warehouse entity)
        {
            return _app.Load(pageRequest, entity);
        }

        public void Ins(Warehouse entity)
        {
            entity.LineId = _unitWork.FindSingle<Line>(u => u.LineCode.Equals(entity.LineCode)).Id;
            entity.WorkshopId = _unitWork.FindSingle<Workshop>(u => u.Code.Equals(entity.WorkshopCode)).Id;
            _app.Add(entity);
        }

        public void Upd(Warehouse entity)
        {
            entity.LineId = _unitWork.FindSingle<Line>(u => u.LineCode.Equals(entity.LineCode)).Id;
            entity.WorkshopId = _unitWork.FindSingle<Workshop>(u => u.Code.Equals(entity.WorkshopCode)).Id;
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public Warehouse FindSingle(Expression<Func<Warehouse, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<Warehouse> Find(Expression<Func<Warehouse, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<Warehouse> exp = imp.ConvertToModel<Warehouse>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    Warehouse e = exp[i];
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

        public TableData ExportData(Warehouse entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(Warehouse entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<Warehouse>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<Warehouse> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
    }
}

