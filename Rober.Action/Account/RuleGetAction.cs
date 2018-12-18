using Rober.Action.Model.Account;
using Rober.Core.Action;
using Rober.IDAL.Repository;

namespace Rober.Action.Account
{
    public class RuleGetAction : ActionSupport<RuleGetRequest, RuleGetResponse>
    {
        #region 构造函数
        private readonly IRuleRepository _baseServer;
        public RuleGetAction(IRuleRepository baseServer)
        {
            this._baseServer = baseServer;
        }
        #endregion

        public override int DoExecute(RuleGetRequest request, RequestHeader requestHeader, out RuleGetResponse response)
        {
            response = new RuleGetResponse();

            response.Rule = _baseServer.Get(request.Id);

            SetSuccess();
            return Code;
        }
    }
}
