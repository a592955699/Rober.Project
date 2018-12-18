using Newtonsoft.Json;
using Rober.Core.Caching;
using Rober.Core.Infrastructure;
using Rober.Core.Session;
using System.Collections.Generic;

namespace Rober.ServerApp.Framework.Session
{
    public class SessionListener : ISessionListener
    {
        #region 
        public const string SessionPrefix = "S_Portal_";
        public int ExpiredTime { get; set; }
        private IUserSessionCacheManager _hashCacheManager;
        #endregion

        #region 
        public SessionListener()
        {
            _hashCacheManager = EngineContext.Current.Resolve<IUserSessionCacheManager>();
        }
        #endregion

        #region 
        public IList<SessionValueObjectWrapper> GetAllSession()
        {
            var list = new List<SessionValueObjectWrapper>();
            var keys = _hashCacheManager.GetAll(SessionPrefix);
            foreach (var item in keys)
            {
                list.Add(JsonConvert.DeserializeObject<SessionValueObjectWrapper>(item.Value.ToString()));
            }
            return list;
        }

        public void SessionCreated(SessionValueObjectWrapper wrapper, params object[] paras)
        {
            if (wrapper == null) return;
            _hashCacheManager.Set(SessionPrefix, wrapper.UserName, wrapper);

            /*
            //1.写redis
            string sessionKey = GetSessionKey(wrapper.SessionId);
            RedisCacheManager.Instance().Set(sessionKey, wrapper, TimeOut);

            //2.写sql登陆日志
            var ohtherParameter = paras as SessionCreatedParameter;
            ohtherParameter = ohtherParameter ?? new SessionCreatedParameter();
            var sessionLog = new MemSessionLog
            {
                MemId = wrapper.AcctId,
                SessionId = wrapper.SessionId,
                LoginTime = DateTime.Now,
                LoginIp = wrapper.Ip,
                LoginIpGeo = GeoService.GetInstance().GetCity(wrapper.Ip),
                Duration = 0,
                Browser = ohtherParameter.Browser,
                CurrentUrl = ohtherParameter.Referrer,
                UserAgent = ohtherParameter.UserAgent,
                Version = ohtherParameter.Version
            };
            MemSessionLogDao.Save(sessionLog);
            */
        }

        public void SessionExpired(SessionValueObjectWrapper wrapper, params object[] obj)
        {
            if (wrapper == null) return;
            _hashCacheManager.Remove(SessionPrefix, wrapper.UserName);
            /*
            if (wrapper == null) return;

            //1.删除redis
            string sessionKey = GetSessionKey(wrapper.SessionId);
            RedisCacheManager.Instance().Remove(sessionKey);

            //2.更新sql日志
            string sessionId = wrapper.SessionId;
            string userId = wrapper.AcctId;
            MemSessionLog sessionLog = MemSessionLogDao.FindByIdAndSesseionId(userId, sessionId);
            if (sessionLog == null) return;
            sessionLog.LogoutTime = wrapper.LastAccessTime;
            TimeSpan? ts = sessionLog.LogoutTime - sessionLog.LoginTime;
            if (ts.HasValue)
                sessionLog.Duration = Convert.ToInt32(ts.Value.TotalSeconds);
            MemSessionLogDao.Update(sessionLog);

            if (objs != null && objs.Any() && objs[0] is bool && (bool)objs[0])
            {
                MessageQueueHelper.GetInstance().SendMessage(new MessageQueue(CommandName.SessionClosed, sessionId));
            }
            */
        }

        public void SessionChange(IEnumerable<SessionValueObjectWrapper> sessions)
        {

            if (sessions != null)
            {
                foreach (var wrapper in sessions)
                {
                    ////1.写redis
                    //string sessionKey = GetSessionKey(wrapper.SessionId);
                    //RedisCacheManager.Instance().Set(sessionKey, wrapper, TimeOut);

                    _hashCacheManager.Set(SessionPrefix, wrapper.UserName, wrapper);
                }
            }
        }
        #endregion
    }
}
