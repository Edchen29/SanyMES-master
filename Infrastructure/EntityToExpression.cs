using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure
{
    public static class EntityToExpression<T>
    {
        public static Expression<Func<T, bool>> GetExpressions(T t)
        {
            ParameterExpression u = Expression.Parameter(typeof(T), "u");
            Filter filterObj = new Filter();
            filterObj.Key = "";
            filterObj.Value = "";

            // 获得此模型的类型   
            Type type = typeof(T);
            Expression result = Expression.Constant(true);
            // 获得此模型的公共属性      
            PropertyInfo[] propertys = t.GetType().GetProperties();

            for (int i = 0; i < propertys.Length; i++)
            {
                Object value2 = propertys[i].GetValue(t, null);
                if (value2 != null && value2.ToString() != "")
                {
                    filterObj.Key = propertys[i].Name.ToString();

                    if (propertys[i].PropertyType == typeof(DateTime) || propertys[i].PropertyType == typeof(DateTime?))
                    {
                        filterObj.Value = ((DateTime)propertys[i].GetValue(t, null)).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    }
                    else
                    {
                        filterObj.Value = propertys[i].GetValue(t, null).ToString();
                    }

                    if (
                        (propertys[i].PropertyType == typeof(int) || propertys[i].PropertyType == typeof(int?))
                        || (propertys[i].PropertyType == typeof(short) || propertys[i].PropertyType == typeof(short?))
                        || (propertys[i].PropertyType == typeof(long) || propertys[i].PropertyType == typeof(long?))
                        || (propertys[i].PropertyType == typeof(decimal) || propertys[i].PropertyType == typeof(decimal?))
                        || (propertys[i].PropertyType == typeof(double) || propertys[i].PropertyType == typeof(double?))
                        || (propertys[i].PropertyType == typeof(bool) || propertys[i].PropertyType == typeof(bool?))
                    )
                    {
                        filterObj.Contrast = ConvertOperString("eq");
                        filterObj.JqContrast = "eq";

                        if (propertys[i].PropertyType == typeof(int?) ||
                            propertys[i].PropertyType == typeof(short?) ||
                            propertys[i].PropertyType == typeof(long?) ||
                            propertys[i].PropertyType == typeof(double?) ||
                            propertys[i].PropertyType == typeof(bool?) 
                        )
                        {
                            PropertyInfo property = typeof(T).GetProperty(filterObj.Key);
                            Expression left = Expression.Property(u, property);
                            try
                            {
                                result = result.AndAlso(Expression.Property(left, "HasValue"));
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                    else
                    {
                        filterObj.Contrast = ConvertOperString("cn");
                        filterObj.JqContrast = "cn";
                    }

                    result = result.AndAlso(u.GenerateBody<T>(filterObj));
                }
            }
            Expression<Func<T, bool>> expression = u.GenerateTypeLambda<T>(result);
            return expression;
        }

        private static string ConvertOperString(string _searchOper)
        {
            string sReturn = _searchOper;
            switch (_searchOper)
            {
                case "eq"://等于
                    sReturn = "==";
                    break;
                case "ne"://不等
                    sReturn = "!=";
                    break;
                case "lt"://小于
                    sReturn = "<";
                    break;
                case "le"://小于等于
                    sReturn = "<=";
                    break;
                case "gt"://大于
                    sReturn = ">";
                    break;
                case "ge"://大于等于
                    sReturn = ">=";
                    break;

                case "bw"://开始于
                    sReturn = "begin with";
                    break;
                case "bn"://不开始于
                    sReturn = "not begin with";
                    break;

                case "in"://属于
                    sReturn = "in";
                    break;
                case "ni"://不属于
                    sReturn = "not in";
                    break;

                case "ew"://结束于
                    sReturn = "end with";
                    break;
                case "en"://不结束于
                    sReturn = "not end with";
                    break;
                case "cn"://包含
                    sReturn = "like";
                    break;
                case "nc"://不包含
                    sReturn = "not like";
                    break;
                case "nu"://为空
                    sReturn = "null";
                    break;
                case "nn"://不为空
                    sReturn = "not null";
                    break;
            }

            return sReturn;
        }
    }

}
