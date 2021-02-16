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
	/// 工序跟踪表
	/// </summary>
    
    public partial class StepTraceApp
    {
        private IUnitWork _unitWork;
        public IRepository<StepTrace> _app;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public StepTraceApp(IUnitWork unitWork, IRepository<StepTrace> repository, IHostingEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _app = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public StepTraceApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, StepTrace entity)
        {
            return _app.Load(pageRequest, entity);
        }

        public void Ins(StepTrace entity)
        {
            _app.Add(entity);
        }

        public void Upd(StepTrace entity)
        {
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public StepTrace FindSingle(Expression<Func<StepTrace, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<StepTrace> Find(Expression<Func<StepTrace, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<StepTrace> exp = imp.ConvertToModel<StepTrace>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    StepTrace e = exp[i];
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

        public TableData ExportData(StepTrace entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(StepTrace entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<StepTrace>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<StepTrace> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
        public TableData TurnStepApp(StepTrace stepTrace, string tostep)
        {
            TableData tab = new TableData
            {
                code = 200
            };
            string sErrorMsg = "";
            int nofree = 0;
            try
            {

                if (stepTrace != null)
                {
                    var sidlist = _unitWork.Find<StepStation>(u => u.StepType == tostep).Select(a=>a.StationId);
                    List<Equipment> equipments = _unitWork.Find<Equipment>(u => sidlist.Contains(u.StationId)).ToList();

                    foreach (var equipment in equipments)
                    {
                        if (_unitWork.IsExist<EquipmentStatus>(u => u.EquipmentId.Equals(equipment.Id) && u.Status == "0"))
                        {
                            //调用桁车接口进行转移工序作业，如果接口调用成功，更新下一工位为指定的去向工位;
                            int? toStepId = _unitWork.Find<Step>(u => u.StepType == tostep).Select(a => a.Id).FirstOrDefault();
                            stepTrace.NextStepId = toStepId;
                            _app.Update(stepTrace);
                            tab.code = 200;
                            tab.msg = "工单号：" + stepTrace.WONumber + "操作成功！";
                            break;
                            //待桁车工序转移作业完成后调用中控桁车运行节点接口告知中控系统完成信号，中控系统更新StepTrace的当前工位，和下一工序工位，及当前工序，及呼叫焊接设备开始工作

                        }
                        else
                        {
                            nofree = nofree + 1;
                            if (nofree == equipments.Count)
                            {
                                sErrorMsg += "此工序没有空闲工位可用，无法转到此工序！<br>";
                                tab.msg = sErrorMsg;
                            }

                        }

                    }
                        
                }
                else
                {
                    sErrorMsg += "数据错误，传入空数据！<br>";
                    tab.msg = sErrorMsg;
                }

            }
            catch (Exception ex)
            {
                tab.code = 300;
                tab.msg += ex.Message;
            }
            return tab;
        }
        public TableData SetNGApp(StepTrace stepTrace, string otype,string ntype,string ncode)
        {
            TableData tab = new TableData
            {
                code = 200
            };
            string sErrorMsg = "";
            try
            {

               if (stepTrace != null)
               {
                    if (otype== "discard")
                    {
                        stepTrace.IsInvalid = true;
                        stepTrace.IsNG = true;
                        stepTrace.NGcode = ncode;
                        _app.Update(stepTrace);
                        //此处写报废逻辑走向，呼叫桁车转移工序，人工补焊工位标记为报废后应该是直接流向下料口退出线体，不能流向下一工位（机加工序）
                    }
                    else
                    {
                        stepTrace.IsNG = true;
                        stepTrace.NGcode = ncode;
                        _app.Update(stepTrace);
                        //此处写不良去向逻辑，呼叫桁车转移工序
                    }
                    tab.code = 200;
                    tab.msg = "工单号：" + stepTrace.WONumber + "操作成功！";
                }
               else
               {
                        sErrorMsg += "数据错误，传入空数据！<br>";
                        tab.msg = sErrorMsg;
               }

            }
            catch (Exception ex)
            {
                tab.code = 300;
                tab.msg += ex.Message;
            }
            return tab;
        }
    }
}

