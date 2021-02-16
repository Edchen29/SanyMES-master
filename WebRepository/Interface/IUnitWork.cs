﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace WebRepository
{
    /// <summary>
    /// 工作单元接口
    /// <para> 适合在一下情况使用:</para>
    /// <para>1 在同一事务中进行多表操作</para>
    /// <para>2 需要多表联合查询</para>
    /// <para>因为架构采用的是EF访问数据库，暂时可以不用考虑采用传统Unit Work的注册机制</para>
    /// </summary>
    public partial interface IUnitWork
    {
        BaseDBContext GetDbContext();
        LoginInfo _loginInfo { get; set; }
        T FindSingle<T>(Expression<Func<T, bool>> exp = null) where T : class;
        bool IsExist<T>(Expression<Func<T, bool>> exp) where T : class;
        IQueryable<T> Find<T>(Expression<Func<T, bool>> exp = null) where T : class;

        IQueryable<T> Find<T>(int pageindex = 1, int pagesize = 10, string orderby = "",
            Expression<Func<T, bool>> exp = null) where T : class;

        int GetCount<T>(Expression<Func<T, bool>> exp = null) where T : class;

        void Add<T>(T entity) where T : class;

        void BatchAdd<T>(T[] entities) where T : class;

        /// <summary>
        /// 更新一个实体的所有属性
        /// </summary>
        void Update<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;
        void DeleteByTracking<T>(T entity) where T : SysEntity;
        /// <summary>
        /// 实现按需要只更新部分更新
        /// <para>如：Update(u =>u.Id==1,u =>new User{Name="ok"}) where T:class;</para>
        /// </summary>
        /// <param name="where">更新条件</param>
        /// <param name="entity">更新后的实体</param>
        void Update<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity) where T : class;
        /// <summary>
        /// 跟踪更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void UpdateByTracking<T>(T entity) where T : SysEntity;
        /// <summary>
        /// 批量删除
        /// </summary>
        void Delete<T>(Expression<Func<T, bool>> exp) where T : class;

        void Save();

        int ExecuteSql(string sql);

        /// <summary>
        /// 获取任务号
        /// </summary>
        /// <param name="TaskType">TaskType.</param>
        /// <param name="SeqLength">长度.</param>
        string GetTaskNo(string TaskType, int SeqLength = 4);
    }
}
