using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using WebRepository;

namespace WebApp
{
    /// <summary>
    /// 仓库地图
    /// </summary>

    public class EchartsDataApp
    {
        private IUnitWork _unitWork;
        TableData result = new TableData();
        DateTime dtnow = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");

        public EchartsDataApp(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }
        //public TableData GetWeekTask()
        //{
        //    DateTime dt = DateTime.Now.AddDays(-6).Date;
        //    dynamic sp = new ExpandoObject();
        //    var weektask = _unitWork.Find<InventoryTransaction>(a => a.CreateTime > dt).GroupBy(a =>a.Type).SelectMany(g => g.GroupBy(b => ((DateTime)b.CreateTime).ToString("yyyy-MM-dd"))).Select(g => (new { name = g.Key, count = g.Count(),type=g.AsQueryable().Select(u=>u.Type).First() })).OrderBy(a => a.name);
        //    result.data = weektask.ToList();
        //    return result;
        //}
        public TableData GetProduct()
        {
            var lct = _unitWork.Find<ProductHeader>(null).GroupBy(a => a.MachineType).SelectMany(g => g.GroupBy(b => b.WorkShop)).Select(g => (new { name = g.Key, count = g.Count(), type = g.AsQueryable().Select(u => u.MachineType).First() })).OrderBy(a => a.name);
            result.data = lct.ToList();
            return result;
        }
        public TableData GetStation()
        {
            var lct = _unitWork.Find<StepTraceLog>(a => a.CreateTime > dtnow).GroupBy(a => a.StationId).SelectMany(g => g.GroupBy(b => b.LineId)).Select(g => (new { name = g.Key, count = g.Count(), type = g.AsQueryable().Select(u => u.StationId).First() })).OrderBy(a => a.name);
            result.data = lct.ToList();
            return result;
        }
        public TableData GetDistribution()
        {
            var lct = _unitWork.Find<MaterialCallHeader>(a => a.Status != CallStatus.完成).GroupBy(a => a.LocationCode).SelectMany(g => g.GroupBy(b => b.NeedStation)).Select(g => (new { name = g.Key, count = g.Count(), type = g.AsQueryable().Select(u => u.LocationCode).First() })).OrderBy(a => a.name);
            result.data = lct.ToList();
            return result;
        }
        public TableData GetOutPut()
        {
            dynamic sp = new ExpandoObject();
            List<dynamic> edata = new List<dynamic>();
            var wocount = from wo in _unitWork.Find<OrderHeader>(u => u.CreateTime > dtnow || u.ActualEndTime > dtnow || u.Status!=OrderStatus.完成)
                          group wo by wo.LineId into g
                          select new
                          {
                              value = g.Sum(p => p.PlanQty),
                              name = g.Key,
                              type = "W"
                          };
            var pcount = from it in _unitWork.Find<StepTraceHistory>(a => a.CreateTime > dtnow)
                         group it by it.LineId into g
                         select new
                         {
                             value = g.Count(),
                             name = g.Key,
                             type = "P",
            };
            edata.Add(wocount);
            edata.Add(pcount);
            result.data = edata;
            return result;
        }
        //public TableData GetWaitRepair()
        //{
        //    dynamic sp = new ExpandoObject();//一个动态的类，省得要去定义Model，.NET4.0以上支持
        //    List<dynamic> edata = new List<dynamic>();
        //    int woount = _unitWork.Find<Repair>(a => a.Status == "reparit").GroupBy(n=>n.ItemCode).Count();
        //    int reparitcount = _unitWork.Find<Repair>(a => a.Status == "reparit").Count();
        //    sp.wo = woount;
        //    sp.repair = reparitcount;
        //    edata.Add(sp);
        //    result.data = edata;
        //    return result;
        //}


        //public TableData GetMaterielByGroup()
        //{
        //    var mcdata = from it in _unitWork.Find <Inventory>(null)
        //               group it by it.MaterialCode into g
        //               select new
        //               {
        //                   value = g.Sum(p => decimal.Parse(p.Qty.ToString())),
        //                   name = g.Key,
        //               };

        //    result.data = mcdata.ToList().OrderByDescending(u=>u.value);
        //    return result;
        //}

    }
}
