// ***********************************************************************
// <summary>仓储接口</summary>
// ***********************************************************************

using System;
using System.Linq;
using System.Linq.Expressions;

namespace WebRepository
{
    public interface IRepository<T> where T : class
    {
        LoginInfo _loginInfo { get; set; }
        bool IsExist(Expression<Func<T, bool>> exp);

        T FindSingle(Expression<Func<T, bool>> exp = null);

        IQueryable<T> Find(Expression<Func<T, bool>> exp = null);

        IQueryable<T> Find(int pageindex = 1, int pagesize = 10, string orderby = "",
            Expression<Func<T, bool>> exp = null);

        int GetCount(Expression<Func<T, bool>> exp = null);

        void Add(T entity);

        void BatchAdd(T[] entities);

        /// <summary>
        /// 更新一个实体的所有属性
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// 应对同一主键多次操作
        /// </summary>
        /// <param name="entity"></param>
        void UpdateByTracking(T entity);

        /// <summary>
        /// 实现按需要只更新部分更新
        /// <para>如：Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        /// <param name="where">更新条件</param>
        /// <param name="entity">更新后的实体</param>
        void Update(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity);

        void Delete(T entity);

        /// <summary>
        /// 应对同一主键多次操作
        /// </summary>
        /// <param name="entity"></param>
        void DeleteByTracking(T entity);

        /// <summary>
        /// 批量删除
        /// </summary>
        void Delete(Expression<Func<T, bool>> exp);

        void Save();

        int ExecuteSql(string sql);

        TableData Load(PageReq pageRequest, T entity);

        TableData ExportData(T entity);

        void GetData(IQueryable<T> data, TableData result, PageReq pageRequest = null);

        /// <summary>
        /// 获取任务号
        /// </summary>
        /// <param name="TaskType">TaskType.</param>
        /// <param name="SeqLength">长度.</param>
        string GetTaskNo(string TaskType, int SeqLength = 4);
    }
}