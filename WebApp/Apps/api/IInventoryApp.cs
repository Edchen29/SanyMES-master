using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Infrastructure;
using WebRepository;

namespace WebApp
{
    /// <summary>
    /// 库存查询接口App
    /// </summary>

    public partial class IInventoryApp : ApiApp
    {

        public IInventoryApp(IUnitWork unitWork, IAuth auth, BaseDBContext context) : base(unitWork, auth, context)
        {

        }

        public Response GetCurrentStock(InventoryModel inventoryModel)
        {
            Response<List<InventoryModel>> Response = new Response<List<InventoryModel>>();

            if (!CheckLogin())
            {
                Response.Code = 500;
                Response.Status = false;
                Response.Message = "请先登录！";
                return Response;
            }

            try
            {
                Inventory inventory = new Inventory
                {
                    WarehouseCode = inventoryModel.WarehouseType,
                    LocationCode = inventoryModel.LocationCode,
                    ContainerCode = inventoryModel.ContainerCode,
                    MaterialCode = inventoryModel.MaterialCode,
                    Lot = inventoryModel.Lot,
                    Status = inventoryModel.Status,
                    Qty = null,
                };

                Response.Result = _unitWork.Find(EntityToExpression<Inventory>.GetExpressions(inventory)).MapToList<InventoryModel>();
            }
            catch (Exception ex)
            {
                Response.Code = 500;
                Response.Status = false;
                Response.Message = ex.Message;
            }

            return Response;
        }
        public Response GetDefectCodeApp(DefectCodeModel defectCodeModel)
        {
            Response<List<DefectCodeModel>> Response = new Response<List<DefectCodeModel>>();

            //if (!CheckLogin())
            //{
            //    Response.Code = 500;
            //    Response.Status = false;
            //    Response.Message = "请先登录！";
            //    return Response;
            //}

            try
            {
                DefectCode defectCode = new DefectCode
                {
                    Code = defectCodeModel.Code,
                    Name = defectCodeModel.Name,
                };

                Response.Result = _unitWork.Find(EntityToExpression<DefectCode>.GetExpressions(defectCode)).MapToList<DefectCodeModel>();
            }
            catch (Exception ex)
            {
                Response.Code = 500;
                Response.Status = false;
                Response.Message = ex.Message;
            }

            return Response;
        }
        public PadResponse GetCheckCountApp(Repair repair)
        {
            PadResponse Response = new PadResponse();
            try
            {
                Repair rep = new Repair
                {
                    NGMarkUser = repair.NGMarkUser,
                };
                var pcount = from it in _unitWork.Find(EntityToExpression<Repair>.GetExpressions(rep))
                             group it by it.NGMarkUser into g
                             select new
                             {
                                 name = g.Key,
                                 value = g.Count(),
                             };
                Response.Result = pcount.ToList();
            }
            catch (Exception ex)
            {
                Response.Code = 500;
                Response.Status = false;
                Response.Message = ex.Message;
            }

            return Response;
        }
    }
}

