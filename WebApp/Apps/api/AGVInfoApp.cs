using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using WebRepository;

namespace WebApp
{
    /// <summary>
    /// AGV配送数据接口App
    /// </summary>

    public partial class AGVInfoApp : ApiApp
    {
        public AGVInfoApp(IUnitWork unitWork, IAuth auth, BaseDBContext context) : base(unitWork, auth, context)
        {

        }
        public Response AGVTaskNodeApp(TaskNodeModel taskNodeModel)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                Response<TaskNodeModel> Response = new Response<TaskNodeModel>();
                string sErrorMsg = "";
                Location location = null;
                Container container = null;
                if (!CheckLogin())
                {
                    Response.Code = 500;
                    Response.Status = false;
                    Response.Message = "请先登录！";
                    return Response;
                }

                try
                {
                    MaterialDistributeTaskHeader materialDistributeTaskHeader = _unitWork.FindSingle<MaterialDistributeTaskHeader>(u => u.TaskNo.Equals(taskNodeModel.TaskNo));
                    if (materialDistributeTaskHeader == null)
                    {
                        sErrorMsg += "任务号为：" + taskNodeModel.TaskNo + "不存在，请确认<br>";
                        Response.Message = sErrorMsg;
                    }
                    else
                    {
                        MaterialCallHeader materialCallHeader = _unitWork.FindSingle<MaterialCallHeader>(u => u.Id.Equals(materialDistributeTaskHeader.MaterialCallId));
                        List<MaterialCallDetail> materialCallDetail = _unitWork.Find<MaterialCallDetail>(u => u.CallHeaderId.Equals(materialCallHeader.Id)).ToList();
                        //更新配送状态
                        if (materialDistributeTaskHeader.Status < AGVTaskState.任务完成)
                        {

                            if (taskNodeModel.Status == AGVTaskState.配送完成 || taskNodeModel.Status == AGVTaskState.放料车完成)
                            {
                                //更新配送任务头表状态为完成
                                materialDistributeTaskHeader.Status = AGVTaskState.任务完成;
                                //同步更新物料呼叫状态为完成
                                materialCallHeader.Status = CallStatus.完成;
                                materialCallHeader.UpdateTime = DateTime.Now;
                                materialCallHeader.UpdateBy = "AGVapi";
                                _unitWork.Update(materialCallHeader);

                                if (materialCallDetail.Count>0)
                                {
                                    foreach (MaterialCallDetail mcdetail in materialCallDetail)
                                    {
                                        //同步更新物料需求状态
                                        List<MaterialDemand> materialDemands = _unitWork.Find<MaterialDemand>(u => u.OrderCode.Equals(mcdetail.OrderCode) && u.ProductCode == mcdetail.ProductCode).ToList();
                                        foreach (MaterialDemand md in materialDemands)
                                        {
                                            md.Status = CallStatus.完成;
                                            md.UpdateTime = DateTime.Now;
                                            md.UpdateBy = "AGVapi";
                                            _unitWork.Update(md);
                                        }
                                    }
                                }

                                //上料缓存区域
                                if (_unitWork.IsExist<Location>(u => u.Code == materialDistributeTaskHeader.LocationCode))
                                {
                                    //更新缓存区位置存放的料框编号和状态
                                    location = _unitWork.Find<Location>(u => u.Code == materialDistributeTaskHeader.LocationCode).FirstOrDefault();
                                    location.UpdateTime = DateTime.Now;
                                    location.UpdateBy = "AGVapi";
                                    location.ContainerCode = materialDistributeTaskHeader.ContainerCode;
                                    location.Status = LocationStatus.有货;
                                    _unitWork.Update(location);
                                }
                                else
                                {
                                    throw new Exception("缓存位置为：" + materialDistributeTaskHeader.LocationCode + "不存在，请先建立！");
                                }

                                    //容器
                                    if (_unitWork.IsExist<Container>(u => u.Code == materialDistributeTaskHeader.ContainerCode))
                                    {
                                        container = _unitWork.Find<Container>(u => u.Code == materialDistributeTaskHeader.ContainerCode).FirstOrDefault();
                                        container.UpdateTime = DateTime.Now;
                                        container.UpdateBy = "AGVapi";
                                        container.LocationCode = materialDistributeTaskHeader.LocationCode;
                                        container.Status = ContainerStatus.有;
                                        _unitWork.Update(container);
                                    }
                                    else
                                    {
                                        throw new Exception("料框为：" + materialDistributeTaskHeader.ContainerCode + "不存在，请先建立！");
                                    }

                            }
                            else if (taskNodeModel.Status == AGVTaskState.回收料框完成 || taskNodeModel.Status == AGVTaskState.取工件完成)
                            {
                                //更新缓存区位置存放的料框编号和状态
                                materialDistributeTaskHeader.Status = AGVTaskState.任务完成;
                                if (_unitWork.IsExist<Location>(u => u.Code == materialDistributeTaskHeader.LocationCode))
                                {
                                    //更新缓存区位置存放的料框编号和状态
                                    location = _unitWork.Find<Location>(u => u.Code == materialDistributeTaskHeader.LocationCode).FirstOrDefault();
                                    location.UpdateTime = DateTime.Now;
                                    location.UpdateBy = "AGVapi";
                                    location.ContainerCode = "";
                                    location.Status = LocationStatus.空仓位;
                                    _unitWork.Update(location);
                                }
                                else
                                {
                                    throw new Exception("缓存位置为：" + materialDistributeTaskHeader.LocationCode + "不存在，请先建立！");
                                }
                                

                                    //容器
                                    if (_unitWork.IsExist<Container>(u => u.Code == materialDistributeTaskHeader.ContainerCode))
                                    {
                                        container = _unitWork.Find<Container>(u => u.Code == materialDistributeTaskHeader.ContainerCode).FirstOrDefault();
                                        container.UpdateTime = DateTime.Now;
                                        container.UpdateBy = "AGVapi";
                                        container.LocationCode = "";
                                        container.Status = ContainerStatus.空;
                                        _unitWork.Update(container);
                                    }
                                    else
                                    {
                                        throw new Exception("料框为：" + materialDistributeTaskHeader.ContainerCode + "不存在，请先建立！");
                                    }

                            }
                            else if (taskNodeModel.Status == AGVTaskState.回收料框开始 || taskNodeModel.Status == AGVTaskState.取工件开始 || taskNodeModel.Status == AGVTaskState.放料车开始)
                            {
                                //更新配送任务头表的小车编号和料框编号
                                // materialDistributeTaskHeader.Status = AGVTaskState.回收料框开始;
                                materialDistributeTaskHeader.Status = taskNodeModel.Status;
                                if (taskNodeModel.Status == AGVTaskState.放料车开始)
                                {
                                    materialDistributeTaskHeader.ContainerCode = taskNodeModel.ContainerCode;
                                }
                                materialDistributeTaskHeader.CarNo = taskNodeModel.CarNo;
                                //同步更新物料呼叫状态
                                materialCallHeader.Status = CallStatus.执行;
                                materialCallHeader.UpdateTime = DateTime.Now;
                                materialCallHeader.UpdateBy = "AGVapi";
                                _unitWork.UpdateByTracking(materialCallHeader);
                            }
                            else if (taskNodeModel.Status == AGVTaskState.配送开始)
                            {
                                //更新配送任务头表的小车编号和料框编号
                                materialDistributeTaskHeader.Status = AGVTaskState.配送开始;
                                materialDistributeTaskHeader.CarNo = taskNodeModel.CarNo;
                                materialDistributeTaskHeader.ContainerCode = taskNodeModel.ContainerCode;
                                List<MaterialDistributeTaskDetail> materialDistributeTaskDetails = _unitWork.Find<MaterialDistributeTaskDetail>(u => u.MaterialDistributeTaskHeaderId.Equals(materialDistributeTaskHeader.Id)).ToList();
                                //更新配送明细
                                foreach (MaterialDistributeTaskDetail mdtdetail in materialDistributeTaskDetails)
                                {
                                    mdtdetail.ContainerCode = taskNodeModel.ContainerCode;
                                    mdtdetail.UpdateTime = DateTime.Now;
                                    mdtdetail.UpdateBy = "AGVapi";
                                    _unitWork.Update(mdtdetail);
                                }

                                //同步更新物料呼叫状态为完成
                                materialCallHeader.Status = CallStatus.执行;
                                materialCallHeader.UpdateTime = DateTime.Now;
                                materialCallHeader.UpdateBy = "AGVapi";
                                _unitWork.UpdateByTracking(materialCallHeader);


                                foreach (MaterialCallDetail mcdetail in materialCallDetail)
                                {
                                    //同步更新物料需求状态
                                    List<MaterialDemand> materialDemands = _unitWork.Find<MaterialDemand>(u => u.OrderCode.Equals(mcdetail.OrderCode) && u.ProductCode == mcdetail.ProductCode).ToList();
                                    foreach (MaterialDemand md in materialDemands)
                                    {
                                        md.Status = CallStatus.执行;
                                        md.UpdateTime = DateTime.Now;
                                        md.UpdateBy = "AGVapi";
                                        _unitWork.UpdateByTracking(md);
                                    }
                                }
                            }
                            else
                            {
                                materialDistributeTaskHeader.Status = taskNodeModel.Status;
                            }
                            materialDistributeTaskHeader.UpdateTime = DateTime.Now;
                            materialDistributeTaskHeader.UpdateBy = "AGVapi";
                            _unitWork.Update(materialDistributeTaskHeader);
                        }
                        else
                        {
                            sErrorMsg += "任务号为：" + taskNodeModel.TaskNo + "任务已经结束，无法更新状态<br>";
                            Response.Message = sErrorMsg;
                        }

                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    Response.Code = 500;
                    Response.Status = false;
                    Response.Message = (Response.Message == "操作成功" ? "" : Response.Message) + "\r\n" + "TaskNo:" + taskNodeModel.TaskNo + "AGV反馈失败！" + ex.Message;
                }

                return Response;
            }
        }
        public Response AGVStateInfoUploadApp(AgvMonitor agvMonitor)
        {
            Response<AgvMonitor> Response = new Response<AgvMonitor>();
            if (!CheckLogin())
            {
                Response.Code = 500;
                Response.Status = false;
                Response.Message = "请先登录！";
                return Response;
            }

            try
            {
                
                if (_unitWork.IsExist<AgvMonitor>(u => u.CarNo.Equals(agvMonitor.CarNo)) )
                {
                    AgvMonitor agvinfo = _unitWork.FindSingle<AgvMonitor>(u => u.CarNo.Equals(agvMonitor.CarNo));
                    agvinfo.TaskNo = agvMonitor.TaskNo;
                    agvinfo.PercentCapacity = agvMonitor.PercentCapacity;
                    agvinfo.ExceptionFlag = agvMonitor.ExceptionFlag;
                    agvinfo.State = agvMonitor.State;
                    agvinfo.ExceptionInfo = agvMonitor.ExceptionInfo;
                    agvinfo.UpdateTime = DateTime.Now;
                    agvinfo.UpdateBy = "AGVApi";
                    _unitWork.Update(agvinfo);             
                }
                else
                {
                    AgvMonitor agvinfoin = new AgvMonitor();
                    agvinfoin.CarNo = agvMonitor.CarNo;
                    agvinfoin.TaskNo = agvMonitor.TaskNo;
                    agvinfoin.PercentCapacity = agvMonitor.PercentCapacity;
                    agvinfoin.ExceptionFlag = agvMonitor.ExceptionFlag;
                    agvinfoin.State = agvMonitor.State;
                    agvinfoin.ExceptionInfo = agvMonitor.ExceptionInfo;
                    agvinfoin.CreateTime = DateTime.Now;
                    agvinfoin.CreateBy = "AGVApi";
                    _unitWork.Add(agvinfoin);
                }
            }
            catch (Exception ex)
            {
                Response.Code = 500;
                Response.Status = false;
                Response.Message = (Response.Message == "操作成功" ? "" : Response.Message) + "\r\n" + "小车编号:" + agvMonitor.CarNo + "AGV反馈失败！" + ex.Message;
            }

            return Response;
        }
        public Response AGVTaskTestApp(SendTaskModel sendTaskModel)
        {
            Response<SendTaskModel> Response = new Response<SendTaskModel>();
            string sErrorMsg = "";
            if (!CheckLogin())
            {
                Response.Code = 500;
                Response.Status = false;
                Response.Message = "请先登录！";
                return Response;
            }

            try
            {
                if (sendTaskModel == null)
                {
                    sErrorMsg += "任务号为：" + sendTaskModel.TaskNo + "传送数据为空，请确认<br>";
                    Response.Message = sErrorMsg;
                }
                else
                {
                    sendTaskModel.CarNo = "TEST88888";
                    sendTaskModel.ContainerCode = "C666666";
                    sendTaskModel.Status = 10;
                    Response.Result = sendTaskModel;


                }
            }
            catch (Exception ex)
            {
                Response.Code = 500;
                Response.Status = false;
                Response.Message = (Response.Message == "操作成功" ? "" : Response.Message) + "\r\n" + "OrderCode:" + sendTaskModel.TaskNo + "AGV反馈失败！" + ex.Message;
            }

            return Response;
        }
    }
}

