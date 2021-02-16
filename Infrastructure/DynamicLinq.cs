// ***********************************************************************
// <copyright file="DynamicLinq.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>动态linq</summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure
{
    public static class DynamicLinq
    {
        public static ParameterExpression CreateLambdaParam<T>(string name)
        {
            return Expression.Parameter(typeof(T), name);
        }

        /// <summary>
        /// 创建linq表达示的body部分
        /// </summary>
        public static Expression GenerateBody<T>(this ParameterExpression param, Filter filterObj)
        {
            PropertyInfo property = typeof(T).GetProperty(filterObj.Key);

            //组装左边
            Expression left = Expression.Property(param, property);
            //组装右边
            Expression right = null;

            if (
                (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                || (property.PropertyType == typeof(short) || property.PropertyType == typeof(short?))
                || (property.PropertyType == typeof(long) || property.PropertyType == typeof(long?))
                || (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                || (property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
            )
            {
                string stype = property.PropertyType.ToString().ToLower();
                Expression leftexp = property.PropertyType.ToString().IndexOf("Nullable") >= 0 ? Expression.Property(left, "Value") : left;
                object filterValue = null;
                if (string.IsNullOrEmpty(filterObj.Value))
                {
                    filterValue = 0;
                }
                else
                {
                    if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                    { filterValue = int.Parse(filterObj.Value); }
                    else if (property.PropertyType == typeof(short) || property.PropertyType == typeof(short?))
                    { filterValue = short.Parse(filterObj.Value); }
                    else if (property.PropertyType == typeof(long) || property.PropertyType == typeof(long?))
                    { filterValue = long.Parse(filterObj.Value); }
                    else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                    { filterValue = decimal.Parse(filterObj.Value); }
                    else if (property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
                    { filterValue = double.Parse(filterObj.Value); }
                    //if (stype.IndexOf("int") >= 0)
                    //{ filterValue = int.Parse(filterObj.Value); }
                    //else if (stype.IndexOf("short") >= 0)
                    //{ filterValue = short.Parse(filterObj.Value); }
                    //else if (stype.IndexOf("long") >= 0)
                    //{ filterValue = long.Parse(filterObj.Value); }
                    //else if (stype.IndexOf("decimal") >= 0)
                    //{ filterValue = decimal.Parse(filterObj.Value); }
                    //else if (stype.IndexOf("double") >= 0)
                    //{ filterValue = double.Parse(filterObj.Value); }
                }
                Type type = filterValue.GetType();
                
                left = Expression.Call(leftexp, type.GetMethod("CompareTo", new Type[] { type }),
                                Expression.Constant(filterValue));

                right = Expression.Constant(0);
            }
            else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
            {
                if (filterObj.Contrast == "like")
                {
                    Expression leftexp = property.PropertyType == typeof(DateTime) ? left : Expression.Property(left, "Value");
                    filterObj.Contrast = "==";
                    if (filterObj.Value.Length == 4)
                    {
                        left = Expression.Call(Expression.Property(leftexp, "Year"), typeof(int).GetMethod("Equals", new Type[] { typeof(int) }),
                                     Expression.Constant(int.Parse(filterObj.Value)));
                    }
                    else if (filterObj.Value.Length == 7)
                    {
                        DateTime date = Convert.ToDateTime(filterObj.Value);
                        left = Expression.Call(Expression.Property(leftexp, "Year"), typeof(int).GetMethod("Equals", new Type[] { typeof(int) }),
                                     Expression.Constant(date.Year)).And(Expression.Call(Expression.Property(leftexp, "Month"), typeof(int).GetMethod("Equals", new Type[] { typeof(int) }),
                                     Expression.Constant(date.Month)));
                    }
                    else
                    {
                        DateTime date = Convert.ToDateTime(filterObj.Value);
                        left = Expression.Call(Expression.Property(leftexp, "Year"), typeof(int).GetMethod("Equals", new Type[] { typeof(int) }),
                                     Expression.Constant(date.Year)).And(Expression.Call(Expression.Property(leftexp, "Month"), typeof(int).GetMethod("Equals", new Type[] { typeof(int) }),
                                     Expression.Constant(date.Month))).And(Expression.Call(Expression.Property(leftexp, "Day"), typeof(int).GetMethod("Equals", new Type[] { typeof(int) }),
                                     Expression.Constant(date.Day)));
                    }
                    right = Expression.Constant(true);
                }
                else
                {
                    Expression leftexp = property.PropertyType == typeof(DateTime) ? left : Expression.Property(left, "Value");
                    object filterValue = string.IsNullOrEmpty(filterObj.Value) ? DateTime.Now : DateTime.Parse(filterObj.Value);

                    left = Expression.Call(leftexp, typeof(DateTime).GetMethod("CompareTo", new Type[] { typeof(DateTime) }),
                                    Expression.Constant(filterValue));

                    right = Expression.Constant(0);
                }
            }
            else if (property.PropertyType == typeof(string))
            {
                filterObj.Value = filterObj.Value.ToUpper();
                left = Expression.Call(left, typeof(string).GetMethod("ToUpper", new Type[] { }));
                right = Expression.Constant((filterObj.Value));
            }
            else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
            {
                if (!string.IsNullOrEmpty(filterObj.Value))
                {
                    if (filterObj.Value.ToLower().Equals("true") || filterObj.Value.ToLower().Equals("1"))
                    {
                        filterObj.Value = "true";
                    }
                    else
                    {
                        filterObj.Value = "false";
                    }
                }

                Expression leftexp = property.PropertyType == typeof(bool) ? left : Expression.Property(left, "Value");
                object filterValue = string.IsNullOrEmpty(filterObj.Value) ? bool.Parse("true") : bool.Parse(filterObj.Value);

                left = Expression.Call(leftexp, typeof(bool).GetMethod("CompareTo", new Type[] { typeof(bool) }),
                             Expression.Constant(filterValue));

                right = Expression.Constant(0);
            }
            else if (property.PropertyType == typeof(Guid) || property.PropertyType == typeof(Guid?))
            {
                Expression leftexp = property.PropertyType == typeof(Guid) ? left : Expression.Property(left, "Value");
                object filterValue = string.IsNullOrEmpty(filterObj.Value) ? Guid.Parse("00000000-0000-0000-0000-000000000000") : Guid.Parse(filterObj.Value);

                left = Expression.Call(leftexp, typeof(Guid).GetMethod("CompareTo", new Type[] { typeof(Guid) }),
                             Expression.Constant(filterValue));

                right = Expression.Constant(0);
            }
            else
            {
                throw new Exception("暂不能解析该Key的类型");
            }

            CheckFilterString(filterObj.JqContrast, property.PropertyType);

            //c.XXX=="XXX"
            Expression filter = Expression.Equal(left, right);//等于
            switch (filterObj.Contrast)
            {
                case "<="://小于等于
                    filter = Expression.LessThanOrEqual(left, right);
                    break;

                case "<"://小于
                    filter = Expression.LessThan(left, right);
                    break;

                case ">"://大于
                    filter = Expression.GreaterThan(left, right);
                    break;

                case ">="://大于等于
                    filter = Expression.GreaterThanOrEqual(left, right);
                    break;
                case "!="://不等于
                    filter = Expression.NotEqual(left, right);
                    break;

                case "like"://包含
                    filter = Expression.Call(left, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),
                                 Expression.Constant(filterObj.Value));
                    break;
                case "not like"://不包含
                    filter = Expression.Not(Expression.Call(left, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),
                                 Expression.Constant(filterObj.Value)));
                    break;
                case "in"://属于
                    var lExp = Expression.Constant(filterObj.Value.Split(',').ToList()); //数组
                    var methodInfo = typeof(List<string>).GetMethod("Contains", new Type[] { typeof(string) }); //Contains语句
                    filter = Expression.Call(lExp, methodInfo, left);
                    break;
                case "not in"://不属于
                    var listExpression = Expression.Constant(filterObj.Value.Split(',').ToList()); //数组
                    var method = typeof(List<string>).GetMethod("Contains", new Type[] { typeof(string) }); //Contains语句
                    filter = Expression.Not(Expression.Call(listExpression, method, left));
                    break;
                case "begin with"://开始于
                    filter = Expression.Call(left, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }),
                                 Expression.Constant(filterObj.Value));
                    break;
                case "not begin with"://不开始于
                    filter = Expression.Not(Expression.Call(left, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }),
                                 Expression.Constant(filterObj.Value)));
                    break;
                case "end with"://结束于
                    filter = Expression.Call(left, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }),
                                 Expression.Constant(filterObj.Value));
                    break;
                case "not end with"://不结束于
                    filter = Expression.Not(Expression.Call(left, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }),
                                 Expression.Constant(filterObj.Value)));
                    break;
                case "null"://为空
                    filter = Expression.Equal(left, Expression.Constant(string.Empty));
                    break;
                case "not null"://不为空
                    filter = Expression.NotEqual(left, Expression.Constant(string.Empty));
                    break;
            }

            return filter;
        }

        private static void CheckFilterString(string p, Type type)
        {
            bool valid = false;
            string errmsg = string.Format("{0}类型不支持{1}操作", type, p);
            string typeS = type.ToString();
            if (type == typeof(int) || type == typeof(int?) || type == typeof(decimal) || type == typeof(decimal?) || type == typeof(long) || type == typeof(long?) || type == typeof(short) || type == typeof(short?) || type == typeof(double) || type == typeof(double?) || type == typeof(DateTime) || type == typeof(DateTime?))
            {
                if (type == typeof(DateTime) || type == typeof(DateTime?))
                {
                    valid = ("eq,ne,lt,le,gt,ge,cn".Split(',').Contains(p));
                }
                else
                {
                    valid = ("eq,ne,lt,le,gt,ge".Split(',').Contains(p));
                }
            }
            else if (type == typeof(string))
            {
                valid = ("eq,ne,bw,bn,in,ni,ew,en,cn,nc,nu,nn".Split(',').Contains(p));
            }
            else if (type == typeof(Guid) || type == typeof(Guid?) || type == typeof(bool) || type == typeof(bool?))
            {
                valid = ("eq,ne".Split(',').Contains(p));
            }

            if (!valid) { throw new Exception(errmsg); };
        }

        public static Expression<Func<T, bool>> GenerateTypeBody<T>(this ParameterExpression param, Filter filterObj)
        {
            return (Expression<Func<T, bool>>)(param.GenerateBody<T>(filterObj));
        }

        /// <summary>
        /// 创建完整的lambda
        /// </summary>
        public static LambdaExpression GenerateLambda(this ParameterExpression param, Expression body)
        {
            //c=>c.XXX=="XXX"
            return Expression.Lambda(body, param);
        }

        public static Expression<Func<T, bool>> GenerateTypeLambda<T>(this ParameterExpression param, Expression body)
        {
            return (Expression<Func<T, bool>>)(param.GenerateLambda(body));
        }

        public static Expression AndAlso(this Expression expression, Expression expressionRight)
        {
            return Expression.AndAlso(expression, expressionRight);
        }

        public static Expression Or(this Expression expression, Expression expressionRight)
        {
            return Expression.Or(expression, expressionRight);
        }

        public static Expression And(this Expression expression, Expression expressionRight)
        {
            return Expression.And(expression, expressionRight);
        }

        //系统已经有该函数的实现
        //public static IQueryable<T> Where<T>(this IQueryable<T> query, Expression expression)
        //{
        //    Expression expr = Expression.Call(typeof(Queryable), "Where", new[] { typeof(T) },
        //       Expression.Constant(query), expression);
        //    //生成动态查询
        //    IQueryable<T> result = query.Provider.CreateQuery<T>(expr);
        //    return result;
        //}

        public static IQueryable<T> GenerateFilter<T>(this IQueryable<T> query, string filterjson)
        {
            if (!string.IsNullOrEmpty(filterjson) && filterjson != "null")
            {
                var filters = JsonHelper.Instance.Deserialize<IEnumerable<Filter>>(filterjson);
                var param = CreateLambdaParam<T>("c");

                Expression result = Expression.Constant(true);
                foreach (var filter in filters)
                {
                    result = result.AndAlso(param.GenerateBody<T>(filter));
                }

                query = query.Where(param.GenerateTypeLambda<T>(result));
            }
            return query;
        }

        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }
    }
}