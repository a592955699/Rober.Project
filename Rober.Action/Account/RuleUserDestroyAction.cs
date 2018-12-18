using Rober.Core.Action;
using Rober.Core.Constants;
using Rober.Core.Domain.Account;
using Rober.Action.Model.Account;
using Rober.IDAL.Repository;

namespace Rober.Action.Account
{
    public class RuleUserDestroyAction : ActionSupport<RuleUserDestroyRequest, RuleUserDestroyResponse>
    {
        #region 构造函数
        private readonly IUserRepository _userServer;
        public RuleUserDestroyAction(IUserRepository userServer)
        {
            this._userServer = userServer;
        }
        #endregion

        public override int DoExecute(RuleUserDestroyRequest request, RequestHeader requestHeader, out RuleUserDestroyResponse response)
        {
            response = new RuleUserDestroyResponse();
            if (_userServer.DestroyUserRuleMapping(request.UserId, request.RuleId))
            {
                SetSuccess();
            }
            else
            {
                SetCode(ResponseCode.DataChangeFail);
            }
            return Code;
        }
    }
}