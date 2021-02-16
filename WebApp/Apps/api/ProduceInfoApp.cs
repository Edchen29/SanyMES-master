using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using WebRepository;

namespace WebApp
{
    /// <summary>
    /// 生产管理数据接口App
    /// </summary>

    public partial class ProduceInfoApp : ApiApp
    {

        public ProduceInfoApp(IUnitWork unitWork, IAuth auth, BaseDBContext context) : base(unitWork, auth, context)
        {

        }
        public Response InsertOrder(InterfaceOrderModel interfaceorder)
        {
            Response Response = new Response();

            if (!CheckLogin())
            {
                Response.Code = 500;
                Response.Status = false;
                Response.Message = "请先登录！";
                return Response;
            }

            try
            {
                #region 保存订单接口主表
                InterfaceOrderHeader headdata = _unitWork.FindSingle<InterfaceOrderHeader>(u => u.Code.Equals(interfaceorder.interfaceOrderHeader.Code));
                interfaceorder.interfaceOrderHeader.Id = headdata?.Id;
                if (interfaceorder.interfaceOrderHeader.PartMaterialCode==null)
                {
                    interfaceorder.interfaceOrderHeader.PartMaterialCode = interfaceorder.interfaceOrderHeader.ProductCode;
                }
                if (interfaceorder.interfaceOrderHeader.Id == null)
                {
                    if (interfaceorder.interfaceOrderHeader.CreateBy == null)
                    {
                        interfaceorder.interfaceOrderHeader.CreateBy = "system";
                        interfaceorder.interfaceOrderHeader.CreateTime = DateTime.Now;
                    }
                    _unitWork.Add(interfaceorder.interfaceOrderHeader);
                }
                else
                {
                    if (interfaceorder.interfaceOrderHeader.UpdateBy == null)
                    {
                        interfaceorder.interfaceOrderHeader.UpdateBy = "system";
                        interfaceorder.interfaceOrderHeader.UpdateTime = DateTime.Now;
                    }
                    _unitWork.UpdateByTracking(interfaceorder.interfaceOrderHeader);
                }
                #endregion

                #region 保存订单主表
                OrderHeader orderHeader = _unitWork.FindSingle<OrderHeader>(u => u.Code.Equals(interfaceorder.interfaceOrderHeader.Code));
                if (orderHeader==null)
                {
                    orderHeader = new OrderHeader();
                }                 
                orderHeader.Code = interfaceorder.interfaceOrderHeader.Code;
                orderHeader.MachineType = interfaceorder.interfaceOrderHeader.MachineType;
                orderHeader.ProductCode = interfaceorder.interfaceOrderHeader.ProductCode;
                if (orderHeader.PartMaterialCode == null)
                {
                    orderHeader.PartMaterialCode = interfaceorder.interfaceOrderHeader.ProductCode;
                }
                else
                {
                    orderHeader.PartMaterialCode = interfaceorder.interfaceOrderHeader.PartMaterialCode;
                }
                orderHeader.PlanQty = (interfaceorder.interfaceOrderHeader.PlanQty == null ? 0 : interfaceorder.interfaceOrderHeader.PlanQty);
                orderHeader.CompleteQty = (interfaceorder.interfaceOrderHeader.CompleteQty == null ? 0 : interfaceorder.interfaceOrderHeader.CompleteQty);
                orderHeader.NGQty = (interfaceorder.interfaceOrderHeader.NGQty == null ? 0 : interfaceorder.interfaceOrderHeader.NGQty);
                if (interfaceorder.interfaceOrderHeader.Status==null)
                {
                    orderHeader.Status = OrderStatus.已准备;
                }
                else
                {
                    orderHeader.Status = interfaceorder.interfaceOrderHeader.Status;
                }
                if (interfaceorder.interfaceOrderHeader.Type == null)
                {
                    orderHeader.Type = OrderType.正常工单;
                }
                else
                {
                    orderHeader.Type = interfaceorder.interfaceOrderHeader.Type;
                }
                if (interfaceorder.interfaceOrderHeader.LineId == null)
                {
                    var workship = _unitWork.FindSingle<ProductHeader>(u => u.Code == interfaceorder.interfaceOrderHeader.ProductCode).WorkShop;
                    orderHeader.LineId = _unitWork.FindSingle<Line>(u=>u.WorkshopCode.Equals(workship)).Id;
                }
                else
                {
                    orderHeader.LineId = interfaceorder.interfaceOrderHeader.LineId;
                }
                if (interfaceorder.interfaceOrderHeader.Priority == null)
                {
                    orderHeader.Priority = 5;
                }
                else
                {
                    orderHeader.Priority = interfaceorder.interfaceOrderHeader.Priority;
                }
                orderHeader.LotNo = interfaceorder.interfaceOrderHeader.LotNo;
                if (interfaceorder.interfaceOrderHeader.LineId == null)
                {
                    orderHeader.WorkFactory = _unitWork.Find<Factory>(null).Select(u=>u.FactoryCode).FirstOrDefault();
                }
                else
                {
                    orderHeader.WorkFactory = interfaceorder.interfaceOrderHeader.WorkFactory;
                }
                
                orderHeader.PlanStartTime = interfaceorder.interfaceOrderHeader.PlanStartTime;
                orderHeader.PlanEndTime = interfaceorder.interfaceOrderHeader.PlanEndTime;
                orderHeader.ActualStartTime = interfaceorder.interfaceOrderHeader.ActualStartTime;
                orderHeader.ActualEndTime = interfaceorder.interfaceOrderHeader.ActualEndTime;
                orderHeader.ReserveNo = interfaceorder.interfaceOrderHeader.ReserveNo;
                orderHeader.ReserveRowNo = interfaceorder.interfaceOrderHeader.ReserveRowNo;
                orderHeader.CreateTime = interfaceorder.interfaceOrderHeader.CreateTime;
                orderHeader.CreateBy = interfaceorder.interfaceOrderHeader.CreateBy;
                orderHeader.UpdateTime = interfaceorder.interfaceOrderHeader.UpdateTime;
                orderHeader.UpdateBy = interfaceorder.interfaceOrderHeader.UpdateBy;

                if (orderHeader.Id == null)
                {
                    if (orderHeader.CreateBy == null)
                    {
                        orderHeader.CreateBy = "system";
                        orderHeader.CreateTime = DateTime.Now;
                    }
                    _unitWork.Add(orderHeader);
                }
                else
                {
                    if (orderHeader.UpdateBy == null)
                    {
                        orderHeader.UpdateBy = "system";
                        orderHeader.UpdateTime = DateTime.Now;
                    }
                    _unitWork.UpdateByTracking(orderHeader);
                }
                #endregion

                foreach (var item in interfaceorder.interfaceOrderDetails)
                {
                    try
                    {
                        #region 保存订单接口子表
                        InterfaceOrderDetiail data = _context.Set<InterfaceOrderDetiail>().AsQueryable().Where(u => u.OrderCode.Equals(item.OrderCode) && u.DrawingNumber.Equals(item.DrawingNumber)).SingleOrDefault();
                        item.Id = data?.Id;

                        if (item.Id == null)
                        {
                            if (item.CreateBy == null)
                            {
                                item.CreateBy = "system";
                                item.CreateTime = DateTime.Now;
                            }
                            _unitWork.Add(item);
                        }
                        else
                        {
                            if (item.UpdateBy == null)
                            {
                                item.UpdateBy = "system";
                                item.UpdateTime = DateTime.Now;
                            }
                            _unitWork.UpdateByTracking(item);
                        }
                        #endregion

                        #region 保存订单子表
                        OrderDetiail orderDetail = _context.Set<OrderDetiail>().AsQueryable().Where(u => u.OrderCode.Equals(item.OrderCode) && u.DrawingNumber.Equals(item.DrawingNumber)).SingleOrDefault();
                        if (orderDetail == null)
                        {
                            orderDetail = new OrderDetiail();
                        }

                        orderDetail.OrderCode = item.OrderCode;
                        orderDetail.OrderHeaderId = orderHeader.Id;
                        orderDetail.DrawingNumber = item.DrawingNumber;
                        orderDetail.SerialNumber = item.SerialNumber;
                        orderDetail.ExecuteStatus = "ready";
                        orderDetail.QualityStatus = "good";
                        orderDetail.StationTraceId = item.StationTraceId;
                        orderDetail.StartTime = item.StartTime;
                        orderDetail.EndTime = item.EndTime;
                        orderDetail.PrintCount = 0;
                        orderDetail.CreateTime = item.CreateTime;
                        orderDetail.CreateBy = item.CreateBy;
                        orderDetail.UpdateTime = item.UpdateTime;
                        orderDetail.UpdateBy = item.UpdateBy;


                        if (orderDetail.Id == null)
                        {
                            if (orderDetail.CreateBy == null)
                            {
                                orderDetail.CreateBy = "system";
                                orderDetail.CreateTime = DateTime.Now;
                            }
                            _unitWork.Add(orderDetail);
                        }
                        else
                        {
                            if (orderDetail.UpdateBy == null)
                            {
                                orderDetail.UpdateBy = "system";
                                orderDetail.UpdateTime = DateTime.Now;
                            }
                            _unitWork.UpdateByTracking(orderDetail);
                        }
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        #region 记录订单接口子表错误信息
                        InterfaceOrderDetiail data = _context.Set<InterfaceOrderDetiail>().AsQueryable().Where(u => u.OrderCode.Equals(item.OrderCode) && u.DrawingNumber.Equals(item.DrawingNumber)).SingleOrDefault();
                        if (data != null)
                        {
                            data.ErrorMessage = ex.Message;
                            if (data.UpdateBy == null)
                            {
                                data.UpdateBy = "system";
                                data.UpdateTime = DateTime.Now;
                            }
                            _unitWork.Update(data);
                        }
                        #endregion

                        Response.Code = 500;
                        Response.Status = false;
                        Response.Message = (Response.Message == "操作成功" ? "" : Response.Message) + "\r\n" + "订单号:" + item.OrderCode + ",产品图号:" + item.DrawingNumber + "同步失败:" + ex.Message;
                    }
                }

            }
            catch (Exception ex)
            {
                #region 记录订单接口主表错误信息
                try
                {
                    InterfaceOrderHeader data = _context.Set<InterfaceOrderHeader>().AsQueryable().Where(u => u.Code.Equals(interfaceorder.interfaceOrderHeader.Code)).SingleOrDefault();
                    if (data != null)
                    {
                        data.ErrorMessage = ex.Message;
                        if (data.UpdateBy == null)
                        {
                            data.UpdateBy = "system";
                            data.UpdateTime = DateTime.Now;
                        }
                        _unitWork.Update(data);
                    }
                }
                catch (Exception)
                {
                }
                #endregion

                Response.Code = 500;
                Response.Status = false;
                Response.Message = (Response.Message == "操作成功" ? "" : Response.Message) + "\r\n" + "同步失败:" + ex.Message;
            }

            return Response;
        }

        public Response InsertMBom(InterfaceMBomModel interfacembom)
        {
            Response Response = new Response();

            if (!CheckLogin())
            {
                Response.Code = 500;
                Response.Status = false;
                Response.Message = "请先登录！";
                return Response;
            }

            try
            {
                #region 保存生产MBOM接口主表
                InterfaceMbomHeader headdata = _context.Set<InterfaceMbomHeader>().AsQueryable().Where(u => u.ProductCode.Equals(interfacembom.interfaceMbomHeader.ProductCode)).SingleOrDefault();
                interfacembom.interfaceMbomHeader.Id = headdata?.Id;
                if (_unitWork.IsExist<ProductHeader>(u => u.Code.Equals(interfacembom.interfaceMbomHeader.ProductCode)))
                {
                    interfacembom.interfaceMbomHeader.ProductId = _unitWork.FindSingle<ProductHeader>(u => u.Code.Equals(interfacembom.interfaceMbomHeader.ProductCode)).Id;
                }
                else
                {
                    interfacembom.interfaceMbomHeader.ProductId = 0;
                }
                if (interfacembom.interfaceMbomHeader.Id == null)
                {
                    if (interfacembom.interfaceMbomHeader.CreateBy == null)
                    {
                        interfacembom.interfaceMbomHeader.CreateBy = "system";
                        interfacembom.interfaceMbomHeader.CreateTime = DateTime.Now;
                    }
                    _unitWork.Add(interfacembom.interfaceMbomHeader);
                }
                else
                {
                    if (interfacembom.interfaceMbomHeader.UpdateBy == null)
                    {
                        interfacembom.interfaceMbomHeader.UpdateBy = "system";
                        interfacembom.interfaceMbomHeader.UpdateTime = DateTime.Now;
                    }
                    _unitWork.UpdateByTracking(interfacembom.interfaceMbomHeader);

                }
                #endregion

                #region 保存生产MBOM主表
                MbomHeader mbomHeader = _unitWork.FindSingle<MbomHeader>(u => u.ProductCode.Equals(interfacembom.interfaceMbomHeader.ProductCode));
                if (mbomHeader == null)
                {
                    mbomHeader = new MbomHeader();
                }
                 
                mbomHeader.ProductCode = interfacembom.interfaceMbomHeader.ProductCode;
                if (_unitWork.IsExist<ProductHeader>(u => u.Code.Equals(interfacembom.interfaceMbomHeader.ProductCode)))
                {
                    mbomHeader.ProductId = _unitWork.FindSingle<ProductHeader>(u => u.Code.Equals(interfacembom.interfaceMbomHeader.ProductCode)).Id;
                }
                else
                {
                    mbomHeader.ProductId = 0;
                }
                mbomHeader.DrawingNumber = interfacembom.interfaceMbomHeader.DrawingNumber;
                mbomHeader.Version = interfacembom.interfaceMbomHeader.Version;
                if (interfacembom.interfaceMbomHeader.LineId == null)
                {
                    var workship = _unitWork.FindSingle<ProductHeader>(u => u.Code == interfacembom.interfaceMbomHeader.ProductCode).WorkShop;
                    mbomHeader.LineId = _unitWork.FindSingle<Line>(u => u.WorkshopCode.Equals(workship)).Id;
                }
                else
                {
                    mbomHeader.LineId = interfacembom.interfaceMbomHeader.LineId;
                }
                mbomHeader.Verifyer = interfacembom.interfaceMbomHeader.Verifyer;
                mbomHeader.CreateTime = interfacembom.interfaceMbomHeader.CreateTime;
                mbomHeader.CreateBy = interfacembom.interfaceMbomHeader.CreateBy;
                mbomHeader.UpdateTime = interfacembom.interfaceMbomHeader.UpdateTime;
                mbomHeader.UpdateBy = interfacembom.interfaceMbomHeader.UpdateBy;

                if (mbomHeader.Id == null)
                {
                    if (mbomHeader.CreateBy == null)
                    {
                        mbomHeader.CreateBy = "system";
                        mbomHeader.CreateTime = DateTime.Now;
                    }
                    _unitWork.Add(mbomHeader);
                }
                else
                {
                    if (mbomHeader.UpdateBy == null)
                    {
                        mbomHeader.UpdateBy = "system";
                        mbomHeader.UpdateTime = DateTime.Now;
                    }
                    _unitWork.UpdateByTracking(mbomHeader);
                }
                #endregion

                foreach (var item in interfacembom.interfaceMbomDetails)
                {
                    try
                    {
                        #region 保存生产MBOM接口子表
                        InterfaceMbomDetail data = _context.Set<InterfaceMbomDetail>().AsQueryable().Where(u => u.MbomHeaderId.Equals(interfacembom.interfaceMbomHeader.Id) && u.MaterialCode.Equals(item.MaterialCode)).SingleOrDefault();
                        item.Id = data?.Id;

                        if (item.Id == null)
                        {
                            item.ProductCode = interfacembom.interfaceMbomHeader.ProductCode;
                            item.MbomHeaderId = interfacembom.interfaceMbomHeader.Id;
                            if (item.CreateBy == null)
                            {
                                item.CreateBy = "system";
                                item.CreateTime = DateTime.Now;
                            }
                            _unitWork.Add(item);
                        }
                        else
                        {
                            if (item.UpdateBy == null)
                            {
                                item.UpdateBy = "system";
                                item.UpdateTime = DateTime.Now;
                            }
                            _unitWork.UpdateByTracking(item);
                        }
                        #endregion

                        #region 保存生产MBOM子表
                        MbomDetail mbomDetail = _context.Set<MbomDetail>().AsQueryable().Where(u => u.MbomHeaderId.Equals(mbomHeader.Id) && u.MaterialCode.Equals(item.MaterialCode)).SingleOrDefault();
                        if (mbomDetail == null)
                        {
                            mbomDetail = new MbomDetail();
                        }

                        mbomDetail.MbomHeaderId = mbomHeader.Id;
                        mbomDetail.ProductCode = interfacembom.interfaceMbomHeader.ProductCode;
                        mbomDetail.StepId = item.StepId;
                        mbomDetail.MaterialCode = item.MaterialCode;
                        mbomDetail.BaseQty = item.BaseQty;
                        if (item.IsCheck == null)
                        {
                            mbomDetail.IsCheck = false;
                        }
                        else
                        {
                            mbomDetail.IsCheck = item.IsCheck;
                        }
                        
                        mbomDetail.DrawingNumber = item.DrawingNumber;
                        mbomDetail.CreateTime = item.CreateTime;
                        mbomDetail.CreateBy = item.CreateBy;
                        mbomDetail.UpdateTime = item.UpdateTime;
                        mbomDetail.UpdateBy = item.UpdateBy;


                        if (mbomDetail.Id == null)
                        {
                            if (mbomDetail.CreateBy == null)
                            {
                                mbomDetail.CreateBy = "system";
                                mbomDetail.CreateTime = DateTime.Now;
                            }
                            _unitWork.Add(mbomDetail);
                        }
                        else
                        {
                            if (mbomDetail.UpdateBy == null)
                            {
                                mbomDetail.UpdateBy = "system";
                                mbomDetail.UpdateTime = DateTime.Now;
                            }
                            _unitWork.UpdateByTracking(mbomDetail);
                        }
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        #region 记录生产MBOM接口子表错误信息
                        InterfaceMbomDetail data = _context.Set<InterfaceMbomDetail>().AsQueryable().Where(u => u.MbomHeaderId.Equals(mbomHeader.Id) && u.MaterialCode.Equals(item.MaterialCode)).SingleOrDefault();
                        if (data != null)
                        {
                            data.ErrorMessage = ex.Message;
                            if (data.UpdateBy == null)
                            {
                                data.UpdateBy = "system";
                                data.UpdateTime = DateTime.Now;
                            }
                            _unitWork.Update(data);
                        }
                        #endregion

                        Response.Code = 500;
                        Response.Status = false;
                        Response.Message = (Response.Message == "操作成功" ? "" : Response.Message) + "\r\n" + "主表ID:" + item.MbomHeaderId + ",MaterialCode:" + item.MaterialCode + "同步失败:" + ex.Message;
                    }
                }

            }
            catch (Exception ex)
            {
                #region 记录生产MBOM接口主表错误信息
                try
                {
                    InterfaceMbomHeader data = _unitWork.FindSingle<InterfaceMbomHeader>(u => u.ProductCode.Equals(interfacembom.interfaceMbomHeader.ProductCode));
                    if (data != null)
                    {
                        data.ErrorMessage = ex.Message;
                        if (data.UpdateBy == null)
                        {
                            data.UpdateBy = "system";
                            data.UpdateTime = DateTime.Now;
                        }
                        _unitWork.Update(data);
                    }
                }
                catch (Exception)
                {
                }
                #endregion

                Response.Code = 500;
                Response.Status = false;
                Response.Message = (Response.Message == "操作成功" ? "" : Response.Message) + "\r\n" + "同步失败:" + ex.Message;
            }

            return Response;
        }
        public Response OrderReviseApp(ReviseModel revises)
        {
            Response<List<ReviseModel>> Response = new Response<List<ReviseModel>>();
            string sErrorMsg = "";
            if (!CheckLogin())
            {
                Response.Code = 500;
                Response.Status = false;
                Response.Message = "请先登录！";
                return Response;
            }

            foreach (var item in revises.Data)
            {
                try
                {
                    OrderHeader orderHeader = _unitWork.FindSingle<OrderHeader>(u => u.Code.Equals(item.OrderCode));
                    if (orderHeader == null)
                    {
                        sErrorMsg += "工单号为：" + item.OrderCode + "不存在，请确认<br>";
                        Response.Message = sErrorMsg;
                    }
                    else
                    {
                        if (orderHeader.Status == "ready")
                        {
                            orderHeader.Status = item.ReviseType;
                            _unitWork.Update(orderHeader);


                            List<OrderDetiail> orderDetiails = _unitWork.Find<OrderDetiail>(u => u.OrderCode.Equals(item.OrderCode)).ToList();

                            if (orderDetiails.Count > 0)
                            {
                                foreach (OrderDetiail orderdt in orderDetiails)
                                {
                                    orderdt.ExecuteStatus = item.ReviseType;
                                    _unitWork.Update(orderdt);
                                }
                            }
                        }
                        else
                        {
                            sErrorMsg += "工单号为：" + orderHeader.Code + "非准备状态，不允许修改<br>";
                            Response.Message = sErrorMsg;
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    Response.Code = 500;
                    Response.Status = false;
                    Response.Message = (Response.Message == "操作成功" ? "" : Response.Message) + "\r\n" + "Code:" + item.OrderCode + "修正失败！" + ex.Message;
                }
            }
            return Response;
        }
        public Response  QCCheckApp(QCCheckModel qccheck)
        {
            Response<QCCheckModel> Response = new Response<QCCheckModel>();
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
                if (qccheck.IsGood==0)//如果不合格
                {
                    OrderHeader orderHeader = _unitWork.FindSingle<OrderHeader>(u => u.Code.Equals(qccheck.OrderCode));
                    if (orderHeader == null)
                    {
                        sErrorMsg += "工单号为：" + qccheck.OrderCode + "不存在，请确认<br>";
                        Response.Message = sErrorMsg;
                    }
                    else
                    {
                        if (_unitWork.IsExist<Repair>(u => u.SerialNumber == qccheck.SerialNumber && u.MaterialCode == qccheck.MaterialCode))
                        {
                            sErrorMsg += "SN：" + qccheck.SerialNumber + "已经判定过，请勿重复判定<br>";
                            Response.Message = sErrorMsg;
                        }
                        else
                        {
                            Repair repair = new Repair();
                            repair.WONumber = qccheck.OrderCode;
                            repair.ItemCode = qccheck.ProductCode;
                            repair.SerialNumber = qccheck.SerialNumber;
                            repair.MaterialCode = qccheck.MaterialCode;
                            repair.NGcode = qccheck.NGCode;
                            repair.NGMarkUser = qccheck.NGMarkUser;
                            repair.CreateTime = DateTime.Now;
                            repair.CreateBy = "padApi";
                            _unitWork.Add(repair);
                        }
                        if (_unitWork.IsExist<StepTrace>(u => u.SerialNumber == qccheck.SerialNumber))
                        {
                            StepTrace stepTrace = _unitWork.Find<StepTrace>(u => u.SerialNumber == qccheck.SerialNumber).FirstOrDefault();
                            stepTrace.IsNG = true;
                            stepTrace.NGcode = qccheck.NGCode;
                            stepTrace.UpdateBy = qccheck.NGMarkUser;
                            stepTrace.UpdateTime = DateTime.Now;
                            _unitWork.UpdateByTracking(stepTrace);
                        }
                    }
                }
                    
            }
            catch (Exception ex)
            {
                Response.Code = 500;
                Response.Status = false;
                Response.Message = (Response.Message == "操作成功" ? "" : Response.Message) + "\r\n" + "Code:" + qccheck.OrderCode + "判定失败！" + ex.Message;
            }
            return Response;
        }
        public Response StockConfirmApp(MaterialDemandConfirmModel mdConfirmList)
        {
            Response Response = new Response();
            string sErrorMsg = "";
            //if (!CheckLogin())
            //{
            //    Response.Code = 500;
            //    Response.Status = false;
            //    Response.Message = "请先登录！";
            //    return Response;
            //}
            foreach (MaterialDemandConfirm mdConfirm in mdConfirmList.Data)
            {
                try
                {

                    MaterialDemand materialDemand = _unitWork.FindSingle<MaterialDemand>(u => u.Id.Equals(mdConfirm.Id));
                    if (materialDemand != null)
                    {
                        materialDemand.DistributeQty = mdConfirm.DistributeQty;
                        materialDemand.UpdateBy = "ConfirmAPI";
                        materialDemand.UpdateTime = DateTime.Now;
                        _unitWork.Update(materialDemand);
                    }
                    MaterialDistributeTaskDetail materialDistributeTaskDetail = _unitWork.FindSingle<MaterialDistributeTaskDetail>(u => u.OrderCode == mdConfirm.OrderCode && u.MaterialCode == mdConfirm.MaterialCode);
                    if (materialDistributeTaskDetail != null)
                    {
                        materialDistributeTaskDetail.Qty = mdConfirm.DistributeQty;
                        materialDistributeTaskDetail.ContainerCode = mdConfirm.ContainerCode;
                        materialDistributeTaskDetail.UpdateBy = "ConfirmAPI";
                        materialDistributeTaskDetail.UpdateTime = DateTime.Now;
                        _unitWork.Update(materialDistributeTaskDetail);

                        MaterialDistributeTaskHeader materialDistributeTaskHeader = _unitWork.FindSingle<MaterialDistributeTaskHeader>(u => u.Id == materialDistributeTaskDetail.MaterialDistributeTaskHeaderId);
                        if (materialDistributeTaskHeader.MaterialConfirm != MaterialConfirm.已确认)
                        {
                            materialDistributeTaskHeader.ContainerCode = mdConfirm.ContainerCode;
                            materialDistributeTaskHeader.StartPlace = mdConfirm.ContainerPlace;
                            materialDistributeTaskHeader.MaterialConfirm = MaterialConfirm.已确认;
                            materialDistributeTaskHeader.UpdateBy = "ConfirmAPI";
                            materialDistributeTaskHeader.UpdateTime = DateTime.Now;
                            _unitWork.Update(materialDistributeTaskHeader);
                        }
                    }
                

                }
                catch (Exception ex)
                {
                    Response.Code = 500;
                    Response.Status = false;
                    Response.Message = (Response.Message == "操作成功" ? "" : Response.Message) + "\r\n" + "OrderCode:" + mdConfirm.OrderCode + "确认失败！" + ex.Message;
                }
            }
            return Response;
        }
    }
}

