using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using WebRepository;

namespace WebApp
{
    /// <summary>
    /// 基础数据接口App
    /// </summary>

    public partial class BasicDataApp: ApiApp
    {
        public BasicDataApp(IUnitWork unitWork, IAuth auth, BaseDBContext context) : base(unitWork, auth, context)
        {

        }


        public Response Material(MaterialsModel Materials)
        {
            Response<List<Material>> Response = new Response<List<Material>>();

            if (!CheckLogin())
            {
                Response.Code = 500;
                Response.Status = false;
                Response.Message = "请先登录！";
              //  return JsonHelper.Instance.Serialize(Response);
                return Response;
            }

            foreach (var item in Materials.Data)
            {
                try
                {
                    Material data = _unitWork.FindSingle<Material>(u => u.Code.Equals(item.Code));
                    if (data == null)
                    {
                        item.Id = null;
                        if (item.CreateBy == null)
                        {
                            item.CreateBy = "API";
                            item.CreateTime = DateTime.Now;
                        }
                        _unitWork.Add(item);
                    }
                    else
                    {
                        item.Id = data.Id;
                        if (item.UpdateBy == null)
                        {
                            item.UpdateBy = "API";
                            item.UpdateTime = DateTime.Now;
                        }
                        _unitWork.UpdateByTracking(item);
                    }
                }
                catch (Exception)
                {
                    Response.Code = 500;
                    Response.Status = false;
                    Response.Message = (Response.Message == "操作成功" ? "" : Response.Message) + "\r\n" + "Code:" + item.Code + "同步失败！";

                    Response.Result.Add(item);
                }
            }

            //  return JsonHelper.Instance.Serialize(Response);
            return Response;
        }

        public Response InsertProduct(InterfaceProductModel interfaceproduct)
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
                #region 保存产品接口主表
                InterfaceProductHeader headdata = _unitWork.FindSingle<InterfaceProductHeader>(u => u.Code.Equals(interfaceproduct.interfaceProductHeader.Code));
                interfaceproduct.interfaceProductHeader.Id = headdata?.Id;
                if (interfaceproduct.interfaceProductHeader.Id == null)
                {
                    if (interfaceproduct.interfaceProductHeader.CreateBy == null)
                    {
                        interfaceproduct.interfaceProductHeader.CreateBy = "system";
                        interfaceproduct.interfaceProductHeader.CreateTime = DateTime.Now;
                    }
                    _unitWork.Add(interfaceproduct.interfaceProductHeader);
                }
                else
                {
                    if (interfaceproduct.interfaceProductHeader.UpdateBy == null)
                    {
                        interfaceproduct.interfaceProductHeader.UpdateBy = "system";
                        interfaceproduct.interfaceProductHeader.UpdateTime = DateTime.Now;
                    }
                    _unitWork.UpdateByTracking(interfaceproduct.interfaceProductHeader);
                }
                #endregion

                #region 保存产品主表
                ProductHeader productHeader = _unitWork.FindSingle<ProductHeader>(u => u.Code.Equals(interfaceproduct.interfaceProductHeader.Code));
                if (productHeader == null)
                {
                    productHeader = new ProductHeader();
                }
                productHeader.Type = interfaceproduct.interfaceProductHeader.Type;
                productHeader.Code = interfaceproduct.interfaceProductHeader.Code;
                productHeader.Name = interfaceproduct.interfaceProductHeader.Name;
                productHeader.Weight = (interfaceproduct.interfaceProductHeader.Weight == null ? 0 : interfaceproduct.interfaceProductHeader.Weight);
                productHeader.Specification = interfaceproduct.interfaceProductHeader.Specification;
                productHeader.MachineType = interfaceproduct.interfaceProductHeader.MachineType;
                productHeader.DrawingNumber = interfaceproduct.interfaceProductHeader.DrawingNumber;
                productHeader.WorkShop = interfaceproduct.interfaceProductHeader.WorkShop;
                productHeader.CreateTime = interfaceproduct.interfaceProductHeader.CreateTime;
                productHeader.CreateBy = interfaceproduct.interfaceProductHeader.CreateBy;
                productHeader.UpdateTime = interfaceproduct.interfaceProductHeader.UpdateTime;
                productHeader.UpdateBy = interfaceproduct.interfaceProductHeader.UpdateBy;

                if (productHeader.Id == null)
                {
                    if (productHeader.CreateBy == null)
                    {
                        productHeader.CreateBy = "system";
                        productHeader.CreateTime = DateTime.Now;
                    }
                    _unitWork.Add(productHeader);
                }
                else
                {
                    if (productHeader.UpdateBy == null)
                    {
                        productHeader.UpdateBy = "system";
                        productHeader.UpdateTime = DateTime.Now;
                    }
                    _unitWork.UpdateByTracking(productHeader);
                }
                #endregion
                if (interfaceproduct.interfaceProductDetails.Count>0)
                {
                    foreach (var item in interfaceproduct.interfaceProductDetails)
                    {
                        try
                        {
                            #region 保存产品接口子表
                            InterfaceProductDetail data = _context.Set<InterfaceProductDetail>().AsQueryable().Where(u => u.ProductHeaderId.Equals(productHeader.Id) && u.LineCode.Equals(item.LineCode) && u.StepCode.Equals(item.StepCode)).SingleOrDefault();
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

                            #region 保存产品子表
                            ProductDetail productDetail = _context.Set<ProductDetail>().AsQueryable().Where(u => u.ProductHeaderId.Equals(productHeader.Id) && u.LineCode.Equals(item.LineCode) && u.StepCode.Equals(item.StepCode)).SingleOrDefault();
                            if (productDetail == null)
                            {
                                productDetail = new ProductDetail();
                            }

                            productDetail.ProductHeaderId = productHeader.Id;
                            if (_unitWork.IsExist<Line>(u => u.LineCode.Equals(item.LineCode)))
                            {
                                productDetail.LineId = _unitWork.FindSingle<Line>(u => u.LineCode.Equals(item.LineCode)).Id;
                            }
                            else
                            {
                                productDetail.LineId = 0;
                            }

                            productDetail.LineCode = item.LineCode;
                            if (_unitWork.IsExist<Step>(u => u.Code.Equals(item.StepCode)))
                            {
                                productDetail.StepId = _unitWork.FindSingle<Step>(u => u.Code.Equals(item.StepCode)).Id;
                            }
                            else
                            {
                                productDetail.StepId = 0;
                            }
                            productDetail.StepCode = item.StepCode;
                            productDetail.CreateTime = item.CreateTime;
                            productDetail.CreateBy = item.CreateBy;
                            productDetail.UpdateTime = item.UpdateTime;
                            productDetail.UpdateBy = item.UpdateBy;
                            productDetail.ProgramCode = item.ProgramCode;

                            if (productDetail.Id == null)
                            {
                                if (productDetail.CreateBy == null)
                                {
                                    productDetail.CreateBy = "system";
                                    productDetail.CreateTime = DateTime.Now;
                                }
                                _unitWork.Add(productDetail);
                            }
                            else
                            {
                                if (productDetail.UpdateBy == null)
                                {
                                    productDetail.UpdateBy = "system";
                                    productDetail.UpdateTime = DateTime.Now;
                                }
                                _unitWork.UpdateByTracking(productDetail);
                            }
                            #endregion

                        }
                        catch (Exception ex)
                        {
                            #region 记录产品接口子表错误信息
                            InterfaceProductDetail data = _context.Set<InterfaceProductDetail>().AsQueryable().Where(u => u.ProductHeaderId.Equals(productHeader.Id) && u.LineCode.Equals(item.LineCode) && u.StepCode.Equals(item.StepCode)).SingleOrDefault();
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
                            Response.Message = (Response.Message == "操作成功" ? "" : Response.Message) + "\r\n" + "产品ID:" + productHeader.Id + "同步失败:" + ex.Message;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                #region 记录产品接口主表错误信息
                try
                {
                    InterfaceProductHeader data = _unitWork.FindSingle<InterfaceProductHeader>(u => u.Code.Equals(interfaceproduct.interfaceProductHeader.Code));
                    if (data != null)
                    {
                        data.ErrorMessage = ex.Message;
                        if (data.UpdateBy == null)
                        {
                            data.UpdateBy = "wms";
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

    }
}

