using HslCommunication.Enthernet;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using WebApp;

namespace WebMvc
{
    public class ChartHub : Hub
    {
        private static Broadcaster _broadcaster;
        public ChartHub(IHubContext<ChartHub> hubContext, IOptions<AppSetting> appConfiguration)
        {
            if (_broadcaster == null)
            {
                _broadcaster = new Broadcaster(hubContext, appConfiguration.Value.NetPushServer, appConfiguration.Value.NetPushPort);
            }
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }

    /// <summary>
    /// 数据广播器
    /// </summary>
    public class Broadcaster
    {
        private readonly IHubContext<ChartHub> _hubContext;
        private readonly NetPushClient pushClient;

        public Broadcaster(IHubContext<ChartHub> hubContext, string netPushServer, int netPushPort)
        {
            // 获取所有连接的句柄，方便后面进行消息广播
            _hubContext = hubContext;

            // 实例化一个数据
            pushClient = new NetPushClient(netPushServer, netPushPort, "STK");
            pushClient.CreatePush(StkNetPushCallBack);

            // 实例化一个数据
            pushClient = new NetPushClient(netPushServer, netPushPort, "AGV");
            pushClient.CreatePush(AgvNetPushCallBack);

        }

        private void StkNetPushCallBack(NetPushClient pushClient, string str)
        {
            JObject json = JObject.Parse(str);

            _hubContext.Clients.All.SendAsync("sendStkData", json);
        }

        private void AgvNetPushCallBack(NetPushClient pushClient, string str)
        {
            JObject json = JObject.Parse(str);

            _hubContext.Clients.All.SendAsync("sendAgvData", json);
        }
    }
}