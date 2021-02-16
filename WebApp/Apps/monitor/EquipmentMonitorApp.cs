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
    /// 设备状态表
    /// </summary>

    public partial class EquipmentMonitorApp
    {
        private IUnitWork _unitWork;
        public IRepository<EquipmentMonitor> _app;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public EquipmentMonitorApp(IUnitWork unitWork, IRepository<EquipmentMonitor> repository, IHostingEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _app = repository;
            _hostingEnvironment = hostingEnvironment;
        }

        public EquipmentMonitorApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            return this;
        }

        public TableData Load(PageReq pageRequest, EquipmentMonitor entity)
        {
            return _app.Load(pageRequest, entity);
        }

        public void Ins(EquipmentMonitor entity)
        {
            _app.Add(entity);
        }

        public void Upd(EquipmentMonitor entity)
        {
            _app.Update(entity);
        }

        public EquipmentMonitor FindSingle(Expression<Func<EquipmentMonitor, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<EquipmentMonitor> Find(Expression<Func<EquipmentMonitor, bool>> exp)
        {
            return _app.Find(exp);
        }


        public TableData ExportData(EquipmentMonitor entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(EquipmentMonitor entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<EquipmentMonitor>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<EquipmentMonitor> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
    }
}