using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using WebRepository;

namespace WebApp
{
    /// <summary>
	/// 工序表
	/// </summary>
    
    public partial class StepApp
    {
        private IUnitWork _unitWork;
        public IRepository<Step> _app;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public StepApp(IUnitWork unitWork, IRepository<Step> repository, IHostingEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _app = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public StepApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, Step entity)
        {
            return _app.Load(pageRequest, entity);
        }

        public void Ins(Step entity)
        {
            if (string.IsNullOrEmpty(entity.ProductId.ToString()))
            {
                entity.ProductId = _unitWork.FindSingle<ProductHeader>(u => u.Code.Equals(entity.ProductCode)).Id;
            }
            if (string.IsNullOrEmpty(entity.ProductCode))
            {
                entity.ProductCode = _unitWork.FindSingle<ProductHeader>(u => u.Id.Equals(entity.ProductId)).Code;
            }
            entity.MaterialId = _unitWork.FindSingle<Material>(u => u.Code.Equals(entity.MaterialCode)).Id;
            _app.Add(entity);
        }

        public void Upd(Step entity)
        {
            if (string.IsNullOrEmpty(entity.ProductId.ToString()))
            {
                entity.ProductId = _unitWork.FindSingle<ProductHeader>(u => u.Code.Equals(entity.ProductCode)).Id;
            }
            if (string.IsNullOrEmpty(entity.ProductCode))
            {
                entity.ProductCode = _unitWork.FindSingle<ProductHeader>(u => u.Id.Equals(entity.ProductId)).Code;
            }
            entity.MaterialId = _unitWork.FindSingle<Material>(u => u.Code.Equals(entity.MaterialCode)).Id;
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public Step FindSingle(Expression<Func<Step, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<Step> Find(Expression<Func<Step, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<Step> exp = imp.ConvertToModel<Step>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    Step e = exp[i];
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

        public TableData ExportData(Step entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(Step entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<Step>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<Step> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
        public Response UploadIForm(IFormFile file,string pcode,string scode)
        {
            string outputDirPath = $"/OfficeFiles/";
            Response result = new Infrastructure.Response();
                var fileName = file.FileName;
                String suffixName = fileName.Substring(fileName.LastIndexOf("."));
                Console.WriteLine(fileName);
                var newfilename = pcode + scode + suffixName;
                fileName = $"/UploadFile/{newfilename}";

                fileName = _hostingEnvironment.WebRootPath + fileName;

                using (FileStream fs = System.IO.File.Create(fileName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            Step step = _unitWork.Find<Step>(u => u.ProductCode == pcode && u.Code == scode).FirstOrDefault();
            if (suffixName.ToUpper()==".XLSX" || suffixName.ToUpper() == ".XLS")
            {
                ExcelPreview.Priview(fileName, pcode + scode, _hostingEnvironment.WebRootPath + outputDirPath);
                step.LinkSop = "/OfficeFiles/" + pcode + scode + ".html";
            }
            else if (suffixName.ToUpper() == ".DOC" || suffixName.ToUpper() == ".DOCX")
            {
                WordPreview.Priview(fileName, pcode + scode, _hostingEnvironment.WebRootPath + outputDirPath);
                step.LinkSop = "/OfficeFiles/" + pcode + scode + ".html";
            }
            else
            {
                step.LinkSop = "/UploadFile/" + newfilename;
            };
            _app.Update(step);
            return result;
        }
    }
}

