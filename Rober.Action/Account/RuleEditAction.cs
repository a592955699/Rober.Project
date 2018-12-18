using Rober.Core.Action;
using Rober.Core.Constants;
using Rober.Core.Domain.Account;
using Rober.Action.Model.Account;
using Rober.IDAL.Repository;

namespace Rober.Action.Account
{
    public class RuleEditAction : ActionSupport<RuleEditRequest, RuleEditResponse>
    {
        #region 构造函数
        private readonly IRuleRepository _baseServer;
        public RuleEditAction(IRuleRepository baseServer)
        {
            this._baseServer = baseServer;
        }
        #endregion

        public override int DoExecute(RuleEditRequest request, RequestHeader requestHeader, out RuleEditResponse response)
        {
            response = new RuleEditResponse();
            if (request.Rule.Id > 0)
            {
                #region 修改
                _baseServer.Update(request.Rule);
                response.Rule = request.Rule;
                SetSuccess();
                return Code;
                #endregion
            }
            else
            {
                #region 新增
                if (_baseServer.Insert(request.Rule))
                {
                    response.Rule = request.Rule;
                    SetSuccess();
                    return Code;
                }
                #endregion
            }
            SetCode(ResponseCode.DataChangeFail);
            return Code;
        }
    }
}
