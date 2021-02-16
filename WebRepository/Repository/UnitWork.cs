using Infrastructure;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Z.EntityFramework.Plus;

namespace WebRepository
{
    public partial class UnitWork : IUnitWork
    {
        private BaseDBContext _context;
        public LoginInfo _loginInfo { get; set; }

        public UnitWork(BaseDBContext context)
        {
            _context = context;
        }

        public BaseDBContext GetDbContext()
        {
            return _context;
        }

        /// <summary>
        /// 根据过滤条件，获取记录
        /// </summary>
        /// <param name="exp">The exp.</param>
        public IQueryable<T> Find<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return Filter(exp);
        }

        public bool IsExist<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return _context.Set<T>().Any(exp);
        }

        /// <summary>
        /// 查找单个
        /// </summary>
        public T FindSingle<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return _context.Set<T>().AsNoTracking().FirstOrDefault(exp);
        }

        /// <summary>
        /// 得到分页记录
        /// </summary>
        /// <param name="pageindex">The pageindex.</param>
        /// <param name="pagesize">The pagesize.</param>
        /// <param name="orderby">排序，格式如："Id"/"Id descending"</param>
        /// <param name="exp">表达式</param>
        public IQueryable<T> Find<T>(int pageindex, int pagesize, string orderby = "", Expression<Func<T, bool>> exp = null) where T : class
        {
            if (pageindex < 1) pageindex = 1;
            if (string.IsNullOrEmpty(orderby))
                orderby = "1 asc";

            return Filter(exp).OrderBy(orderby).Skip(pagesize * (pageindex - 1)).Take(pagesize);
        }

        /// <summary>
        /// 根据过滤条件获取记录数
        /// </summary>
        public int GetCount<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return Filter(exp).Count();
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
            Save();
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void BatchAdd<T>(T[] entities) where T : class
        {
            _context.Set<T>().AddRange(entities);
            Save();
        }

        public void Update<T>(T entity) where T : class
        {
            var entry = this._context.Entry(entity);
            entry.State = EntityState.Modified;

            //如果数据没有发生变化
            if (!this._context.ChangeTracker.HasChanges())
            {
                entry.State = EntityState.Unchanged;
            }
            Save();
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
            Save();
        }

        public virtual void Delete<T>(Expression<Func<T, bool>> exp) where T : class
        {
            System.Collections.Generic.List<T> entitys = _context.Set<T>().Where(exp).ToList();
            foreach (var item in entitys)
            {
                Delete(item);
            }
        }
        public void DeleteByTracking<T>(T entity) where T : SysEntity
        {
            T entity_exist = _context.Set<T>().AsQueryable().Where(u => u.Id.Equals(entity.Id)).FirstOrDefault();
            if (entity_exist != null)
            {
                _context.Set<T>().Remove(entity_exist);
            }
            else
            {
                _context.Set<T>().Remove(entity);
            }

            Save();
        }
        /// <summary>
        /// 实现按需要只更新部分更新
        /// <para>如：Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="entity">The entity.</param>
        public void Update<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity) where T : class
        {
            _context.Set<T>().Where(where).Update(entity);
            Save();
        }
        public void UpdateByTracking<T>(T entity) where T : SysEntity
        {
            if (string.IsNullOrEmpty(entity.CreateBy))
            {
                T oldentity = FindSingle<T>(u => u.Id == entity.Id);
                entity.CreateBy = oldentity.CreateBy;
                entity.CreateTime = oldentity.CreateTime;
            }
            entity.UpdateBy = "UnitWork";
            // entity.UpdateBy = _loginInfo.Account;
            entity.UpdateTime = DateTime.Now;

            T entity_exist = _context.Set<T>().AsQueryable().Where(u => u.Id.Equals(entity.Id)).FirstOrDefault();

            if (entity_exist != null)
            {
                foreach (var property in entity_exist.GetType().GetProperties())
                {
                    var propertyValue = entity.GetType().GetProperty(property.Name).GetValue(entity, null);
                    if (propertyValue != null)
                    {
                        if (propertyValue.GetType().IsClass)
                        {

                        }
                        entity_exist.GetType().InvokeMember(property.Name, BindingFlags.SetProperty, null, entity_exist, new object[] { propertyValue });
                    }
                }

                foreach (var field in entity_exist.GetType().GetFields())
                {
                    var fieldValue = entity.GetType().GetField(field.Name).GetValue(entity);
                    if (fieldValue != null)
                    {
                        entity_exist.GetType().InvokeMember(field.Name, BindingFlags.SetField, null, entity_exist, new object[] { fieldValue });
                    }
                }

                var entry = this._context.Entry(entity_exist);
                entry.State = EntityState.Modified;
                //如果数据没有发生变化
                if (!this._context.ChangeTracker.HasChanges())
                {
                    return;
                }
            }
            else
            {
                var entry = this._context.Entry(entity);
                entry.State = EntityState.Modified;
                //如果数据没有发生变化
                if (!this._context.ChangeTracker.HasChanges())
                {
                    return;
                }
            }

            Save();
        }
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string sMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    sMessage = ex.InnerException.Message;

                    if (ex.InnerException.InnerException != null)
                    {
                        sMessage = ex.InnerException.InnerException.Message;
                    }
                }
                throw new Exception(sMessage);
            }
        }

        private IQueryable<T> Filter<T>(Expression<Func<T, bool>> exp) where T : class
        {
            var dbSet = _context.Set<T>().AsNoTracking().AsQueryable();
            if (exp != null)
                dbSet = dbSet.Where(exp);
            return dbSet;
        }

        public int ExecuteSql(string sql)
        {
            try
            {
                return _context.Database.ExecuteSqlCommand(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.Replace("ROLLBACK TRANSACTION 请求没有对应的 BEGIN TRANSACTION。", "", StringComparison.CurrentCulture));
            }
        }
        /// <summary>
        /// 获取任务号
        /// </summary>
        /// <param name="TaskType">TaskType.</param>
        /// <param name="SeqLength">长度.</param>
        public string GetTaskNo(string TaskType, int SeqLength = 4)
        {
            string Value = "1";
            SysCount sysCount = _context.Set<SysCount>().AsNoTracking().FirstOrDefault(u => u.Type == TaskType);
            if (sysCount == null)
            {
                Value = TaskType + DateTime.Now.ToString("yyyyMMdd") + Value.PadLeft(SeqLength, '0');
                sysCount = new SysCount { Type = TaskType, Value = Value };
                _context.Set<SysCount>().Add(sysCount);
                Save();
            }
            else
            {
                string Date = sysCount.Value.Substring(TaskType.Length, 8);

                if (Date == DateTime.Now.ToString("yyyyMMdd"))
                {
                    Value = Date + (int.Parse(sysCount.Value.Substring(sysCount.Value.Length - 4, 4)) + 1).ToString().PadLeft(SeqLength, '0');
                }
                else
                {
                    Value = DateTime.Now.ToString("yyyyMMdd") + Value.PadLeft(SeqLength, '0');
                }
                Value = TaskType + Value;
                _context.Set<SysCount>().Where(u => u.Type == TaskType).Update(u => new SysCount { Value = Value });
                Save();
            }
            return Value;
        }
    }
}
