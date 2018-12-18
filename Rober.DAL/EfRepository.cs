using Microsoft.EntityFrameworkCore;
using Rober.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Rober.Core.Extensions;
using Rober.Core;

namespace Rober.DAL
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        #region Fields
        private DbSet<T> _entities;
        IEfUnitOfWork _unitOfWork;
        #endregion

        #region 属性
        public virtual IEfUnitOfWork UnitOfWork => _unitOfWork;
        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table => Entities;
        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking => Entities.AsNoTracking();
        /// <summary>
        /// Entities
        /// </summary>
        protected virtual DbSet<T> Entities => _entities ?? (_entities = _unitOfWork.Context.Set<T>());
        #endregion

        #region 构造函数   
        public EfRepository(IEfUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region 公有方法
        public virtual bool Insert(T entity, bool IsCommit = true)
        {
            //Entities.Add(entity);
            UnitOfWork.RegisterNew(entity);
            return IsCommit && _unitOfWork.Commit() > 0;
        }
        public virtual bool Insert(IEnumerable<T> entities, bool IsCommit = true)
        {
            foreach (var entity in entities)
            {
                //Entities.Add(entity);
                UnitOfWork.RegisterNew(entity);
            }
            return IsCommit && _unitOfWork.Commit() > 0;
        }
        public virtual bool Delete(T entity, bool IsCommit = true)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            //var existingEntity = Entities.Find(entity.Id);
            //Entities.Remove(existingEntity);
            UnitOfWork.RegisterDeleted(entity);
            return IsCommit && _unitOfWork.Commit() > 0;
        }
        public virtual bool Delete(IEnumerable<T> entities, bool IsCommit = true)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
            {
                //var existingEntity = Entities.Find(entity.Id);
                //Entities.Remove(existingEntity);
                UnitOfWork.RegisterDeleted(entity);
            }
            return IsCommit && _unitOfWork.Commit() > 0;
        }
        public virtual bool Update(T entity, bool IsCommit = true)
        {
            //方式二，全部更新
            var dataBaseEntity = Entities.Find(entity.Id);
            if (dataBaseEntity != null)
            {
                //DbContext.Entry(dataBaseEntity).CurrentValues.SetValues(entity);
                UnitOfWork.RegisterModified(entity);
                return HasChanges() && IsCommit && _unitOfWork.Commit() > 0;
            }
            else
            {
                return false;
            }
        }
        public virtual bool Update(T entity, bool IsCommit = true, params Expression<Func<T, object>>[] properties)
        {
            //T existingEntity = null;
            //if (properties.Any())
            //{
            //    existingEntity = Entities.Find(entity.Id);
            //    foreach (var property in properties)
            //    {
            //        var propertyName = property.GetPropertyName();
            //        var value = entity.GetType().GetProperty(propertyName).GetValue(entity);
            //        CommonHelper.SetProperty(existingEntity, propertyName, value);
            //    }
            //    Entities.Update(existingEntity);
            //    return HasChanges() && IsCommit && _unitOfWork.Commit() > 0;
            //}
            //else
            //{
            //    return Update(entity, IsCommit);
            //}

            //方式二，全部更新
            var dataBaseEntity = Entities.Find(entity.Id);
            if (dataBaseEntity != null)
            {
                UnitOfWork.RegisterModified(entity, properties);
                return HasChanges() && IsCommit && _unitOfWork.Commit() > 0;
            }
            else
            {
                return false;
            }
        }
        public virtual bool Update(IEnumerable<T> entities, bool IsCommit = true)
        {
            foreach (var entity in entities)
            {
                //方式二，全部更新
                var dataBaseEntity = Entities.Find(entity.Id);
                if (dataBaseEntity != null)
                {
                    //DbContext.Entry(dataBaseEntity).CurrentValues.SetValues(entity);
                    UnitOfWork.RegisterModified(entity);
                }
            }
            return HasChanges() && IsCommit && _unitOfWork.Commit() > 0;
        }
        public virtual bool Update(IEnumerable<T> entities, bool IsCommit = true, params Expression<Func<T, object>>[] properties)
        {
            //if (properties.Any())
            //{
            //    foreach (var entity in entities)
            //    {
            //        T existingEntity = null;

            //        existingEntity = Entities.Find(entity.Id);
            //        foreach (var property in properties)
            //        {
            //            var propertyName = property.GetPropertyName();
            //            var value = entity.GetType().GetProperty(propertyName).GetValue(entity);
            //            CommonHelper.SetProperty(existingEntity, propertyName, value);
            //        }
            //        Entities.Update(existingEntity);
            //    }
            //    return HasChanges() && IsCommit && _unitOfWork.Commit() > 0;
            //}
            //else
            //{
            //    return Update(entities, IsCommit);
            //}

            foreach (var entity in entities)
            {
                UnitOfWork.RegisterModified(entity, properties);
            }
            return HasChanges() && IsCommit && _unitOfWork.Commit() > 0;
        }
        public virtual T Get(object key)
        {
            return Entities.Find(key);
        }
        /// <summary>
        /// 注意区分大小写问题
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public virtual T Get(Expression<Func<T, bool>> whereLambda)
        {
            return Entities.FirstOrDefault(whereLambda.Compile());
        }
        public virtual List<T> GetList()
        {
            return Table.ToList();
        }
        public virtual List<T> GetList(Expression<Func<T, bool>> whereLambda)
        {
            return Table.Where<T>(whereLambda).ToList();
        }
        //[Obsolete]
        //public virtual IQueryable<T> GetList(string entity)
        //{
        //    return string.IsNullOrEmpty(entity) ? Table : Table.Include(entity);
        //}
        public virtual PagedList<T> GetList<model>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda = null, string orderby = null, bool? isAsc = null)
        {
            //var temp = Table.Where<T>(whereLambda.Compile()).AsQueryable(); //Compile()
            //toalCount = temp.Count();
            //if (isAsc.HasValue)//排序
            //{
            //    temp = isAsc.Value ? temp.OrderBy<T>(orderby).Skip<T>((pageIdex - 1) * pageSize).Take<T>(pageSize) : temp = temp.OrderByDescending<T>(orderby).Skip<T>((pageIdex - 1) * pageSize).Take<T>(pageSize); ;
            //}
            //else
            //{
            //    temp = temp.Skip<T>((pageIdex - 1) * pageSize).Take<T>(pageSize);
            //}
            //return temp;

            var temp = Table;
            if (whereLambda != null)
            {
                temp = temp.Where<T>(whereLambda.Compile()).AsQueryable();
            }
            if (isAsc.HasValue && !string.IsNullOrWhiteSpace(orderby))//排序
            {
                temp = isAsc.Value ? temp.OrderBy<T>(orderby).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize) : temp = temp.OrderByDescending<T>(orderby).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize); ;
            }
            else
            {
                temp = temp.Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            return new PagedList<T>(temp, pageIndex, pageSize);
        }
        #endregion

        #region 私有方法
        private bool HasChanges()
        {
            return _unitOfWork.Context.ChangeTracker.HasChanges();
        }
        #endregion
    }
}
