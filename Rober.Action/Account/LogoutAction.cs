using Rober.Core.Action;
using Rober.Core.Constants;
using Rober.Action.Model.Account;
using Rober.Core.Session;

namespace Rober.Action.Account
{
    public class LogoutAction : ActionSupport<LogoutRequest, LogoutResponse>
    {
        private readonly ISessionServer _sessionServer;
        public LogoutAction(ISessionServer sessionServer)
        {
            this._sessionServer = sessionServer;
        }
        public override int DoExecute(LogoutRequest request, RequestHeader requestHeader, out LogoutResponse response)
        {
            response = new LogoutResponse();
            if (KickOff(request.SessionHead.Sid))
                SetSuccess();
            else
                SetCode(ResponseCode.InvalidSession);
            return Code;
        }

        private bool KickOff(string sessionId)
        {
            var sessionWrapper = _sessionServer.GetSessionWrapperBySessionKey(sessionId);
            if (sessionWrapper == null) return false;
            //移除历史session
            _sessionServer.RemoveSession(sessionWrapper.SessionId);
            _sessionServer.CurrentSessionKey = null;
            SessionValueObject.Current = null;
            //#TODO 记录退出日志

            //#TODO 推送踢人事件

            return true;
        }
    }
}
