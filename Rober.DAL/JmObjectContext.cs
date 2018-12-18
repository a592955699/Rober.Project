using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rober.Core;
using Rober.Core.Domain.Account;
using Rober.Core.Domain.Common;
using Rober.Core.Domain.Schedules;
using Rober.IDAL;
using Rober.Model.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rober.DAL
{
    public class JmObjectContext : DbContext, IDbContext
    {
        #region 构造函数
        public JmObjectContext(DbContextOptions<JmObjectContext> options)
            : base(options)
        {
        }
        #endregion

        #region DbSet
        public DbSet<User> User { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Rober.Core.Domain.Account.Rule> Rule { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<UserDepartmentMapping> UserDepartmentMapping { get; set; }
        public DbSet<UserMenuMapping> UserMenuMapping { get; set; }
        public DbSet<UserRuleMapping> UserRuleMapping { get; set; }
        public DbSet<RuleMenuMapping> RuleMenuMapping { get; set; }
        public DbSet<GenericAttribute> GenericAttribute { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<ScheduleMapping> ScheduleMapping { get; set; }
        public DbSet<ScheduleSubCategory> ScheduleSubCategory { get; set; }
        public DbSet<CustomerFile> CustomerFile { get; set; }
        #endregion

        #region 公有方法
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public new EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return base.Entry<TEntity>(entity);
        }
        public new ChangeTracker ChangeTracker { get { return base.ChangeTracker; } }
        /// <summary>
        /// 执行sql 获取影响记录行
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="doNotEnsureTransaction"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, params IDbDataParameter[] parameters)
        {
            if (doNotEnsureTransaction)
            {
                using (var transaction = Database.BeginTransaction())
                {
                    try
                    {
                        var row = Database.ExecuteSqlCommand(sql, parameters);
                        transaction.Commit();
                        return row;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
            else
            {
                return Database.ExecuteSqlCommand(sql);
            }
        }
        /// <summary>
        /// 执行sql 获取影响记录行
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="doNotEnsureTransaction"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<int> ExecuteSqlCommandAsync(string sql, bool doNotEnsureTransaction = false, params IDbDataParameter[] parameters)
        {
            if (doNotEnsureTransaction)
            {
                using (var transaction = Database.BeginTransaction())
                {
                    try
                    {
                        var row = await Database.ExecuteSqlCommandAsync(sql, parameters);
                        transaction.Commit();
                        return row;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
            else
            {
                return await Database.ExecuteSqlCommandAsync(sql, parameters);
            }
        }

        /// <summary>
        /// 执行sql 返回第一行第一列
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="doNotEnsureTransaction">false - the transaction creation is not ensured; true - the transaction creation is ensured.</param>
        /// <param name="commandType"></param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public TEntity ExecuteScalar<TEntity>(string sql, bool doNotEnsureTransaction = false, CommandType commandType = CommandType.Text, int? timeout = null, params IDbDataParameter[] parameters)
            where TEntity : new()
        {
            var connection = Database.GetDbConnection();
            using (var cmd = connection.CreateCommand())
            {
                Database.OpenConnection();
                if (timeout.HasValue)
                    cmd.CommandTimeout = timeout.Value;
                cmd.CommandText = sql;
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);
                return (TEntity)cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// 执行sql 返回第一行第一列
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="doNotEnsureTransaction">false - the transaction creation is not ensured; true - the transaction creation is ensured.</param>
        /// <param name="commandType"></param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public async Task<TEntity> ExecuteScalarAsync<TEntity>(string sql, bool doNotEnsureTransaction = false, CommandType commandType = CommandType.Text, int? timeout = null, params IDbDataParameter[] parameters)
            where TEntity : new()
        {
            var connection = Database.GetDbConnection();
            using (var cmd = connection.CreateCommand())
            {
                await Database.OpenConnectionAsync();
                if (timeout.HasValue)
                    cmd.CommandTimeout = timeout.Value;
                cmd.CommandText = sql;
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);
                var taskObjecct = await cmd.ExecuteScalarAsync();
                return (TEntity)taskObjecct;
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params IDbDataParameter[] parameters) where TEntity : BaseEntity, new()
        {
            var connection = Database.GetDbConnection();
            using (var cmd = connection.CreateCommand())
            {
                Database.OpenConnection();
                cmd.CommandText = commandText;
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                using (IDataReader dataRead = cmd.ExecuteReader())
                {
                    var result = dataRead.DataReaderToObjectList<TEntity>();

                    for (var i = 0; i < result.Count; i++)
                        result[i] = AttachEntityToContext(result[i]);

                    return result;
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> ExecuteStoredProcedureListAsync<TEntity>(string commandText, params IDbDataParameter[] parameters) where TEntity : BaseEntity, new()
        {
            var connection = Database.GetDbConnection();
            using (var cmd = connection.CreateCommand())
            {
                Database.OpenConnection();
                cmd.CommandText = commandText;
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                using (IDataReader dataRead = await cmd.ExecuteReaderAsync())
                {
                    var result = dataRead.DataReaderToObjectList<TEntity>();

                    //for (var i = 0; i < result.Count; i++)
                    //    result[i] = AttachEntityToContext(result[i]);

                    return result;
                }
            }
        }
        public bool HasChanges()
        {
            return ChangeTracker.HasChanges();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //dynamically load all configuration
            //System.Type configType = typeof(LanguageMap);   //any of your configuration classes here
            //var typesToRegister = Assembly.GetAssembly(configType).GetTypes()

            //注意 mapping JmEntityTypeConfiguration<> 必须和 JmObjectContext 放同一个项目下面
            //否则需要另外处理 Assembly.GetExecutingAssembly().GetTypes()
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                               type.BaseType.GetGenericTypeDefinition() == typeof(JmEntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                Activator.CreateInstance(type, modelBuilder);
            }
            //...or do it manually below. For example,
            //modelBuilder.Configurations.Add(new LanguageMap());
            base.OnModelCreating(modelBuilder);
        }
        /// <summary>
        /// Attach an entity to the context or return an already attached entity (if it was already attached)
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Attached entity</returns>
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        {
            //little hack here until Entity Framework really supports stored procedures
            //otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                //attach new entity
                Set<TEntity>().Attach(entity);
                return entity;
            }

            //entity is already loaded
            return alreadyAttached;
        }
        #endregion
    }
}
