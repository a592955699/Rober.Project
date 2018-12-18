using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rober.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rober.IDAL
{
    public interface IDbContext
    {
        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>DbSet</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// 执行sql 获取影响记录行
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="doNotEnsureTransaction"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, params IDbDataParameter[] parameters);

        /// <summary>
        /// 执行sql 获取影响记录行
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="doNotEnsureTransaction"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<int> ExecuteSqlCommandAsync(string sql, bool doNotEnsureTransaction = false, params IDbDataParameter[] parameters);

        /// <summary>
        /// 执行sql 返回第一行第一列
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="doNotEnsureTransaction"></param>
        /// <param name="commandType"></param>
        /// <param name="timeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        TEntity ExecuteScalar<TEntity>(string sql, bool doNotEnsureTransaction = false, CommandType commandType = CommandType.Text, int? timeout = null, params IDbDataParameter[] parameters)
            where TEntity : new();

        /// <summary>
        /// 执行sql 返回第一行第一列
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="doNotEnsureTransaction"></param>
        /// <param name="commandType"></param>
        /// <param name="timeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<TEntity> ExecuteScalarAsync<TEntity>(string sql, bool doNotEnsureTransaction = false, CommandType commandType = CommandType.Text, int? timeout = null, params IDbDataParameter[] parameters)
            where TEntity : new();

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        List<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params IDbDataParameter[] parameters)
            where TEntity : BaseEntity, new();

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<List<TEntity>> ExecuteStoredProcedureListAsync<TEntity>(string commandText, params IDbDataParameter[] parameters)
            where TEntity : BaseEntity, new();

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns>Result</returns>
        int SaveChanges();

        void Dispose();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        ChangeTracker ChangeTracker { get; }
    }
}
