using Rober.Action.Model.Account;
using Rober.Core.Action;
using Rober.Core.Constants;
using Rober.Core.Domain.Account;
using Rober.IDAL.Repository;
using System.Linq;

namespace Rober.Action.Account
{
    public class UserDestroyAction : ActionSupport<UserDestroyRequest, UserDestroyResponse>
    {
        #region 构造函数

        private readonly IUserRepository _userRepository;
        public UserDestroyAction(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        #endregion

        public override int DoExecute(UserDestroyRequest request, RequestHeader requestHeader, out UserDestroyResponse response)
        {
            response = new UserDestroyResponse();
            //TODO 事物控制
            //删除角色关联
            _userRepository.DestroyUserRuleMapping(request.Users.Select(x => x.Id));
            //删除部门关联
            _userRepository.DestroyUserDepartmentMapping(request.Users.Select(x => x.Id));
            //删除用户
            if (_userRepository.Delete(request.Users))
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