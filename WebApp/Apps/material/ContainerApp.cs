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
	/// 容器表
	/// </summary>
    
    public partial class ContainerApp
    {
        private IUnitWork _unitWork;
        public IRepository<Container> _app;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public ContainerApp(IUnitWork unitWork, IRepository<Container> repository, IHostingEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _app = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public ContainerApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, Container entity)
        {
            return _app.Load(pageRequest, entity);
        }

        public void Ins(Container entity)
        {
            if (entity.PrintCount==null)
            {
                entity.PrintCount = 0;
            }
            _app.Add(entity);
        }

        public void Upd(Container entity)
        {
            if (entity.PrintCount == null)
            {
                entity.PrintCount = 0;
            }
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public Container FindSingle(Expression<Func<Container, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<Container> Find(Expression<Func<Container, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<Container> exp = imp.ConvertToModel<Container>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    Container e = exp[i];
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

        public TableData ExportData(Container entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(Container entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<Container>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<Container> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
        public string UpDatas(List<Container> ids)
        {
            string strError = "";
            foreach (Container UpDate in ids)
            {
                string sql = string.Format("UPDATE dbo.container SET printCount = '{0}' WHERE id = '{1}';", UpDate.PrintCount, UpDate.Id);
                try
                {
                    _unitWork.ExecuteSql(sql);
                }
                catch (Exception ex)
                {
                    strError += "失败原因：" + ex.Message + "||";
                }
            }
            return strError;
        }
        //批量创建
        public Response BtchAdd(string Type, int Num)
        {
            Response res = new Response();
            try
            {
                _unitWork.ExecuteSql(string.Format("EXECUTE dbo.P_Create_Container '{0}','{1}'", Type, Num));
                return res;
            }
            catch (Exception ex)
            {
                res.Code = 300;
                res.Message = ex.Message;
                return res;
            }
        }
    }
}

