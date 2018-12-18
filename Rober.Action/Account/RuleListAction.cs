using Rober.Action.Model.Account;
using Rober.Core;
using Rober.Core.Action;
using Rober.Core.Domain.Account;
using Rober.IDAL.Repository;

namespace Rober.Action.Account
{
    public class RuleListAction : ActionSupport<RuleListRequest, RuleListResponse>
    {
        #region 构造函数
        private readonly IRuleRepository _ruleRepository;
        public RuleListAction(IRuleRepository ruleRepository)
        {
            this._ruleRepository = ruleRepository;
        } 
        #endregion

        public override int DoExecute(RuleListRequest request, RequestHeader requestHeader, out RuleListResponse response)
        {
            response = new RuleListResponse();
            if (request.PageSize == 0)
            {
                var list = _ruleRepository.GetList();
                var pageList = new PagedList<Rule>(list);
                response.PagedList = pageList;
                //response.TotalPages = pageList.TotalPages;
                //response.TotalCount = pageList.TotalCount;
            }
            else
            {
                var list = _ruleRepository.GetList(id: request.Id, name: request.Name, publiched: request.Published, pageSize: request.PageSize, pageIndex: request.PageIndex);
                response.PagedList = list;
                //response.TotalPages = list.TotalPages;
                //response.TotalCount = list.TotalCount;
            }
            SetSuccess();
            return Code;
        }
    }
}
