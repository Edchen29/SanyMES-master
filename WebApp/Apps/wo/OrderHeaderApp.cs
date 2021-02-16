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
	/// 订单表
	/// </summary>
    
    public partial class OrderHeaderApp
    {
        private IUnitWork _unitWork;
        public IRepository<OrderHeader> _app;
        public IRepository<OrderDetiail> _appdt;
        public IRepository<MaterialDemand> _appmd;
        public IRepository<SerialNumber> _apps;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);
        private BaseDBContext _context;

        public OrderHeaderApp(IUnitWork unitWork, IRepository<OrderHeader> repository, IRepository<MaterialDemand> repositorymd, IRepository<SerialNumber> repositorys, IRepository<OrderDetiail> repositorydt, IHostingEnvironment hostingEnvironment, BaseDBContext context)
        {
            _unitWork = unitWork;
            _app = repository;
            _appdt = repositorydt;
            _appmd = repositorymd;
            _apps = repositorys;
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }
        
        public OrderHeaderApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            _appdt._loginInfo = loginInfo;
            _appmd._loginInfo = loginInfo;
            _apps._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, OrderHeader entity)
        {
            return _app.Load(pageRequest, entity);
        }

        public void Ins(OrderHeader entity)
        {
            entity.PartMaterialCode = entity.ProductCode;
            _app.Add(entity);
        }

        public void Upd(OrderHeader entity)
        {
            entity.PartMaterialCode = entity.ProductCode;
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public OrderHeader FindSingle(Expression<Func<OrderHeader, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<OrderHeader> Find(Expression<Func<OrderHeader, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<OrderHeader> exp = imp.ConvertToModel<OrderHeader>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    OrderHeader e = exp[i];
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

        public TableData ExportData(OrderHeader entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(OrderHeader entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<OrderHeader>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<OrderHeader> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
        public TableData CreateSNApp(List<OrderHeader> ohlist)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                TableData tab = new TableData
                {
                    code = 200
                };
                int hcount = 0;
                int listcount = 0;
                string sErrorMsg = "";
                try
                {
                    listcount = ohlist.Count;
                    foreach (OrderHeader orderhearder in ohlist)
                    {
                        if (orderhearder.Status == OrderStatus.已准备)
                        {
                            if (_unitWork.IsExist<SerialNumber>(u => u.OrderCode.Equals(orderhearder.Code)))
                            {
                                sErrorMsg += "工单号为：" + orderhearder.Code + "已生成过SN,不可再生成！<br>";
                                tab.msg = sErrorMsg;
                            }
                            else
                            {
                                List<OrderDetiail> orderDetiails = _unitWork.Find<OrderDetiail>(u => u.OrderHeaderId.Equals(orderhearder.Id)).ToList();
                                for (int i = 0; i < orderhearder.PlanQty; i++)
                                {
                                    SerialNumber serialNumber = new SerialNumber();
                                    serialNumber.OrderCode = orderhearder.Code;
                                    serialNumber.ProductCode = orderhearder.ProductCode;
                                    serialNumber.SerialNumberMember = _app.GetTaskNo(orderhearder.ProductCode);
                                    if (_unitWork.IsExist<ProductBarcodeRule>(u => u.ProductCode == orderhearder.ProductCode))
                                    {
                                        serialNumber.SerialRule = _unitWork.FindSingle<ProductBarcodeRule>(u => u.ProductCode == orderhearder.ProductCode).Code;
                                    }
                                    else
                                    {
                                        serialNumber.SerialRule = "";
                                    }
                                    serialNumber.PrintCount = 0;
                                    _apps.Add(serialNumber);

                                    if (orderDetiails.Count > 0)
                                    {
                                        OrderDetiail orderDetiail = _unitWork.Find<OrderDetiail>(u => u.OrderCode.Equals(serialNumber.OrderCode)).Skip(i).Take(1).OrderBy(a=>a.Id).FirstOrDefault();
                                        //更新工单明细表的SN
                                        orderDetiail.SerialNumber = serialNumber.SerialNumberMember;
                                        _appdt.Update(orderDetiail);
                                    }
                                    else
                                    {
                                            //写入工单明细表
                                            OrderDetiail odin = new OrderDetiail();
                                            odin.OrderCode = serialNumber.OrderCode;
                                            odin.OrderHeaderId = orderhearder.Id;
                                            ProductHeader dno = _unitWork.FindSingle<ProductHeader>(u => u.Code.Equals(orderhearder.ProductCode));
                                            odin.DrawingNumber = dno.DrawingNumber;
                                            //if (_unitWork.IsExist<ProductHeader>(u => u.Code.Equals(orderhearder.ProductCode)))
                                            //{
                                            //    odin.DrawingNumber = _unitWork.FindSingle<ProductHeader>(u => u.Code.Equals(orderhearder.ProductCode)).DrawingNumber;
                                            //}
                                            odin.SerialNumber = serialNumber.SerialNumberMember;
                                            odin.ExecuteStatus = "ready";
                                            odin.QualityStatus = "good";
                                            _appdt.Add(odin);
                                    }

                                }


                                hcount = hcount + 1;
                            }

                        }
                        else
                        {
                            sErrorMsg += "工单号为：" + orderhearder.Code + "状态错误，为非准备状态！<br>";
                            tab.msg = sErrorMsg;
                        }

                    }

                    if (listcount <= hcount)
                    {
                        tab.code = 200;
                        tab.msg = "工单已全部成功生成SN！";
                    }
                    else
                    {
                        tab.code = 200;
                        tab.msg = "工单全部完成！部分未成功生成SN的信息如下：<br>" + tab.msg;
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    tab.code = 300;
                    tab.msg += ex.Message;
                }
                return tab;
            }
        }

        public TableData CreateMaterialListApp(List<OrderHeader> ohlist)
        {
            TableData tab = new TableData
            {
                code = 200
            };
            int hcount = 0;
            int listcount = 0;
            string sErrorMsg = "";
            try
            {
                listcount = ohlist.Count;
                foreach (OrderHeader orderhearder in ohlist)
                {
                    OrderHeader oh = _unitWork.Find<OrderHeader>(u => u.Code == orderhearder.Code).FirstOrDefault();
                    //判断此工单的物料需求数据是否已经建立
                    if (!_unitWork.IsExist<MaterialDemand>(u => u.OrderCode == orderhearder.Code))
                    {
                        if (oh.PartMaterialCode==null)
                        {
                            oh.PartMaterialCode = oh.ProductCode;
                        }
                        //判断此产品的生产MBOM主表数据是否建立
                        if (_unitWork.IsExist<MbomHeader>(n => n.ProductCode == oh.PartMaterialCode))
                        {

                            var mhid = _unitWork.FindSingle<MbomHeader>(n => n.ProductCode == oh.PartMaterialCode).Id;
                            List<MbomDetail> mbomDetails = _unitWork.Find<MbomDetail>(u => u.MbomHeaderId.Equals(mhid)).ToList();
                            //判断此产品的生产MBOM明细数据是否建立
                            if (mbomDetails.Count > 0)
                            {
                                //根据生产工单和生产MBOM信息生成工单物料需求数据
                                foreach (MbomDetail mbomdt in mbomDetails)
                                {
                                    var abctype = _unitWork.Find<Material>(u=>u.Code== mbomdt.MaterialCode).Select(a=>a.ClassABC).FirstOrDefault();
                                    MaterialDemand md = new MaterialDemand();
                                    md.OrderCode = oh.Code;
                                    md.ProductCode = oh.ProductCode;
                                    md.PartMaterialCode = oh.PartMaterialCode;
                                    md.ClassABC = abctype;
                                    md.DamandQty = oh.PlanQty * mbomdt.BaseQty;
                                    md.DistributeQty = mbomdt.BaseQty;
                                    md.MaterialCode = mbomdt.MaterialCode;
                                    md.OnlineQty = 0;
                                    md.OfflineQty = 0;
                                    md.Status = "ready";
                                    _appmd.Add(md);
                                }
                                hcount = hcount + 1;
                            }
                            else
                            {
                                sErrorMsg += "工单号为：" + oh.Code + "的生产MBOM明细资料未建立，请先去创建此工单的生产MBOM明细！<br>";
                                tab.msg = sErrorMsg;
                            }
                        }
                        else
                        {
                            sErrorMsg += "工单号为：" + oh.Code + "的生产MBOM未建立，请先去创建此工单的生产MBOM！<br>";
                            tab.msg = sErrorMsg;
                        }

                    }
                    else
                    {
                        sErrorMsg += "工单号为：" + oh.Code + "已经生成过物料需求清单，不可再生成！<br>";
                        tab.msg = sErrorMsg;
                    }

                }

                if (listcount <= hcount)
                {
                    tab.code = 200;
                    tab.msg = "已按工单全部成功生成物料需求清单！";
                }
                else
                {
                    tab.code = 200;
                    tab.msg = "执行完成！部分未成功生成的信息如下：<br>" + tab.msg;
                }
            }
            catch (Exception ex)
            {
                tab.code = 300;
                tab.msg += ex.Message;
            }
            return tab;
        }
        public TableData ReviseApp(List<OrderHeader> orderHeaderList,string revisetype)
        {
            TableData tab = new TableData
            {
                code = 200
            };
            int hcount = 0;
            int listcount = 0;
            string sErrorMsg = "";
            try
            {
                listcount = orderHeaderList.Count;
                foreach (OrderHeader orderhearder in orderHeaderList)
                {
                    OrderHeader oh = _unitWork.Find<OrderHeader>(u => u.Code == orderhearder.Code).FirstOrDefault();
                    //判断此工单的状态
                    if (oh.Status== "ready")
                    {
                        oh.Status = revisetype;
                        _app.Update(oh);

                        List<OrderDetiail> orderDetiails = _unitWork.Find<OrderDetiail>(u => u.OrderCode.Equals(orderhearder.Code)).ToList();
                        if (orderDetiails.Count > 0)
                        {
                            foreach (OrderDetiail orderdt in orderDetiails)
                            {
                                orderdt.ExecuteStatus = revisetype;
                                _appdt.Update(orderdt);
                            }
                        }

                        hcount = hcount + 1;
                    }
                    else
                    {
                        sErrorMsg += "工单号为：" + oh.Code + "不在准备状态，不允许修改<br>";
                        tab.msg = sErrorMsg;
                    }

                }

                if (listcount <= hcount)
                {
                    tab.code = 200;
                    tab.msg = "已按工单全部成功生成物料需求清单！";
                }
                else
                {
                    tab.code = 200;
                    tab.msg = "执行完成！部分未成功生成的信息如下：<br>" + tab.msg;
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

