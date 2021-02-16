using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// http请求类
    /// </summary>
    public class HttpHelper
    {
        private HttpClient _httpClient;
        private string _baseIPAddress;
        private ICacheContext _cacheContext;
        public string usertoken;

        public HttpHelper(string ipaddress = "")
        {
            this._baseIPAddress = ipaddress;
            _httpClient = new HttpClient { BaseAddress = new Uri(_baseIPAddress) };
        }

        /// <summary>
        /// 创建带用户信息的请求客户端
        /// </summary>
        /// <param name="userName">用户账号</param>
        /// <param name="pwd">用户密码，当WebApi端不要求密码验证时，可传空串</param>
        /// <param name="uriString">The URI string.</param>
        public HttpHelper(string userName, string pwd = "", string uriString = "")
            : this(uriString)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                _httpClient.DefaultRequestHeaders.Authorization = CreateBasicCredentials(userName, pwd);
            }
        }

        /// <summary>
        /// Get请求数据
        ///   /// <para>最终以url参数的方式提交</para>
        /// <para>重构与post同样异步调用</para>
        /// </summary>
        /// <param name="parameters">参数字典,可为空</param>
        /// <param name="requestUri">例如/api/Files/UploadFile</param>
        /// <returns></returns>
        public string Get(Dictionary<string, string> parameters, string requestUri)
        {
            if (parameters != null)
            {
                var strParam = string.Join("&", parameters.Select(o => o.Key + "=" + o.Value));
                requestUri = string.Concat(ConcatURL(requestUri), '?', strParam);
            }
            else
            {
                requestUri = ConcatURL(requestUri);
            }

            var result = _httpClient.GetStringAsync(requestUri);
            return result.Result;
        }

        /// <summary>
        /// Get请求数据
        /// <para>最终以url参数的方式提交</para>
        /// </summary>
        /// <param name="parameters">参数字典</param>
        /// <param name="requestUri">例如/api/Files/UploadFile</param>
        /// <returns>实体对象</returns>
        public T Get<T>(Dictionary<string, string> parameters, string requestUri) where T : class
        {
            string jsonString = Get(parameters, requestUri);
            if (string.IsNullOrEmpty(jsonString))
                return null;

            return JsonHelper.Instance.Deserialize<T>(jsonString);
        }

        /// <summary>
        /// 以json的方式Post数据 返回string类型
        /// <para>最终以json的方式放置在http体中</para>
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="requestUri">例如/api/Files/UploadFile</param>
        /// <returns></returns>
        public string Post(object entity, string requestUri)
        {
            string request = string.Empty;
            if (entity != null)
                request = JsonHelper.Instance.Serialize(entity);
            HttpContent httpContent = new StringContent(request);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return Post(requestUri, httpContent);
        }

        /// <summary>
        /// 提交字典类型的数据
        /// <para>最终以formurlencode的方式放置在http体中</para>
        /// </summary>
        /// <returns>System.String.</returns>
        public string PostDicObj(Dictionary<string, object> para, string requestUri)
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();
            foreach (var item in para)
            {
                if (item.Value != null)
                {
                    if (item.Value.GetType().Name.ToLower() != "string")
                    {
                        temp.Add(item.Key, JsonHelper.Instance.Serialize(item.Value));
                    }
                    else
                    {
                        temp.Add(item.Key, item.Value.ToString());
                    }
                }
                else
                {
                    temp.Add(item.Key, "");
                }
            }

            return PostDic(temp, requestUri);
        }

        /// <summary>
        /// Post Dic数据
        /// <para>最终以formurlencode的方式放置在http体中</para>
        /// </summary>
        /// <returns>System.String.</returns>
        public string PostDic(Dictionary<string, string> temp, string requestUri)
        {
            HttpContent httpContent = new FormUrlEncodedContent(temp);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            return Post(requestUri, httpContent);
        }

        public string PostByte(byte[] bytes, string requestUrl)
        {
            HttpContent content = new ByteArrayContent(bytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return Post(requestUrl, content);
        }

        private string Post(string requestUrl, HttpContent content)
        {
            var result = _httpClient.PostAsync(ConcatURL(requestUrl), content);
            return result.Result.Content.ReadAsStringAsync().Result;
        }

        private AuthenticationHeaderValue CreateBasicCredentials(string userName, string password)
        {
            string toEncode = userName + ":" + password;
            // The current HTTP specification says characters here are ISO-8859-1.
            // However, the draft specification for the next version of HTTP indicates this encoding is infrequently
            // used in practice and defines behavior only for ASCII.
            Encoding encoding = Encoding.GetEncoding("utf-8");
            byte[] toBase64 = encoding.GetBytes(toEncode);
            string parameter = Convert.ToBase64String(toBase64);

            return new AuthenticationHeaderValue("Basic", parameter);
        }

        /// <summary>
        /// 以json的方式Post数据 返回string类型
        /// <para>最终以json的方式放置在http体中</para>
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="requestUri">例如/api/Files/UploadFile</param>
        /// <returns></returns>
        /// 
        public string Post(string parameter, string requestUri)
        {
            HttpContent httpContent = new StringContent(parameter);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return Post(requestUri, httpContent);
        }

        /// <summary>
        /// 把请求的URL相对路径组合成绝对路径
        /// </summary>
        private string ConcatURL(string requestUrl)
        {
            return new Uri(_httpClient.BaseAddress, requestUrl).OriginalString;
        }

        public HttpHelper(string ipaddress, ICacheContext cacheContext)
        {
            this._baseIPAddress = ipaddress;
            _httpClient = new HttpClient { BaseAddress = new Uri(_baseIPAddress) };
            _cacheContext = cacheContext;
        }
        //接口需要权限认证的调用，使用账号密码或对接方系统分配的Token\sessionid\jsessionid等，配合HttpPostLogin使用，先登录获取Token\sessionid\jsessionid
        public string HttpPost(string url, string postData, CookieContainer cookies)
        {

            //定义request并设置request的路径
            // WebRequest request = WebRequest.Create(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (cookies.Count == 0)
            {
                request.CookieContainer = new CookieContainer();
                cookies = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cookies;
            }
            //  request.CookieContainer = cookies;
            request.Method = "POST";

            //初始化request参数
            // string postData = "{ ID: \"1\", NAME: \"Jim\", CREATETIME: \"1988-09-11\" }";

            //设置参数的编码格式，解决中文乱码
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //设置request的MIME类型及内容长度
            request.ContentType = "application/json";
            //  request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            request.ContentLength = byteArray.Length;

            //打开request字符流
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //定义response为前面的request响应
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
          //  WebResponse response = request.GetResponse();
            Console.WriteLine(response.Headers.Get("Set-Cookie"));
            //获取相应的状态代码
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            if (cookies.Count == 0)
            {
                response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);
                CookieCollection cook;
                cook = response.Cookies;
                cookies.Add(cook);
            }

            //定义response字符流
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();//读取所有
            Console.WriteLine(responseFromServer);
            return responseFromServer;
        }
        //接口登录POST，登录成功后把账号密码或Token\sessionid\jsessionid写入CookieContainer
        public string HttpPostLogin(string url, string postData, CookieContainer cookies)
        {

            //定义request并设置request的路径
            // WebRequest request = WebRequest.Create(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (cookies.Count==0)
            {
                request.CookieContainer = new CookieContainer();
                cookies = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cookies;
            }
            //  request.CookieContainer = cookies;
            request.Method = "POST";

            //初始化request参数
            // string postData = "{ ID: \"1\", NAME: \"Jim\", CREATETIME: \"1988-09-11\" }";

            //设置参数的编码格式，解决中文乱码
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //设置request的MIME类型及内容长度
            request.ContentType = "application/json";
            //  request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            request.ContentLength = byteArray.Length;
            //打开request字符流
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //定义response为前面的request响应
           // WebResponse response = request.GetResponse();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (cookies.Count == 0)
            {
                response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);
                CookieCollection cook;
                cook = response.Cookies;
                cookies.Add(cook);
            }
            //Console.WriteLine(response.Headers.Get("Set-Cookie"));
            //获取相应的状态代码
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            //定义response字符流
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();//读取所有
            //----系统有Token\sessionid\jsessionid认证，但是接口未提供Token的返回值，需要从回应头中自己解析取Token码的情况
            //JObject jresponse = JObject.Parse(responseFromServer);
            //if (jresponse["code"].ToString() == "200")//登录成功后
            //{
            //    //读取回应头的Token\sessionid\jsessionid
            //    return response.Headers.Get("Set-Cookie");
            //}
            //else
            //{
            //    return responseFromServer;
            //}
            //----end-----
            return responseFromServer;
        }
        //接口不需要权限验证的调用
        public string HttpPost(string url, string postData)
        {

            //定义request并设置request的路径
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";

            //初始化request参数
            // string postData = "{ ID: \"1\", NAME: \"Jim\", CREATETIME: \"1988-09-11\" }";

            //设置参数的编码格式，解决中文乱码
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //设置request的MIME类型及内容长度
            request.ContentType = "application/json";
            // request.ContentType = "text/xml; charset=utf-8"; 
            //  request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            request.ContentLength = byteArray.Length;

            //打开request字符流
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //定义response为前面的request响应
            WebResponse response = request.GetResponse();

            //获取相应的状态代码
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            //定义response字符流
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();//读取所有
            return responseFromServer;

        }

        /// <summary>
        /// 获取Cookie的值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="cc">Cookie集合对象</param>
        /// <returns>返回Cookie名称对应值</returns>
        public string GetCookie(string cookieName, CookieContainer cc)

        {

            List<Cookie> lstCookies = new List<Cookie>();

            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",

                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |

                System.Reflection.BindingFlags.Instance, null, cc, new object[] { });

            foreach (object pathList in table.Values)

            {

                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",

                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField

                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });

                foreach (CookieCollection colCookies in lstCookieCol.Values)

                    foreach (Cookie c1 in colCookies) lstCookies.Add(c1);

            }

            var model = lstCookies.Find(p => p.Name == cookieName);

            if (model != null)

            {

                return model.Value;

            }
            return string.Empty;
        }
    }
}