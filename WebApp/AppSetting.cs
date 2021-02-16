namespace WebApp
{
    /// <summary>
    /// 配置项
    /// </summary>
    public class AppSetting
    {
        public AppSetting()
        {
        }

        /// <summary>
        /// 广播服务器IP
        /// </summary>
        public string NetPushServer { get; set; }

        /// <summary>
        /// 广播服务器IP
        /// </summary>
        public int NetPushPort { get; set; }
    }
}
