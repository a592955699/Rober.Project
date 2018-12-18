using Rober.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rober.IDAL
{
    public interface IRepository<T> where T : BaseEntity
    {
        //bool SaveChanges(bool? isAsc = false);
        T Get(object key);
        T Get(Expression<Func<T, bool>> whereLambda);
        List<T> GetList();
        //List<T> GetList(string entity);
        List<T> GetList(Expression<Func<T, bool>> whereLambda);
        PagedList<T> GetList<model>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda = null, string orderby = null, bool? isAsc = null);
        bool Delete(T entity, bool IsCommit = true);
        bool Delete(IEnumerable<T> entities, bool IsCommit = true);
        bool Update(T entity, bool IsCommit = true);
        bool Update(T entity, bool IsCommit = true, params Expression<Func<T, object>>[] properties);
        bool Update(IEnumerable<T> entities, bool IsCommit = true);
        bool Update(IEnumerable<T> entities, bool IsCommit = true, params Expression<Func<T, object>>[] properties);
        bool Insert(T entity, bool IsCommit = true);
        bool Insert(IEnumerable<T> entities, bool IsCommit = true);
        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<T> Table { get; }
        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<T> TableNoTracking { get; }
    }
}
