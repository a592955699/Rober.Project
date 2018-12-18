using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Action;
using Rober.Core.Session;
using Rober.Action.Model.Account;
using Rober.IDAL.Repository;

namespace Rober.Action.Account
{
    public class RuleUserListAction : ActionSupport<RuleUserListRequest, RuleUserListResponse>
    {
        #region 构造函数
        private readonly IUserRepository _userServer;
        public RuleUserListAction(IUserRepository userServer)
        {
            this._userServer = userServer;
        }
        #endregion

        public override int DoExecute(RuleUserListRequest request, RequestHeader requestHeader, out RuleUserListResponse response)
        {
            response = new RuleUserListResponse();
            var list = _userServer.GetList(ruleId: request.RuleId, userName: request.UserName,nickName:request.NickName, publiched: request.Published, pageSize: request.PageSize, pageIndex: request.PageIndex);
            response.PagedList = list;
            response.TotalPages = list.TotalPages;
            response.TotalCount = list.TotalCount;
            SetSuccess();
            return Code;
        }
    }
}
