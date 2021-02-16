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
	/// 订单预警表
	/// </summary>
    
    public partial class OrderAlertApp
    {
        private IUnitWork _unitWork;
        public IRepository<OrderAlert> _app;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public OrderAlertApp(IUnitWork unitWork, IRepository<OrderAlert> repository, IHostingEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _app = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public OrderAlertApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, OrderAlert entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<OrderAlert>.GetExpressions(entity));
            data = data.Where(u => u.Flag != 1);

            GetData(data, result, pageRequest);
            result.count = data.Count();

            return result;
        }

        public void Ins(OrderAlert entity)
        {
            _app.Add(entity);
        }

        public void Upd(OrderAlert entity)
        {
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public OrderAlert FindSingle(Expression<Func<OrderAlert, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<OrderAlert> Find(Expression<Func<OrderAlert, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<OrderAlert> exp = imp.ConvertToModel<OrderAlert>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    OrderAlert e = exp[i];
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

        public TableData ExportData(OrderAlert entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(OrderAlert entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<OrderAlert>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<OrderAlert> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }

        public void Update(int Id, bool IsSpeak)
        {
            var data = _app.FindSingle(u => u.Id == Id);
            data.IsSpeak = IsSpeak;
            _app.Update(data);
        }
    }
}

