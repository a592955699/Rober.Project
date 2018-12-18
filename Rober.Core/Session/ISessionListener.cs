using System.Collections.Generic;

namespace Rober.Core.Session
{
    /// <summary>
    /// ISessionListener
    /// </summary>
    public interface ISessionListener
    {
        /// <summary>
        /// 过期时间
        /// </summary>
        int ExpiredTime { get; set; }
        /// <summary>
        /// 重启后，从 redis 中加载session
        /// </summary>
        /// <returns></returns>
        IList<SessionValueObjectWrapper> GetAllSession();
        /// <summary>
        /// SessionCreated
        /// </summary>
        /// <param name="wrapper"></param>
        /// <param name="paras"></param>
        void SessionCreated(SessionValueObjectWrapper wrapper, params object[] paras);
        /// <summary>
        /// SessionExpired
        /// </summary>
        /// <param name="wrapper"></param>
        /// <param name="obj"></param>
        void SessionExpired(SessionValueObjectWrapper wrapper, params object[] obj);
        /// <summary>
        /// session最后访问时间变化
        /// </summary>
        /// <param name="sessions"></param>
        void SessionChange(IEnumerable<SessionValueObjectWrapper> sessions);
    }
}
