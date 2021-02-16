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
	/// 设备列表
	/// </summary>
    
    public partial class EquipmentApp
    {
        private IUnitWork _unitWork;
        public IRepository<Equipment> _app;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public EquipmentApp(IUnitWork unitWork, IRepository<Equipment> repository, IHostingEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _app = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public EquipmentApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, Equipment entity)
        {
            return _app.Load(pageRequest, entity);
        }

        public void Ins(Equipment entity)
        {
            if (!string.IsNullOrEmpty(entity.LineCode))
            {
                entity.LineId = _unitWork.FindSingle<Line>(u => u.LineCode.Equals(entity.LineCode)).Id;
            }
            if (!string.IsNullOrEmpty(entity.StationCode))
            {
                entity.StationId = _unitWork.FindSingle<Station>(u => u.Code.Equals(entity.StationCode)).Id;
            }
            _app.Add(entity);
        }

        public void Upd(Equipment entity)
        {
            if (!string.IsNullOrEmpty(entity.LineCode))
            {
                entity.LineId = _unitWork.FindSingle<Line>(u => u.LineCode.Equals(entity.LineCode)).Id;
            }
            if (!string.IsNullOrEmpty(entity.StationCode))
            {
                entity.StationId = _unitWork.FindSingle<Station>(u => u.Code.Equals(entity.StationCode)).Id;
            }
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public Equipment FindSingle(Expression<Func<Equipment, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<Equipment> Find(Expression<Func<Equipment, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<Equipment> exp = imp.ConvertToModel<Equipment>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    Equipment e = exp[i];
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

        public TableData ExportData(Equipment entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(Equipment entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<Equipment>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<Equipment> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
    }
}

