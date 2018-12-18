using Rober.Action.Model.Account;
using Rober.Core.Action;
using Rober.Core.Constants;
using Rober.IDAL.Repository;

namespace Rober.Action.Account
{
    public class RuleUserCreateAction : ActionSupport<RuleUserCreateRequest, RuleUserCreateResponse>
    {
        #region 构造函数
        private readonly IRuleRepository _ruleServer;
        public RuleUserCreateAction(IRuleRepository ruleServer)
        {
            this._ruleServer = ruleServer;
        }
        #endregion

        public override int DoExecute(RuleUserCreateRequest request, RequestHeader requestHeader, out RuleUserCreateResponse response)
        {
            response = new RuleUserCreateResponse();

            if (_ruleServer.RuleUserCreate(request.RuleId, request.UserIds))
            {
                SetSuccess();
                return Code;
            }

            SetCode(ResponseCode.DataChangeFail);
            return Code;
        }
    }
}
