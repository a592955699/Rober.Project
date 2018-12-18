using StackExchange.Redis;
using System;

namespace Rober.Core.Caching
{
    /// <summary>
    /// 管理用户登录账号会话信息
    /// </summary>
    public interface IUserSessionCacheManager : IDisposable
    {
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Key of cached item</param>
        /// <param name="field">field of hash</param>
        /// <returns>The cached value associated with the specified key</returns>
        T Get<T>(string key, string field);


        HashEntry[] GetAll(string key);

        /// <summary>
        /// Adds the specified key and object to the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="field">field of hash</param>
        /// <param name="data">Value for caching</param>
        void Set(string key, string field, object data);

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="field">field of hash</param>
        /// <returns>True if item already is in cache; otherwise false</returns>
        bool IsSet(string key, string field);

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="field">field of hash</param>
        void Remove(string key, string field);

        /// <summary>
        /// Clear all cache data
        /// </summary>
        /// <param name="key">Key of cached item</param>
        void Clear(string key);
    }
}