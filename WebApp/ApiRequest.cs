using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using Infrastructure;
using WebRepository;

namespace WebApp
{
    /// <summary>
    /// Api请求
    /// </summary>
    public class ApiRequest
    {
        private string ConnString;
        private string Server;
        private string LoginUrl;
        private string Uid;
        private string Pwd;
        private bool _LoginFlag = false;
        private string _System;

        private Stopwatch Stopwatch { get; set; }

        private string _method;
        private string _requestApi;
        private string _apiGroup;
        private string _request;
        private string _response;

        public ApiRequest(string System, bool LoginFlag = false)
        {
            Stopwatch = new Stopwatch();
            Stopwatch.Start();

            _System = System;
            _LoginFlag = LoginFlag;
            try
            {
                var config = AppSettingsJson.GetAppSettings();
                ConnString = config.GetSection("ConnectionStrings:BaseDBContext").Value;

                SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(ConnString);
                sqlConnectionStringBuilder.Password = Encryption.Decrypt(sqlConnectionStringBuilder.Password);
                ConnString = sqlConnectionStringBuilder.ConnectionString;

                Server = config.GetSection(System + "Setting:Server").Value;
                LoginUrl = config.GetSection(System + "Setting:LoginUrl").Value;
                Uid = config.GetSection(System + "Setting:Uid").Value;
                Pwd = config.GetSection(System + "Setting:Pwd").Value;
            }
            catch (Exception) { }
        }

        public T Post<T>(string parameter, string requestApi, string apiGroup = "") where T : IResponse
        {
            T result = default(T);

            _method = "POST";
            _requestApi = requestApi;
            _apiGroup = apiGroup;
            _request = parameter;

            try
            {
                HttpHelper httpHelper = new HttpHelper(Server);
                if (_LoginFlag)
                {
                    LoginEntity loginEntity = new LoginEntity { username = Uid, password = Pwd };
                    _response = FormatResponse(httpHelper.Post(loginEntity, LoginUrl));
                    result = JsonHelper.Instance.Deserialize<T>(_response);
                    if (result.Code != 200)
                    {
                        throw new Exception(result.Message);
                    }
                }
                _response = FormatResponse(httpHelper.Post(parameter, requestApi));

                result = JsonHelper.Instance.Deserialize<T>(_response);
                Log(true);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
                Log(false);
            }

            return result;
        }

        public T Get<T>(Dictionary<string, string> parameter, string requestApi, string apiGroup = "") where T : IResponse
        {
            T result = default(T);

            _method = "GET";
            _requestApi = requestApi;
            _apiGroup = apiGroup;
            _request = JsonHelper.Instance.Serialize(parameter);

            try
            {
                HttpHelper httpHelper = new HttpHelper(Server);
                if (_LoginFlag)
                {
                    LoginEntity loginEntity = new LoginEntity { username = Uid, password = Pwd };
                    _response = FormatResponse(httpHelper.Post(loginEntity, LoginUrl));
                    result = JsonHelper.Instance.Deserialize<T>(_response);
                    if (result.Code != 200)
                    {
                        throw new Exception(result.Message);
                    }
                }

                _response = FormatResponse(httpHelper.Get(parameter, requestApi));
                result = JsonHelper.Instance.Deserialize<T>(_response);
                Log(true);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
                Log(false);
            }

            return result;
        }

        private void Log(bool bresult)
        {
            Stopwatch.Stop();

            try
            {
                if (string.IsNullOrEmpty(ConnString))
                {
                    return;
                }

                string type = "发送";
                string system = _System;
                string method = _method;
                string server = Server;
                string path = _requestApi;
                string[] paths = _requestApi.Split('/');
                paths = paths[paths.Length - 1].Split('?');
                string actionName = paths[0];
                string queryString = paths.Length > 1 ? paths[1] : "";
                string apiGroup = _apiGroup;
                string request = _request;
                string response = _response;
                double totalMilliseconds = Stopwatch.Elapsed.TotalMilliseconds;
                string logTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                string name = "MES";
                string ip = "";
                string browser = "";
                string result = bresult ? "成功" : "失败";
                int flag = 1;
                string createTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                string createBy = "MES";

                DbHelp dbHelp = new DbHelp(ConnString);
                string sql = string.Format(@"INSERT INTO [dbo].[sys_interface_log]
                                        ([type]
                                        ,[system]
                                        ,[method]
                                        ,[server]
                                        ,[path]
                                        ,[actionName]
                                        ,[queryString]
                                        ,[apiGroup]
                                        ,[request]
                                        ,[response]
                                        ,[totalMilliseconds]
                                        ,[logTime]
                                        ,[name]
                                        ,[ip]
                                        ,[browser]
                                        ,[result]
                                        ,[flag]
                                        ,[createTime]
                                        ,[createBy])
                                    VALUES
                                        ('{0}'
                                        ,'{1}'
                                        ,'{2}'
                                        ,'{3}'
                                        ,'{4}'
                                        ,'{5}'
                                        ,'{6}'
                                        ,'{7}'
                                        ,'{8}'
                                        ,'{9}'
                                        ,'{10}'
                                        ,'{11}'
                                        ,'{12}'
                                        ,'{13}'
                                        ,'{14}'
                                        ,'{15}'
                                        ,'{16}'
                                        ,'{17}'
                                        ,'{18}')",
                                        type
                                        ,system
                                        ,method
                                        ,server
                                        ,path
                                        ,actionName
                                        ,queryString
                                        ,apiGroup
                                        ,request
                                        ,response
                                        ,totalMilliseconds
                                        ,logTime
                                        ,name
                                        ,ip
                                        ,browser
                                        ,result
                                        ,flag
                                        ,createTime
                                        ,createBy
                                        );
                dbHelp.DataOperator(sql);
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        private string FormatResponse(string response)
        {
            response = response.Replace("\r", "").Replace("\n", "");
            response = response.Replace("\"\\\"{", "{").Replace("}\\\"\"", "}").Replace("\\\\\\", "");
            response = response.Replace("\"{", "{").Replace("}\"", "}").Replace("\\\"", "\"");

            return response;
        }
    }
}
