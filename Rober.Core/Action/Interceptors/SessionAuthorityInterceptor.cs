using Newtonsoft.Json;
using Rober.Core.Constants;
using Rober.Core.Infrastructure;
using Rober.Core.Session;

namespace Rober.Core.Action.Interceptors
{
    public class SessionAuthorityInterceptor: AuthorityInterceptor
    {
        protected override bool IsAuthoritied(ActionInvocation invocation)
        {
            var sessionServer = EngineContext.Current.Resolve<ISessionServer>();
            if (invocation.Request == null)
            {
                sessionServer.CurrentSessionKey = null;
                SessionValueObject.Current = null;
                invocation.Code = ResponseCode.InvalidRequest;
                return false;
            }

            //#TODO JObject 转 SessionRequest 优化
            SessionRequest request;
            if (invocation.Request is SessionRequest)
            {
                request = invocation.Request as SessionRequest;
            }
            else
            {
                request = JsonConvert.DeserializeObject<SessionRequest>(invocation.Request.ToString());
            }
            if (request == null)
            {
                sessionServer.CurrentSessionKey = null;
                SessionValueObject.Current = null;
                invocation.Code = ResponseCode.InvalidRequest;
                return false;
            }

            var session = sessionServer.GetSessionWrapperBySessionKey(request.SessionHead.Sid);
            if (session == null)
            {
                sessionServer.CurrentSessionKey = null;
                SessionValueObject.Current = null;
                invocation.Code = ResponseCode.InvalidSession;
                return false;
            }
            else
            {
                sessionServer.CurrentSessionKey = session.SessionId;
                SessionValueObject.Current = sessionServer.GetSessionValueObject<SessionValueObject>(session);
                return true;
            }
        }
    }
}
