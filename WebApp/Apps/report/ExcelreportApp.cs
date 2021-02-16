using Newtonsoft.Json.Linq;
using System;
using WebRepository;

namespace WebApp
{
    /// <summary>
    /// 
    /// </summary>

    public class ExcelReportApp
    {
        private ISqlWork _sqlWork;
        private IUnitWork _unitWork;

        public ExcelReportApp(ISqlWork sqlWork, IUnitWork unitWork)
        {
            _sqlWork = sqlWork;
            _unitWork = unitWork;
        }

        public TableData QueryData(int ReportId, string Filter)
        {
            var result = new TableData();

            try
            {
                Excelreport excelreport = _unitWork.FindSingle<Excelreport>(u => u.Id == ReportId);
                var sql = excelreport.Sql;
                var sfilter = excelreport.Params;

                if (!string.IsNullOrEmpty(sfilter))
                {
                    if (string.IsNullOrEmpty(Filter))
                    {
                        throw new Exception("请填写查询条件！");
                    }
                    JObject json = null;
                    json = JObject.Parse(Filter);

                    JObject jsondefault = null;
                    jsondefault = JObject.Parse(sfilter);
                    foreach (var item in jsondefault)
                    {
                        var key = item.Key;
                        if (!json.ContainsKey(key))
                        {
                            throw new Exception("查询条件不正确！");
                        }
                    }

                    foreach (var item in json)
                    {
                        sql = sql.Replace(item.Key, item.Value.ToString());
                    }
                }
                var ds = _sqlWork.SelectGet(sql);
                result.data = ds.Tables[0];
                result.count = ds.Tables[0].Rows.Count;
                result.code = 200;
            }
            catch (Exception ex)
            {
                result.code = 500;
                result.msg = ex.Message;
            }
            return result;
        }
    }
}

