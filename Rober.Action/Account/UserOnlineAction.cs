using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Action;
using Rober.Core.Session;
using Rober.Action.Model.Account;
using Rober.IDAL.Repository;

namespace Rober.Action.Account
{
    public class UserOnlineAction : ActionSupport<UserOnlineRequest, UserOnlineResponse>
    {
        #region 构造函数

        private readonly IUserRepository _userRepository;
        private readonly ISessionServer _sessionServer;

        public UserOnlineAction(IUserRepository userRepository, ISessionServer sessionServer)
        {
            this._userRepository = userRepository;
            this._sessionServer = sessionServer;
        }

        #endregion

        public override int DoExecute(UserOnlineRequest request, RequestHeader requestHeader,
            out UserOnlineResponse response)
        {
            var userIds = _sessionServer.GetSessionUserIds();
            response = new UserOnlineResponse();
            var list = _userRepository.GetOnlineList(userIds: userIds, userName: request.UserName, nickName: request.NickName, pageSize: request.PageSize, pageIndex: request.PageIndex);
            response.PagedList = list;
            response.TotalPages = list.TotalPages;
            response.TotalCount = list.TotalCount;
            SetSuccess();
            return Code;
        }
    }
}