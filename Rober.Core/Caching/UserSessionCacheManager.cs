using Newtonsoft.Json;
using Rober.Core.Configuration;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Rober.Core.Caching
{
    /// <summary>
    /// 管理用户登录账号会话信息
    /// </summary>
    public class UserSessionCacheManager: IUserSessionCacheManager
    {
        #region Fields
        private readonly IRedisConnectionWrapper _connectionWrapper;
        private readonly IDatabase _db;
        #endregion

        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="perRequestCacheManager">Cache manager</param>
        /// <param name="connectionWrapper">ConnectionW wrapper</param>
        /// <param name="config">Config</param>
        public UserSessionCacheManager(
            IRedisConnectionWrapper connectionWrapper,
            JmConfig config)
        {
            if (string.IsNullOrEmpty(config.RedisCachingConnectionString))
                throw new Exception("Redis connection string is empty");

            // ConnectionMultiplexer.Connect should only be called once and shared between callers
            this._connectionWrapper = connectionWrapper;

            this._db = _connectionWrapper.GetDatabase();
        }
        #endregion

        #region Utilities

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Key of cached item</param>
        /// <param name="field">field of hash</param>
        /// <returns>The cached value associated with the specified key</returns>
        protected virtual async Task<T> GetAsync<T>(string key, string field)
        {
            //get serialized item from cache
            var serializedItem = await _db.HashGetAsync(key, field);
            if (!serializedItem.HasValue)
                return default(T);

            //deserialize item
            var item = JsonConvert.DeserializeObject<T>(serializedItem);
            if (item == null)
                return default(T);

            return item;
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Key of cached item</param>
        /// <param name="field">field of hash</param>
        /// <returns>The cached value associated with the specified key</returns>
        protected virtual async Task<HashEntry[]> GetAllAsync(string key)
        {
            return await _db.HashGetAllAsync(key);
        }

        /// <summary>
        /// Adds the specified key and object to the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="field">field of hash</param>
        /// <param name="data">Value for caching</param>
        protected virtual async Task SetAsync(string key, string field, object data)
        {
            if (data == null)
                return;

            //serialize item
            var serializedItem = JsonConvert.SerializeObject(data);

            //and set it to cache
            await _db.HashSetAsync(key, field, serializedItem);
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="field">field of hash</param>
        /// <returns>True if item already is in cache; otherwise false</returns>
        protected virtual async Task<bool> IsSetAsync(string key, string field)
        {
            return await _db.HashExistsAsync(key, field);
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="field">field of hash</param>
        protected virtual async Task RemoveAsync(string key, string field)
        {
            ////we should always persist the data protection key list
            //if (key.Equals(RedisConfiguration.DataProtectionKeysName, StringComparison.OrdinalIgnoreCase))
            //    return;

            //remove item from caches
            await _db.HashDeleteAsync(key, field);
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="field">field of hash</param>
        protected virtual async Task ClearAsync(string key)
        {
            //remove item from caches
            await _db.KeyDeleteAsync(key);
        }
        #endregion

        #region Methods

        public virtual async void Clear(string key)
        {
            await this.ClearAsync(key);
        }

        public virtual void Dispose()
        {
            
        }

        public virtual T Get<T>(string key, string field)
        {
            return this.GetAsync<T>(key, field).Result;
        }
        public virtual HashEntry[] GetAll(string key)
        {
            return this.GetAllAsync(key).Result;
        }

        public virtual bool IsSet(string key, string field)
        {
            return this.IsSetAsync(key, field).Result;
        }

        public virtual async void Remove(string key, string field)
        {
            await this.RemoveAsync(key, field);
        }

        public virtual async void Set(string key, string field, object data)
        {
            await this.SetAsync(key, field, data);
        }
        #endregion
    }
}
