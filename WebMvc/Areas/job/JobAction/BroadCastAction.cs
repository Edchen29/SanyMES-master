using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Infrastructure;
using Newtonsoft.Json.Linq;
using Quartz;
using WebApp;
using WebRepository;

namespace WebMvc
{
    /// <summary>
    /// AGV
    /// </summary>
    public class AGVAction
    {
        private string ConnString { set; get; }
        IJobExecutionContext Context { set; get; }

        private static int I_x = 0;
        private static int I_flag = 1;
        private static int I_maxx = 100;

        public AGVAction(string _ConnString, IJobExecutionContext _Context)
        {
            ConnString = _ConnString;
            Context = _Context;
        }

        public void Execute(JobContainer jobContainer)
        {
            //初始化广播
            JobContainer.InitBroadCaster();

            I_x += I_flag;
            if (I_x >= I_maxx)
            {
                I_flag = -1;
            }
            if (I_x <= 0)
            {
                I_flag = 1;
            }

            HslCommunication.OperateResult<JObject> read = null;
            read = HslCommunication.OperateResult.CreateSuccessResult(new JObject()
                        {
                            {"X",new JValue(I_x) },
                        });
            JobContainer.pushServer.PushString("AGV", read.Content.ToString());
        }
    }
}
