using Rober.Action.Model.Account;
using Rober.Core.Action;
using Rober.IDAL.Repository;

namespace Rober.Action.Account
{
    public class UserListAction : ActionSupport<UserListRequest, UserListResponse>
    {
        #region 构造函数
        private readonly IUserRepository _userRepository;

        public UserListAction(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        #endregion

        public override int DoExecute(UserListRequest request, RequestHeader requestHeader,
            out UserListResponse response)
        {
            response = new UserListResponse();
            var list = _userRepository.GetList(userIds: request.UserIds, userName: request.UserName, nickName: request.NickName, publiched: request.Published, pageSize: request.PageSize, pageIndex: request.PageIndex);
            response.PagedList = list;
            response.TotalPages = list.TotalPages;
            response.TotalCount = list.TotalCount;
            SetSuccess();
            return Code;
        }
    }
}