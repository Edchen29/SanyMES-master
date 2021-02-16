// ***********************************************************************
// <summary>json序列化帮助类</summary>
// ***********************************************************************

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure
{
    public class JsonHelper
    {
        private static JsonHelper _jsonHelper = new JsonHelper();
        public static JsonHelper Instance { get { return _jsonHelper; } }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        }

        public string SerializeByConverter(object obj, params JsonConverter[] converters)
        {
            return JsonConvert.SerializeObject(obj, converters);
        }

        public T Deserialize<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input);
        }

        public T DeserializeByConverter<T>(string input,params JsonConverter[] converter)
        {
            return JsonConvert.DeserializeObject<T>(input, converter);
        }

        public T DeserializeBySetting<T>(string input, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(input, settings);
        }

        private object NullToEmpty(object obj)
        {
            return null;
        }

        public static string GetDataTableForJson(DataTable dt)
        {
            StringBuilder json = new StringBuilder();
            if (dt == null || dt.Rows.Count == 0) return json.Append("]}").ToString();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                json.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    json.Append(string.Format("\"{0}\":\"{1}\",", dt.Columns[j].ColumnName, dt.Rows[i][j].ToString()));
                }
                json.Remove(json.ToString().LastIndexOf(","), 1);
                json.Append("}");
                json.Append(",");
            }
            json.Remove(json.ToString().LastIndexOf(","), 1);
            return json.ToString();

            //StringBuilder json = new StringBuilder();
            //json.Append("{\"count\":" + dt.Rows.Count.ToString());
            //json.Append(",");
            //json.Append("\"data\":[");
            //if (dt == null || dt.Rows.Count == 0) return json.Append("]}").ToString();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    json.Append("{");
            //    for (int j = 0; j < dt.Columns.Count; j++)
            //    {
            //        json.Append(string.Format("\"{0}\":\"{1}\",", dt.Columns[j].ColumnName, dt.Rows[i][j].ToString()));
            //    }
            //    json.Remove(json.ToString().LastIndexOf(","), 1);
            //    json.Append("}");
            //    json.Append(",");
            //}
            //json.Remove(json.ToString().LastIndexOf(","), 1);

            //json.Append("]}");
            //return json.ToString();

        }

        public static DataTable JsonConvertToDataTable(string strJson)
        {
            //转换json格式  
            string splitChar0 = "*";
            string splitChar0_1 = "splitChar0";
            string splitChar1 = "#";
            string splitChar1_1 = "splitChar1";
            strJson = strJson.Replace(splitChar0, splitChar0_1).Replace(splitChar1, splitChar1_1);

            strJson = strJson.Replace(",\"", "*\"").Replace("\":", "\"#").ToString();
            //取出表名     
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value.Replace(splitChar0_1, splitChar0).Replace(splitChar1_1, splitChar1);
            DataTable tb = null;
            //去除表名     
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.IndexOf("]"));
            //获取数据     
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split("*");

                //创建表     
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        var dc = new DataColumn();
                        if (str.Contains("#"))
                        {
                            string[] strCell = str.Split("#");

                            if (strCell[0].Substring(0, 1) == "\"")
                            {
                                int a = strCell[0].Length;

                                if (a > 2)
                                {
                                    dc.ColumnName = strCell[0].Substring(1, a - 2).Replace(splitChar0_1, splitChar0).Replace(splitChar1_1, splitChar1);
                                }

                            }
                            else
                            {
                                dc.ColumnName = strCell[0].Replace(splitChar0_1, splitChar0).Replace(splitChar1_1, splitChar1);
                            }
                            tb.Columns.Add(dc);
                        }

                    }
                    tb.AcceptChanges();
                }

                //增加内容     
                DataRow dr = tb.NewRow();
                int k = 0;
                for (int r = 0; r < strRows.Length; r++)
                {
                    if (strRows[r].Contains("#"))
                    {
                        string svalue = strRows[r].Split("#")[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "").Replace(splitChar0_1, splitChar0).Replace(splitChar1_1, splitChar1);
                        if (svalue != "null")
                        {
                            dr[k] = svalue;
                        }
                    }
                    else
                    {
                        k = r - 1;
                    }

                    k++;

                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }

            return tb;
        }

        /// <summary>
        /// 将对象序列化为json格式
        /// </summary>
        /// <param name="obj">序列化对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObjct(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="json">JSON字符串</param>
        /// <returns></returns>
        public static T JsonConvertObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object obj = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = obj as T;
            return t;
        }
        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object obj = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = obj as List<T>;
            return list;
        }
        /// <summary>
        /// 将JSON转数组
        /// 用法:jsonArr[0]["xxxx"]
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static JArray GetToJsonList(string json)
        {
            JArray jsonArr = (JArray)JsonConvert.DeserializeObject(json);
            return jsonArr;
        }
        /// <summary>
        /// 将DataTable转换成实体类
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static List<T> DtConvertToModel<T>(DataTable dt) where T : new()
        {
            List<T> ts = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                foreach (PropertyInfo pi in t.GetType().GetProperties())
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        if (!pi.CanWrite) continue;
                        var value = dr[pi.Name];
                        if (value != DBNull.Value)
                        {
                            switch (pi.PropertyType.FullName)
                            {
                                case "System.Decimal":
                                    pi.SetValue(t, decimal.Parse(value.ToString()), null);
                                    break;
                                case "System.String":
                                    pi.SetValue(t, value.ToString(), null);
                                    break;
                                case "System.Int32":
                                    pi.SetValue(t, int.Parse(value.ToString()), null);
                                    break;
                                default:
                                    pi.SetValue(t, value, null);
                                    break;
                            }
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

    }
}