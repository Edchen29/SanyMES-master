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
	/// 设备保养记录表
	/// </summary>
    
    public partial class EquipmentMaintainLogApp
    {
        private IUnitWork _unitWork;
        public IRepository<EquipmentMaintainLog> _app;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public EquipmentMaintainLogApp(IUnitWork unitWork, IRepository<EquipmentMaintainLog> repository, IHostingEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _app = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public EquipmentMaintainLogApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, EquipmentMaintainLog entity)
        {
            return _app.Load(pageRequest, entity);
        }

        public void Ins(EquipmentMaintainLog entity)
        {
            _app.Add(entity);
        }

        public void Upd(EquipmentMaintainLog entity)
        {
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public EquipmentMaintainLog FindSingle(Expression<Func<EquipmentMaintainLog, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<EquipmentMaintainLog> Find(Expression<Func<EquipmentMaintainLog, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<EquipmentMaintainLog> exp = imp.ConvertToModel<EquipmentMaintainLog>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    EquipmentMaintainLog e = exp[i];
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

        public TableData ExportData(EquipmentMaintainLog entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(EquipmentMaintainLog entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<EquipmentMaintainLog>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<EquipmentMaintainLog> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
    }
}

