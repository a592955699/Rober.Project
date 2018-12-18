using Rober.Action.Model.Account;
using Rober.Core.Action;
using Rober.Core.Constants;
using Rober.IDAL.Repository;

namespace Rober.Action.Account
{
    public class UserGetAction : ActionSupport<UserGetRequest, UserGetResponse>
    {
        #region 构造函数
        private readonly IUserRepository _userRepository;
        public UserGetAction(
            IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        #endregion

        public override int DoExecute(UserGetRequest request, RequestHeader requestHeader, out UserGetResponse response)
        {
            response = new UserGetResponse
            {
                User = _userRepository.Get(request.Id)
            };
            if (response.User != null)
            {
                response.User.Departments = _userRepository.GetUseDepartments(request.Id);
                response.User.Roles = _userRepository.GetUseRules(request.Id);
                SetSuccess();
                return Code;
            }
            else
            {
                SetCode(ResponseCode.DataNotFind);
                return Code;
            }
        }
    }
}
