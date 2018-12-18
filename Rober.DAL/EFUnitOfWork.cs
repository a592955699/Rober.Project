using Microsoft.EntityFrameworkCore;
using Rober.Core;
using Rober.Core.Extensions;
using Rober.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Rober.DAL
{
    public class EfUnitOfWork : IEfUnitOfWork
    {
        #region 私有字段
        private readonly IDbContext _dbContext = null; 
        #endregion

        #region 构造函数
        public EfUnitOfWork(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region 共有属性
        public IDbContext Context => _dbContext; 
        #endregion

        #region IUnitOfWorkRepositoryContext接口
        public void RegisterNew<TEntity>(TEntity obj) where TEntity : BaseEntity
        {
            //var state = Context.Entry(obj).State;
            //if (state == EntityState.Detached)
            //{
            //    Context.Entry(obj).State = EntityState.Added;
            //}
            //IsCommitted = false;

            Context.Set<TEntity>().Add(obj);
            IsCommitted = false;
        }

        public void RegisterModified<TEntity>(TEntity obj) where TEntity : BaseEntity
        {
            //if (Context.Entry(obj).State == EntityState.Detached)
            //{
            //    Context.Set<TEntity>().Attach(obj);
            //}
            //Context.Entry(obj).State = EntityState.Modified;
            //IsCommitted = false;

            var dataBaseEntity = Context.Set<TEntity>().Find(obj.Id);
            if (dataBaseEntity != null)
            {
                Context.Entry(dataBaseEntity).CurrentValues.SetValues(obj);
            }
            IsCommitted = false;
        }

        public void RegisterModified<TEntity>(TEntity obj, params Expression<Func<TEntity, object>>[] properties) where TEntity : BaseEntity
        {
            TEntity existingEntity = null;
            if (properties.Any())
            {
                existingEntity = Context.Set<TEntity>().Find(obj.Id);
                foreach (var property in properties)
                {
                    var propertyName = property.GetPropertyName();
                    var value = obj.GetType().GetProperty(propertyName).GetValue(obj);
                    CommonHelper.SetProperty(existingEntity, propertyName, value);
                }
                IsCommitted = false;
            }
            else
            {
                RegisterModified(obj);
            }
        }

        public void RegisterDeleted<TEntity>(TEntity obj) where TEntity : BaseEntity
        {
            //Context.Entry(obj).State = EntityState.Deleted;
            //IsCommitted = false;

            var existingEntity = Context.Set<TEntity>().Find(obj.Id);
            Context.Set<TEntity>().Remove(existingEntity);
            IsCommitted = false;
        }
        #endregion

        #region IUnitOfWork接口

        public bool IsCommitted { get; set; }

        public int Commit()
        {
            if (IsCommitted)
            {
                return 0;
            }
            try
            {
                int result = Context.SaveChanges();
                IsCommitted = true;
                return result;
            }
            catch (DbUpdateException e)
            {

                throw e;
            }
        }

        public void Rollback()
        {
            IsCommitted = false;
        }
        #endregion

        #region IDisposable接口
        public void Dispose()
        {
            if (!IsCommitted)
            {
                Commit();
            }
            Context.Dispose();
        }
        #endregion
    }
}
