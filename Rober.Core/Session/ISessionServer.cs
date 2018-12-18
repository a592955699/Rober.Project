using System.Collections.Generic;

namespace Rober.Core.Session
{
    public interface ISessionServer
    {
        int ExpiredTime { get; set; }
        string CurrentSessionKey { get; set; }
        ISessionListener SessionListener { get; set; }
        bool AddSession(SessionValueObjectWrapper sessionValueObjectWrapper, params object[] paras);
        T GetSession<T>(bool changeAccessTime = true);
        T GetSession<T>(string sessionKey, bool changeAccessTime = true);
        SessionValueObjectWrapper GetSessionWrapperByAcctId(string acctId, bool changeAccessTime = true);
        SessionValueObjectWrapper GetSessionWrapperBySessionKey(string sessionKey, bool changeAccessTime = true);
        T GetSessionValueObject<T>(SessionValueObjectWrapper sessionValueObjectWrapper);
        void RemoveSession(string sessionId, params object[] paras);
        #region 供查询在线用户列表使用
        List<string> GetSessionUserNames();
        List<int> GetSessionUserIds();
        #endregion
    }
}
