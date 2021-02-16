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
	/// 设备报警警示文档表
	/// </summary>
    
    public partial class EquipmentAlarmTextApp
    {
        private IUnitWork _unitWork;
        public IRepository<EquipmentAlarmText> _app;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public EquipmentAlarmTextApp(IUnitWork unitWork, IRepository<EquipmentAlarmText> repository, IHostingEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _app = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public EquipmentAlarmTextApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, EquipmentAlarmText entity)
        {
            return _app.Load(pageRequest, entity);
        }

        public void Ins(EquipmentAlarmText entity)
        {
            _app.Add(entity);
        }

        public void Upd(EquipmentAlarmText entity)
        {
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public EquipmentAlarmText FindSingle(Expression<Func<EquipmentAlarmText, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<EquipmentAlarmText> Find(Expression<Func<EquipmentAlarmText, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<EquipmentAlarmText> exp = imp.ConvertToModel<EquipmentAlarmText>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    EquipmentAlarmText e = exp[i];
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

        public TableData ExportData(EquipmentAlarmText entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(EquipmentAlarmText entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<EquipmentAlarmText>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<EquipmentAlarmText> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
    }
}

