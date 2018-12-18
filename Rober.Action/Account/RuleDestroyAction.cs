using Rober.Action.Model.Account;
using Rober.Core.Action;
using Rober.Core.Constants;
using Rober.IDAL.Repository;

namespace Rober.Action.Account
{
    public class RuleDestroyAction : ActionSupport<RuleDestroyRequest, RuleDestroyResponse>
    {
        #region 构造函数
        private readonly IRuleRepository _baseServer;
        public RuleDestroyAction(IRuleRepository baseServer)
        {
            this._baseServer = baseServer;
        }
        #endregion

        public override int DoExecute(RuleDestroyRequest request, RequestHeader requestHeader, out RuleDestroyResponse response)
        {
            response = new RuleDestroyResponse();
            if (_baseServer.Delete(request.Rules))
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