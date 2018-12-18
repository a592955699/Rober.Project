using Newtonsoft.Json;
using Rober.Core.Caching;
using Rober.Core.Infrastructure;
using Rober.Core.Session;
using System.Collections.Generic;

namespace JM.Portal.App.Framework.Session
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
                //var json = item.ToString().TrimStart(string.Format("{0}:",item.Name).ToCharArray());
                //list.Add(JsonConvert.DeserializeObject<SessionValueObjectWrapper>(json));
                list.Add(JsonConvert.DeserializeObject<SessionValueObjectWrapper>(item.Value.ToString()));
            }
            return list;
        }

        public void SessionCreated(SessionValueObjectWrapper wrapper, params object[] paras)
        {
            
        }

        public void SessionExpired(SessionValueObjectWrapper wrapper, params object[] obj)
        {
            
        }

        public void SessionChange(IEnumerable<SessionValueObjectWrapper> sessions)
        {

        } 
        #endregion
    }
}
