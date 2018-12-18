using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Rober.Core.Log;
using log4net;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Rober.Core.Session
{
    public sealed class SessionServer : ISessionServer, IDisposable
    {
        private ILog Log = Logger.Instance;
        public SessionServer(IOptions<SessionServerOptions> optionsAccessor)
        {
            if (optionsAccessor != null)
            {
                this.SessionListener = optionsAccessor.Value.SessionListener;
                this.ExpiredTime = optionsAccessor.Value.ExpiredTime;
            }
           
            OnSessionInit();
            
            _checkSessionThread = new Thread(CheckSessionTimeout) { Name = "Check-Session-Task" };
            _checkSessionThread.IsBackground = true;
            _checkSessionThread.Start();
        }

        #region 属性/变量定义
        /// <summary>
        /// 有效时间
        /// </summary>
        public int ExpiredTime { get; set; } = 20;

        /// <summary>
        /// SessionListener
        /// </summary>
        public ISessionListener SessionListener { get; set; }

        /// <summary>
        /// key is SessionId
        /// </summary>
        private readonly IDictionary<string, SessionValueObjectWrapper> _sessionMap = new ConcurrentDictionary<string, SessionValueObjectWrapper>();

        /// <summary>
        /// key is AcctId,  value is SessionId
        /// </summary>
        private readonly IDictionary<string, string> _userSession = new ConcurrentDictionary<string, string>();

        private static AsyncLocal<string> _current = new AsyncLocal<string>();
        public string CurrentSessionKey
        {
            get
            {
                return _current.Value;
            }
            set
            {
                _current.Value = value;
            }
        }
        #endregion

        #region session定时检查
        private readonly Thread _checkSessionThread;
        private void CheckSessionTimeout()
        {
            try
            {
                //Log.Trace("SessionManager CheckSessionTimeout");
                while (_checkSessionThread.IsAlive)
                {
                    Thread.Sleep(60000); //60s
                    try
                    {
                        CheckSessionInternal();
                    }
                    catch (Exception ex)
                    {
                        Log.Error("CheckSessionTimeout while", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Check Session failed", ex);
            }
        }
        private void CheckSessionInternal()
        {
            foreach (var item in _sessionMap)
            {
                var timegap = DateTime.Now - item.Value.LastAccessTime;
                if (timegap.TotalMinutes <= ExpiredTime)
                {
                    SessionChange();
                    continue;
                }
                else
                {
                    RemoveSession(item.Key);
                    Log.DebugFormat("SessionManager remove session {0}", item.Key);
                }
            }
        }
        private void SessionChange()
        {
            if (SessionListener == null)
            {
                Log.Warn("SessionCheck fail");
                return;
            }
            try
            {
                var sessions = _sessionMap.Select(x => x.Value);
                SessionListener.SessionChange(sessions);
            }
            catch (Exception ex)
            {
                Log.Error("SessionCheck fail,ex", ex);
            }
        }
        #endregion

        #region 私有方法
        private void OnSessionCreated(SessionValueObjectWrapper wrapper, params object[] paras)
        {
            if (SessionListener == null) return;

            try
            {
                SessionListener.SessionCreated(wrapper, paras);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("error on session created", ex);
            }
        }

        private void OnSessionExpired(SessionValueObjectWrapper wrapper, params object[] paras)
        {
            if (SessionListener == null) return;

            try
            {
                SessionListener.SessionExpired(wrapper, paras);
            }
            catch (Exception ex)
            {
                Log.Error("error on session expired", ex);
            }
        }

        private void OnSessionInit()
        {
            if (SessionListener == null) return;
            SessionListener.ExpiredTime = this.ExpiredTime;
            try
            {
                _sessionMap.Clear();
                _userSession.Clear();

                var sessionValueObjectWrappers = SessionListener.GetAllSession();
                if (sessionValueObjectWrappers == null) return;

                foreach (var sessionItem in sessionValueObjectWrappers.OrderByDescending(x => x.LastAccessTime))
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(sessionItem.UserName) || string.IsNullOrWhiteSpace(sessionItem.SessionId))
                        {
                            //Log.ErrorFormat("SessionInit sessionItem error:" + JsonConvert.SerializeObject(sessionItem));
                            continue;
                        }
                        if (_userSession.ContainsKey(sessionItem.UserName) ||
                            _sessionMap.ContainsKey(sessionItem.SessionId))
                        {
                            SessionListener.SessionExpired(sessionItem);
                            //Log.ErrorFormat("{0} {1} exists,try remove it", sessionItem.AcctId, sessionItem.SessionId);
                            continue;
                        }
                        _userSession.Add(sessionItem.UserName, sessionItem.SessionId);
                        _sessionMap.Add(sessionItem.SessionId, sessionItem);
                        //Log.TraceFormat("load session by redis for {0} - {1} - {2} - {3}", sessionItem.AcctId, sessionItem.SessionId, sessionItem.Token, sessionItem.Ip);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("error on session on init foreach", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("error on session on init", ex);
            }
        }
        #endregion

        /// <summary>
        /// 添加session
        /// </summary>
        /// <param name="sessionValueObjectWrapper"></param>
        /// <param name="paras"></param>
        public bool AddSession(SessionValueObjectWrapper sessionValueObjectWrapper, params object[] paras)
        {
            if (sessionValueObjectWrapper == null || string.IsNullOrEmpty(sessionValueObjectWrapper.UserName) || string.IsNullOrEmpty(sessionValueObjectWrapper.SessionId))
                return false;
            if (_sessionMap.ContainsKey(sessionValueObjectWrapper.SessionId) ||
                _userSession.ContainsKey(sessionValueObjectWrapper.UserName))
            {
                return false;
            }
            _sessionMap.Add(sessionValueObjectWrapper.SessionId, sessionValueObjectWrapper);
            _userSession.Add(sessionValueObjectWrapper.UserName, sessionValueObjectWrapper.SessionId);
            OnSessionCreated(sessionValueObjectWrapper, paras);
            return true;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="changeAccessTime"></param>
        /// <returns></returns>
        public T GetSession<T>(bool changeAccessTime = true)
        {
            var sessionKey = CurrentSessionKey;
            return GetSession<T>(sessionKey, changeAccessTime);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sessionKey"></param>
        /// <param name="changeAccessTime"></param>
        /// <returns></returns>
        public T GetSession<T>(string sessionKey, bool changeAccessTime = true)
        {
            if (!string.IsNullOrEmpty(sessionKey))
            {
                SessionValueObjectWrapper o = null;
                if (_sessionMap.TryGetValue(sessionKey, out o))
                {
                    if (o == null) return default(T);
                    if (changeAccessTime)
                        o.LastAccessTime = DateTime.Now;

                    return GetSessionValueObject<T>(o);
                }
            }
            return default(T);
        }

        public SessionValueObjectWrapper GetSessionWrapperByAcctId(string acctId, bool changeAccessTime = true)
        {
            if (string.IsNullOrEmpty(acctId)) return null;
            if (!_userSession.TryGetValue(acctId, out var sessionId)) return null;
            if (string.IsNullOrWhiteSpace(sessionId) || !_sessionMap.TryGetValue(sessionId, out var o)) return null;
            if (o == null) return null;
            var tspan = DateTime.Now - o.LastAccessTime;
            if (changeAccessTime)
                o.LastAccessTime = DateTime.Now;
            return tspan.TotalMinutes <= ExpiredTime ? o : null;
        }

        public SessionValueObjectWrapper GetSessionWrapperBySessionKey(string sessionKey, bool changeAccessTime = true)
        {
            if (string.IsNullOrEmpty(sessionKey)) return null;
            if (!_sessionMap.TryGetValue(sessionKey, out var o)) return null;
            if (o == null) return null;
            var tspan = DateTime.Now - o.LastAccessTime;
            if (changeAccessTime)
                o.LastAccessTime = DateTime.Now;
            return tspan.TotalMinutes <= ExpiredTime ? o : null;
        }

        public void RemoveSession(string sessionId, params object[] objs)
        {
            if (string.IsNullOrEmpty(sessionId)) return;

            if (!_sessionMap.TryGetValue(sessionId, out var wrapper)) return;

            var userId = wrapper.UserName ?? string.Empty;
            OnSessionExpired(wrapper, objs);
            _sessionMap.Remove(sessionId);
            _userSession.Remove(userId);
            //var msg = string.Format("session expired, session id {0} for user {1} {2}", sessionId, userId, JsonConvert.SerializeObject(wrapper));
            //Log.Info(msg);
        }

        public T GetSessionValueObject<T>(SessionValueObjectWrapper sessionValueObjectWrapper)
        {
            if (sessionValueObjectWrapper == null || sessionValueObjectWrapper.ValueObject == null) return default(T);
            if (sessionValueObjectWrapper.ValueObject is JObject)
            {
                var jObject = (JObject)sessionValueObjectWrapper.ValueObject;
                return jObject == null ? default(T) : jObject.ToObject<T>();
            }
            else
            {
                return (T)sessionValueObjectWrapper.ValueObject;
            }
        }

        #region 查询在线用户列表使用
        public List<string> GetSessionUserNames()
        {
            return _userSession.Select(x => x.Key).ToList();
        }
        public List<int> GetSessionUserIds()
        {
            return _sessionMap.Select(x => x.Value.UserId).ToList();
        }
        #endregion

        public void Dispose()
        {
            if (_checkSessionThread != null && _checkSessionThread.IsAlive) { _checkSessionThread.Abort(); }
        }

       
    }

   
    /// <summary>
    /// Configuration options for <see cref="SessionServerOptions"/>.
    /// </summary>
    public class SessionServerOptions : IOptions<SessionServerOptions>
    {
        /// <summary>
        /// 有效时间
        /// </summary>
        public int ExpiredTime { get; set; } = 20;

        /// <summary>
        /// SessionListener
        /// </summary>
        public ISessionListener SessionListener { get; set; }

        SessionServerOptions IOptions<SessionServerOptions>.Value
        {
            get { return this; }
        }
    }

}
