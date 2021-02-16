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
	/// 物料呼叫表
	/// </summary>
    
    public partial class MaterialCallHeaderApp
    {
        private IUnitWork _unitWork;
        public IRepository<MaterialCallHeader> _app;
        public IRepository<MaterialDistributeTaskHeader> _appmh;
        public IRepository<MaterialDistributeTaskDetail> _appmd;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);

        public MaterialCallHeaderApp(IUnitWork unitWork, IRepository<MaterialCallHeader> repository, IRepository<MaterialDistributeTaskHeader> repositorymh, IRepository<MaterialDistributeTaskDetail> repositorymd, IHostingEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _app = repository;
            _appmh = repositorymh;
            _appmd = repositorymd;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public MaterialCallHeaderApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            _appmh._loginInfo = loginInfo;
            _appmd._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, MaterialCallHeader entity)
        {
            return _app.Load(pageRequest, entity);
        }

        public void Ins(MaterialCallHeader entity)
        {
            _app.Add(entity);
        }

        public void Upd(MaterialCallHeader entity)
        {
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public MaterialCallHeader FindSingle(Expression<Func<MaterialCallHeader, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<MaterialCallHeader> Find(Expression<Func<MaterialCallHeader, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<MaterialCallHeader> exp = imp.ConvertToModel<MaterialCallHeader>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    MaterialCallHeader e = exp[i];
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

        public TableData ExportData(MaterialCallHeader entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(MaterialCallHeader entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<MaterialCallHeader>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<MaterialCallHeader> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
        public TableData CreateDistributeTaskApp(List<MaterialCallHeader> mcheaderlist)
        {
            TableData tab = new TableData
            {
                code = 200
            };
            int hcount = 0;
            int listcount = 0;
            string sErrorMsg = "";
            Location location = null ;
            try
            {
                listcount = mcheaderlist.Count;
                foreach (MaterialCallHeader materialCallHeader in mcheaderlist)
                {
                    MaterialDistributeTaskHeader materialDistributeTaskHeader = new MaterialDistributeTaskHeader();
                    List<MaterialCallDetail> materialCallDetails = _unitWork.Find<MaterialCallDetail>(u=>u.CallHeaderId.Equals(materialCallHeader.Id)).ToList();
                    if (!_unitWork.IsExist<MaterialDistributeTaskHeader>(u=>u.MaterialCallId.Equals(materialCallHeader.Id)))
                    {
                        //生成配送主数据
                        // MaterialDistributeTaskHeader materialDistributeTaskHeader = new MaterialDistributeTaskHeader();
                        var pcode = _unitWork.Find<MaterialCallDetail>(u=>u.CallHeaderId== materialCallHeader.Id).Select(a=>a.ProductCode).FirstOrDefault();
                        var pid = _unitWork.Find<ProductHeader>(u => u.Code == pcode).Select(a => a.Id).FirstOrDefault();
                        location = _unitWork.Find<Location>(u => u.Code == materialCallHeader.LocationCode).FirstOrDefault();
                        materialDistributeTaskHeader.MaterialCallId = materialCallHeader.Id;
                        materialDistributeTaskHeader.ProductId = pid;
                        materialDistributeTaskHeader.ProductCode = pcode;
                        materialDistributeTaskHeader.NeedStation = materialCallHeader.NeedStation;
                        materialDistributeTaskHeader.LocationCode = materialCallHeader.LocationCode;
                        materialDistributeTaskHeader.ContainerType = location.Type;
                        if (!string.IsNullOrEmpty(location.ContainerCode))
                        {
                            materialDistributeTaskHeader.ContainerCode = location.ContainerCode;
                        }
                        materialDistributeTaskHeader.NeedTime = materialCallHeader.CallTime;
                        materialDistributeTaskHeader.Status = 1;
                        materialDistributeTaskHeader.UserCode = _app._loginInfo.Account;
                        if (materialCallHeader.CallType==CallType.上料)
                        {
                            if (location.Status == LocationStatus.空仓位)
                            {//第一次上料
                                materialDistributeTaskHeader.TaskNo = _app.GetTaskNo(AGVTaskNo.工位叫料);
                                materialDistributeTaskHeader.CallType = TaskType.上料配送;
                                materialDistributeTaskHeader.EndPlace = materialCallHeader.LocationCode;
                                if (string.IsNullOrEmpty(pcode))
                                {
                                    materialDistributeTaskHeader.MaterialConfirm = MaterialConfirm.已确认;
                                }
                                else
                                {
                                    materialDistributeTaskHeader.MaterialConfirm = MaterialConfirm.未确认;
                                }
                                _appmh.Add(materialDistributeTaskHeader);
                            }
                            else
                            {
                                //先生成取空料框任务
                                MaterialDistributeTaskHeader mt = new MaterialDistributeTaskHeader();
                                materialDistributeTaskHeader.CopyTo(mt);
                                mt.Id = null;
                                mt.TaskNo = _app.GetTaskNo(AGVTaskNo.取空料框);
                                mt.CallType = TaskType.回收料框;
                                mt.StartPlace = materialCallHeader.LocationCode;
                                mt.MaterialConfirm = MaterialConfirm.已确认;
                                _appmh.Add(mt);
                                //再成上料任务
                                materialDistributeTaskHeader.TaskNo = _app.GetTaskNo(AGVTaskNo.工位叫料);
                                materialDistributeTaskHeader.CallType = TaskType.上料配送;
                                materialDistributeTaskHeader.StartPlace = "";
                                materialDistributeTaskHeader.ContainerCode = "";
                                materialDistributeTaskHeader.EndPlace = materialCallHeader.LocationCode;
                                if (string.IsNullOrEmpty(pcode))
                                {
                                    materialDistributeTaskHeader.MaterialConfirm = MaterialConfirm.已确认;
                                }
                                else
                                {
                                    materialDistributeTaskHeader.MaterialConfirm = MaterialConfirm.未确认;
                                }
                                _appmh.Add(materialDistributeTaskHeader);
                            }
                            //只有呼叫类型是上料任务时，才有呼叫明细数据,其它呼叫类型无需写入任务配送明细表
                            if (materialCallDetails.Count > 0)
                            {
                                foreach (MaterialCallDetail mcdetail in materialCallDetails)
                                {
                                    List<MaterialDemand> materialDemands = _unitWork.Find<MaterialDemand>(u => u.OrderCode == mcdetail.OrderCode).ToList();
                                    //判断此工单的物料需求是否生成
                                    if (materialDemands.Count > 0)
                                    {
                                        //判断此工单的物料配送任务是否已经建立
                                        //if (!_unitWork.IsExist<MaterialDistributeTaskDetail>(u => u.OrderCode == mcdetail.OrderCode))
                                        //{
                                        //生成配送明细数据
                                        foreach (MaterialDemand md in materialDemands)
                                        {
                                            if (md.ClassABC == location.Type)
                                            {
                                                MaterialDistributeTaskDetail materialDistributeTaskDetail = new MaterialDistributeTaskDetail();
                                                materialDistributeTaskDetail.MaterialDistributeTaskHeaderId = materialDistributeTaskHeader.Id;
                                                materialDistributeTaskDetail.OrderCode = md.OrderCode;
                                                materialDistributeTaskDetail.MaterialCode = md.MaterialCode;
                                                materialDistributeTaskDetail.SerialNumber = mcdetail.SerialNumber;
                                                materialDistributeTaskDetail.Qty = md.DistributeQty;
                                                materialDistributeTaskDetail.UserCode = _app._loginInfo.Account;
                                                _appmd.Add(materialDistributeTaskDetail);
                                            }

                                        }
                                        hcount = hcount + 1;
                                        //}
                                        //else
                                        //{
                                        //    sErrorMsg += "工单号为：" + mcdetail.OrderCode + "的配送任务已经建立过，不可重复建立！<br>";
                                        //    tab.msg = sErrorMsg;
                                        //}


                                    }
                                    else
                                    {
                                        sErrorMsg += "工单号为：" + mcdetail.OrderCode + "未建立工单对应的物料需求，无法生成！<br>";
                                        tab.msg = sErrorMsg;
                                    }
                                }
                            }
                        }
                        else if (materialCallHeader.CallType == CallType.下料)
                        {
                            if (location.Status == LocationStatus.空仓位)
                            {//第一次下料，即只补空料框
                                materialDistributeTaskHeader.TaskNo = _app.GetTaskNo(AGVTaskNo.补给空框);
                                materialDistributeTaskHeader.CallType = TaskType.补给料框;
                                materialDistributeTaskHeader.EndPlace = materialCallHeader.LocationCode;
                                materialDistributeTaskHeader.MaterialConfirm = MaterialConfirm.已确认;
                                _appmh.Add(materialDistributeTaskHeader);
                            }
                            else
                            {//先生成下料任务
                                materialDistributeTaskHeader.TaskNo = _app.GetTaskNo(AGVTaskNo.成品下料);
                                materialDistributeTaskHeader.CallType = TaskType.下料取件;
                                materialDistributeTaskHeader.StartPlace = materialCallHeader.LocationCode;
                                _appmh.Add(materialDistributeTaskHeader);
                                //再生成补空料框任务
                                MaterialDistributeTaskHeader mtbk = new MaterialDistributeTaskHeader();
                                materialDistributeTaskHeader.CopyTo(mtbk);
                                mtbk.Id = null;
                                mtbk.TaskNo = _app.GetTaskNo(AGVTaskNo.补给空框);
                                mtbk.CallType = TaskType.补给料框;
                                mtbk.ContainerCode = "";
                                mtbk.StartPlace ="";
                                mtbk.EndPlace = materialCallHeader.LocationCode;
                                mtbk.MaterialConfirm = MaterialConfirm.已确认;
                                _appmh.Add(mtbk);
                            }
                        }
                    }


                }

                if (listcount <= hcount)
                {
                    tab.code = 200;
                    tab.msg = "配送任务已成功生成！";
                }
                else
                {
                    tab.code = 200;
                    tab.msg = "执行完成！部分生成失败的信息如下：<br>" + tab.msg;
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

