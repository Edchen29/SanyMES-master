using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebRepository;

namespace WebApp
{
    /// <summary>
	/// 在线用户记录
	/// </summary>
    
    public partial class LocationMonitorApp
    {
        private IUnitWork _unitWork;

        public LocationMonitorApp(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }

        public TableData GetRoadWays()
        {
            var result = new TableData();
            try
            {
                var data = _unitWork.Find<Location>(u => u.Code.Contains("-01-01-01")).Select(u => u.RoadWay);

                result.data = data;
                result.count = data.Count();
            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
            }

            return result;
        }

        public TableData GetRoadWayConfig(int roadway)
        {
            var result = new TableData();
            try
            {
                var maxRow = _unitWork.Find<Location>(u => u.RoadWay == roadway).OrderByDescending(u => u.Row).First().Row;
                var maxLine =_unitWork.Find<Location>(u => u.RoadWay == roadway).OrderByDescending(u=>u.Column).First().Column;
                var maxLayer = _unitWork.Find<Location>(u => u.RoadWay == roadway).OrderByDescending(u => u.Layer).First().Layer;
                var type = _unitWork.Find<Location>(u => u.RoadWay == roadway).OrderBy(u => u.Id).First().Type;

                JObject jObject = new JObject();
                jObject.Add("MaxRow", maxRow);
                jObject.Add("MaxLine", maxLine);
                jObject.Add("MaxLayer", maxLayer);
                jObject.Add("Type", type);

                result.data = jObject;
                result.count = 1;
            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
            }

            return result;
        }

        public TableData GetLocations(int roadway)
        {
            var result = new TableData();
            try
            {
                var data = _unitWork.Find<Location>(u => u.RoadWay == roadway).ToList();
                result.data = data;
                result.count = data.Count;
            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
            }

            return result;
        }
    }
}

