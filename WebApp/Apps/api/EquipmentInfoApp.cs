using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using WebRepository;

namespace WebApp
{
    /// <summary>
    /// 设备运行数据接口App
    /// </summary>

    public partial class EquipmentInfoApp : ApiApp
    {
        public EquipmentInfoApp(IUnitWork unitWork, IAuth auth, BaseDBContext context) : base(unitWork, auth, context)
        {

        }
        public Response EquipmentWorkNodeApp(EquipmentWorkNodeModel equipmentWorkNode)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                Response<ReviseModel> Response = new Response<ReviseModel>();
                string sErrorMsg = "";
                //if (!CheckLogin())
                //{
                //    Response.Code = 500;
                //    Response.Status = false;
                //    Response.Message = "请先登录！";
                //    return Response;
                //}

                try
                {
                    Equipment equipment = _unitWork.FindSingle<Equipment>(u => u.Code.Equals(equipmentWorkNode.EquipmentCode));
                    if (equipment == null)
                    {
                        sErrorMsg += "设备号为：" + equipmentWorkNode.EquipmentCode + "系统中不存在，请确认<br>";
                        Response.Message = sErrorMsg;
                    }
                    else
                    {
                        EquipmentStatus equipmentStatus = _unitWork.FindSingle<EquipmentStatus>(u => u.EquipmentId.Equals(equipment.Id));
                        if (equipmentWorkNode.EquipmentCode == EquipmentStart.桁车1 || equipmentWorkNode.EquipmentCode == EquipmentStart.桁车2 || equipmentWorkNode.EquipmentCode == EquipmentStart.桁车3)
                        {
                            StepTrace stCar = _unitWork.Find<StepTrace>(u => u.SerialNumber == equipmentWorkNode.SerialNumber).FirstOrDefault();
                            if (equipmentWorkNode.Status == "1")
                            {
                                stCar.StationId = equipment.StationId;
                                stCar.StationOutTime = DateTime.Now;
                                equipmentStatus.Status = equipmentWorkNode.Status;
                            }
                            else
                            {
                                stCar.StationId = 0;
                                stCar.StationInTime = DateTime.Now;
                                equipmentStatus.Status = "0";
                            }
                            _unitWork.UpdateByTracking(stCar);
                            equipmentStatus.UpdateBy = "ECSApi";
                            equipmentStatus.UpdateTime = DateTime.Now;
                            _unitWork.Update(equipmentStatus);
                        }
                        else
                        {
                            //工位工序对应关系
                            StepStation stepStation = _unitWork.Find<StepStation>(u => u.StationId == equipment.StationId).FirstOrDefault();
                            Step step = _unitWork.Find<Step>(u => u.ProductCode == equipmentWorkNode.ProductCode && u.StepType == stepStation.StepType).FirstOrDefault();

                            //更新设备状态
                            if (equipmentWorkNode.Status == "0")
                            {
                                equipmentStatus.Status = equipmentWorkNode.Status;
                                equipmentStatus.UpdateBy = "ECSApi";
                                equipmentStatus.UpdateTime = DateTime.Now;
                                _unitWork.Update(equipmentStatus);
                            }
                            else if (equipmentWorkNode.Status == "1")//开始工作
                            {

                                if (equipmentWorkNode.EquipmentCode == EquipmentStart.组对1 || equipmentWorkNode.EquipmentCode == EquipmentStart.组对2 || equipmentWorkNode.EquipmentCode == EquipmentStart.组对3 || equipmentWorkNode.EquipmentCode == EquipmentStart.组对4 || equipmentWorkNode.EquipmentCode == EquipmentStart.组对5 || equipmentWorkNode.EquipmentCode == EquipmentStart.组对6)
                                {//开始生产，写入在制品
                                    StepTrace steptrace = _unitWork.FindSingle<StepTrace>(u => u.WONumber.Equals(equipmentWorkNode.OrderCode));
                                    if (steptrace == null)
                                    {
                                        steptrace = new StepTrace();
                                        steptrace.WONumber = equipmentWorkNode.OrderCode;
                                        steptrace.ProductCode = equipmentWorkNode.ProductCode;
                                        steptrace.SerialNumber = equipmentWorkNode.SerialNumber;
                                        steptrace.LineId = equipment.LineId;
                                        steptrace.StepId = step.Id;
                                        steptrace.StationId = equipment.StationId;
                                        steptrace.NextStepId = _unitWork.FindSingle<Step>(u => u.Sequence == step.Sequence + 1).Id;
                                        steptrace.IsNG = false;
                                        steptrace.IsInvalid = false;
                                        steptrace.LineInTime = DateTime.Now;
                                        steptrace.StationInTime = DateTime.Now;
                                        steptrace.CreateBy = "ECSApi";
                                        steptrace.CreateTime = DateTime.Now;
                                        _unitWork.Add(steptrace);
                                    }
                                }
                                else
                                {
                                    StepTrace st = _unitWork.Find<StepTrace>(u => u.SerialNumber == equipmentWorkNode.SerialNumber).FirstOrDefault();
                                    st.StepId = step.Id;
                                    st.StationId = equipment.StationId;
                                    if (step.Sequence == 4)
                                    {
                                        st.NextStepId = 0;
                                    }
                                    else
                                    {
                                        st.NextStepId = _unitWork.FindSingle<Step>(u => u.Sequence == step.Sequence + 1).Id;
                                    }
                                    st.StationInTime = DateTime.Now;
                                    _unitWork.UpdateByTracking(st);
                                }

                                equipmentStatus.Status = equipmentWorkNode.Status;
                                equipmentStatus.UpdateBy = "ECSApi";
                                equipmentStatus.UpdateTime = DateTime.Now;
                                _unitWork.Update(equipmentStatus);
                            }
                            else if (equipmentWorkNode.Status == "2")//完成
                            {

                                equipmentStatus.Status = "0";
                                equipmentStatus.UpdateBy = "ECSApi";
                                equipmentStatus.UpdateTime = DateTime.Now;
                                _unitWork.Update(equipmentStatus);
                                //执行其它逻辑块(如更新steptrace在制品信息，呼叫桁车已转至下一个工序，给三一MES工序报工等)
                                if (_unitWork.IsExist<StepTrace>(u => u.SerialNumber == equipmentWorkNode.SerialNumber))
                                {
                                    //在制品追踪表
                                    StepTrace stepTrace = _unitWork.Find<StepTrace>(u => u.SerialNumber == equipmentWorkNode.SerialNumber).FirstOrDefault();
                                    stepTrace.StationOutTime = DateTime.Now;
                                    //取下道工序
                                    int? StationIdSequence = _unitWork.FindSingle<Step>(u => u.Id.Equals(stepTrace.StepId)).Sequence;
                                    //如果下道工序为最后一道机加工序则为空否则取下道工序
                                    if (step.Sequence == 4)
                                    {
                                        stepTrace.LineOutTime = DateTime.Now;
                                        stepTrace.NextStepId = 0;
                                        _unitWork.UpdateByTracking(stepTrace);
                                        OrderHeader oh;
                                        if (equipmentWorkNode.OrderCode != null)
                                        {
                                            oh = _unitWork.Find<OrderHeader>(u => u.Code == equipmentWorkNode.OrderCode).FirstOrDefault();
                                        }
                                        else
                                        {
                                            var headerid = _unitWork.FindSingle<OrderDetiail>(u => u.SerialNumber == equipmentWorkNode.SerialNumber).OrderHeaderId;
                                            oh = _unitWork.Find<OrderHeader>(u => u.Id == headerid).FirstOrDefault();
                                        }
                                        //更新明细状态为完成
                                        OrderDetiail orderdt = _unitWork.Find<OrderDetiail>(u => u.OrderCode.Equals(equipmentWorkNode.OrderCode) && u.SerialNumber == equipmentWorkNode.SerialNumber).FirstOrDefault();
                                        orderdt.ExecuteStatus = "done";
                                        orderdt.EndTime = DateTime.Now;
                                        orderdt.UpdateBy = "ECSApi";
                                        orderdt.UpdateTime = DateTime.Now;
                                        _unitWork.Update(orderdt);
                                        if (oh.CompleteQty == null)
                                        {
                                            oh.CompleteQty = 0;
                                        }
                                        oh.CompleteQty = oh.CompleteQty + 1;
                                        List<OrderDetiail> orderDetiails = _unitWork.Find<OrderDetiail>(u => u.OrderCode.Equals(equipmentWorkNode.OrderCode) && u.ExecuteStatus != "done").ToList();
                                        if (orderDetiails.Count == 0)
                                        {
                                            //更新工单状态为已完成
                                            oh.Status = OrderStatus.完成;
                                            oh.ActualEndTime = DateTime.Now;
                                            oh.UpdateBy = "ECSApi";
                                            oh.UpdateTime = DateTime.Now;
                                            _unitWork.Update(oh);
                                        }

                                        if (oh.Status == OrderStatus.完成)
                                        {
                                            //在制品写入历史表
                                            StepTraceHistory stepTraceHistory = _unitWork.FindSingle<StepTraceHistory>(u => u.WONumber.Equals(stepTrace.WONumber));
                                            if (stepTraceHistory == null)
                                            {
                                                stepTraceHistory = new StepTraceHistory();
                                                stepTrace.CopyTo(stepTraceHistory);
                                                stepTraceHistory.Id = null;
                                                _unitWork.Add(stepTraceHistory);
                                                //删除已完成的在制品数据
                                                _unitWork.DeleteByTracking(stepTrace);
                                            }

                                            OrderHeaderHistory orderHeaderHistory = _unitWork.FindSingle<OrderHeaderHistory>(u => u.Code.Equals(oh.Code));
                                            if (orderHeaderHistory == null)
                                            {
                                                orderHeaderHistory = new OrderHeaderHistory();
                                                //工单主表写入工单历史表
                                                oh.CopyTo(orderHeaderHistory);
                                                orderHeaderHistory.Id = null;
                                                _unitWork.Add(orderHeaderHistory);
                                                //写入工单明细历史表
                                                List<OrderDetiail> detiails = _unitWork.Find<OrderDetiail>(u => u.OrderHeaderId.Equals(oh.Id)).ToList();
                                                foreach (OrderDetiail oddt in detiails)
                                                {
                                                    OrderDetiailHistory orderDetiailHistory = new OrderDetiailHistory();
                                                    oddt.CopyTo(orderDetiailHistory);
                                                    orderDetiailHistory.Id = null;
                                                    orderDetiailHistory.OrderHeaderId = orderHeaderHistory.Id;
                                                    _unitWork.Add(orderDetiailHistory);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //stepTrace.NextStepId = _unitWork.FindSingle<Step>(u => u.Sequence == StationIdSequence + 1).Id;
                                        stepTrace.UpdateBy = "ECSApi";
                                        stepTrace.UpdateTime = DateTime.Now;
                                        _unitWork.Update(stepTrace);
                                    }
                                    //写入过站记录表
                                    StepTraceLog stepTraceLog = _unitWork.FindSingle<StepTraceLog>(u => u.WONumber.Equals(stepTrace.WONumber) && u.SerialNumber == stepTrace.SerialNumber && u.StationId == stepTrace.StationId);
                                    if (stepTraceLog == null)
                                    {
                                        stepTraceLog = new StepTraceLog();
                                        stepTraceLog.WONumber = stepTrace.WONumber;
                                        stepTraceLog.ProductCode = stepTrace.ProductCode;
                                        stepTraceLog.SerialNumber = stepTrace.SerialNumber;
                                        stepTraceLog.LineId = stepTrace.LineId;
                                        stepTraceLog.StationId = stepTrace.StationId;
                                        stepTraceLog.PassOrFail = "pass";
                                        stepTraceLog.IsNG = stepTrace.IsNG;
                                        stepTraceLog.NGcode = stepTrace.NGcode;
                                        stepTraceLog.LineInTime = stepTrace.LineInTime;
                                        stepTraceLog.StationInTime = stepTrace.StationInTime;
                                        stepTraceLog.StationOutTime = DateTime.Now;
                                        stepTraceLog.CreateBy = "ECSApi";
                                        stepTraceLog.CreateTime = DateTime.Now;
                                        //如果工序为最后一道机加则更新出线时间
                                        if (step.Sequence == 4)
                                        {
                                            stepTraceLog.LineOutTime = DateTime.Now;
                                        }
                                        _unitWork.Add(stepTraceLog);
                                    }
                                }
                                else
                                {
                                    sErrorMsg += "在制品：" + equipmentWorkNode.SerialNumber + "不存在或已完工，请至在制品历史页面确认！<br>";
                                    Response.Message = sErrorMsg;
                                }

                            }
                            else
                            {
                                sErrorMsg += "状态为：" + equipmentWorkNode.Status + "不识别的设备状态<br>";
                                Response.Message = sErrorMsg;
                            }
                        }

                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    Response.Code = 500;
                    Response.Status = false;
                    Response.Message = (Response.Message == "操作成功" ? "" : Response.Message) + "\r\n" + "EquipmentCode:" + equipmentWorkNode.EquipmentCode + "反馈失败！" + ex.Message;
                }

                return Response;
            }
        }

    }
}

