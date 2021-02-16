using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Infrastructure;
using Quartz;
using WebApp;
using WebRepository;

namespace WebMvc
{
    //(WMS主动拉取 ERP ==> WMS)

    /// <summary>
    /// 测试拉取库存信息
    /// </summary>
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class GetCurrentStockAction
    {
        private string ConnString { set; get; }
        IJobExecutionContext Context { set; get; }

        public GetCurrentStockAction(string _ConnString, IJobExecutionContext _Context)
        {
            ConnString = _ConnString;
            Context = _Context;
        }

        public void Execute(JobContainer jobContainer)
        {
            //string sql = "";

            DbHelp dbHelp = new DbHelp(ConnString);
            #region 执行任务语句
            try
            {
                ApiRequest apiRequest = new ApiRequest("ICS", true);
                Inventory filter = JsonHelper.Instance.Deserialize<Inventory>(jobContainer.MethodParams);
                ICSResponse<List<Inventory>> _ICSResponse = apiRequest.Post<ICSResponse<List<Inventory>>>(JsonHelper.Instance.Serialize(filter), "IInventory/GetCurrentStock", "获取库存");

                if (_ICSResponse.Result != null)
                {
                    foreach (var item in _ICSResponse.Result)
                    {
                        //sql = string.Format("DELETE FROM [dbo].[sys_oper_log] WHERE createTime < '{0}';", DateTime.Now.AddDays(-1 * double.Parse(Days.ToString())));
                        //dbHelp.DataOperator(sql);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }
    }

    /// <summary>
    /// ICS客户档案添加接口
    /// </summary>
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class GetICSCustomerAction
    {
        private string ConnString { set; get; }
        IJobExecutionContext Context { set; get; }

        public GetICSCustomerAction(string _ConnString, IJobExecutionContext _Context)
        {
            ConnString = _ConnString;
            Context = _Context;
        }

    }

    /// <summary>
    /// ICS物料添加接口
    /// </summary>
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class GetICSInventoryAction
    {
        private string ConnString { set; get; }
        IJobExecutionContext Context { set; get; }

        public GetICSInventoryAction(string _ConnString, IJobExecutionContext _Context)
        {
            ConnString = _ConnString;
            Context = _Context;
        }

        public void Execute(JobContainer jobContainer)
        {
            //string sql = "";

            DbHelp dbHelp = new DbHelp(ConnString);
            #region 执行任务语句
            try
            {
                ApiRequest apiRequest = new ApiRequest("ICS", true);
                Inventory filter = JsonHelper.Instance.Deserialize<Inventory>(jobContainer.MethodParams);
                ICSResponse<List<Inventory>> _ICSResponse = apiRequest.Post<ICSResponse<List<Inventory>>>(JsonHelper.Instance.Serialize(filter), "GetICSInventory", "获取库存");

                if (_ICSResponse.Result != null)
                {
                    foreach (var item in _ICSResponse.Result)
                    {
                        //sql = string.Format("DELETE FROM [dbo].[sys_oper_log] WHERE createTime < '{0}';", DateTime.Now.AddDays(-1 * double.Parse(Days.ToString())));
                        //dbHelp.DataOperator(sql);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }
    }

    /// <summary>
    /// ICS供应商档案添加接口
    /// </summary>
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class GetICSVendorAction
    {
        private string ConnString { set; get; }
        IJobExecutionContext Context { set; get; }

        public GetICSVendorAction(string _ConnString, IJobExecutionContext _Context)
        {
            ConnString = _ConnString;
            Context = _Context;
        }

    }
}
