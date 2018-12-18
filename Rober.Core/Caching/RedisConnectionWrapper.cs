using System;
using System.Net;
using Rober.Core.Configuration;
using StackExchange.Redis;

namespace Rober.Core.Caching
{
    /// <summary>
    /// Represents Redis connection wrapper implementation
    /// </summary>
    public class RedisConnectionWrapper : IRedisConnectionWrapper
    {
        #region Fields

        private readonly JmConfig _config;

        private readonly Lazy<string> _connectionString;
        private volatile ConnectionMultiplexer _connection;
        //private volatile RedisLockFactory _redisLockFactory;
        private readonly object _lock = new object();
      
        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="config">Config</param>
        public RedisConnectionWrapper(JmConfig config)
        {
            this._config = config;
            this._connectionString = new Lazy<string>(GetConnectionString);
            //this._redisLockFactory = CreateRedisLockFactory();
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get connection string to Redis cache from configuration
        /// </summary>
        /// <returns></returns>
        protected string GetConnectionString()
        {
            return _config.RedisCachingConnectionString;
        }

        /// <summary>
        /// Get connection to Redis servers
        /// </summary>
        /// <returns></returns>
        protected ConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected) return _connection;

            lock (_lock)
            {
                if (_connection != null && _connection.IsConnected) return _connection;

                //Connection disconnected. Disposing connection...
                _connection?.Dispose();

                //Creating new instance of Redis Connection
                _connection = ConnectionMultiplexer.Connect(_connectionString.Value);
            }

            return _connection;
        }

        ///// <summary>
        ///// Create instance of RedisLockFactory
        ///// </summary>
        ///// <returns>RedisLockFactory</returns>
        //protected RedisLockFactory CreateRedisLockFactory()
        //{
        //    //get password and value whether to use ssl from connection string
        //    var password = string.Empty;
        //    var useSsl = false;
        //    foreach (var option in _connectionString.Value.Split(',').Where(option => option.Contains('=')))
        //    {
        //        switch (option.Substring(0, option.IndexOf('=')).Trim().ToLowerInvariant())
        //        {
        //            case "password":
        //                password = option.Substring(option.IndexOf('=') + 1).Trim();
        //                break;
        //            case "ssl":
        //                bool.TryParse(option.Substring(option.IndexOf('=') + 1).Trim(), out useSsl);
        //                break;
        //        }
        //    }

        //    //create RedisLockFactory for using Redlock distributed lock algorithm
        //    return new RedisLockFactory(GetEndPoints().Select(endPoint => new RedisLockEndPoint
        //    {
        //        EndPoint = endPoint,
        //        Password = password,
        //        Ssl = useSsl
        //    }));
        //}

        #endregion

        #region Methods

        /// <summary>
        /// Obtain an interactive connection to a database inside Redis
        /// </summary>
        /// <param name="db">Database number; pass null to use the default value</param>
        /// <returns>Redis cache database</returns>
        public IDatabase GetDatabase(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1);
        }

        /// <summary>
        /// Obtain a configuration API for an individual server
        /// </summary>
        /// <param name="endPoint">The network endpoint</param>
        /// <returns>Redis server</returns>
        public IServer GetServer(EndPoint endPoint)
        {
            return GetConnection().GetServer(endPoint);
        }

        /// <summary>
        /// Gets all endpoints defined on the server
        /// </summary>
        /// <returns>Array of endpoints</returns>
        public EndPoint[] GetEndPoints()
        {
            return GetConnection().GetEndPoints();
        }

        /// <summary>
        /// Delete all the keys of the database
        /// </summary>
        /// <param name="db">Database number; pass null to use the default value</param>
        public void FlushDatabase(int? db = null)
        {
            var endPoints = GetEndPoints();

            foreach (var endPoint in endPoints)
            {
                GetServer(endPoint).FlushDatabase(db ?? -1);
            }
        }

        /// <summary>
        /// Perform some action with Redis distributed lock
        /// </summary>
        /// <param name="resource">The thing we are locking on</param>
        /// <param name="expirationTime">The time after which the lock will automatically be expired by Redis</param>
        /// <param name="action">Action to be performed with locking</param>
        /// <returns>True if lock was acquired and action was performed; otherwise false</returns>
        public bool PerformActionWithLock(string resource, TimeSpan expirationTime, System.Action action)
        {
            //#TODO
            ////use RedLock library
            //using (var redisLock = _redisLockFactory.Create(resource, expirationTime))
            //{
            //    //ensure that lock is acquired
            //    if (!redisLock.IsAcquired)
            //        return false;

            //    //perform action
            //    action();

            //    return true;
            //}

            //perform action
            action();

            return true;
        }

        /// <summary>
        /// Release all resources associated with this object
        /// </summary>
        public void Dispose()
        {
            //dispose ConnectionMultiplexer
            _connection?.Dispose();

            ////dispose RedisLockFactory
            //_redisLockFactory?.Dispose();
        }

        #endregion
    }
}
