using Rober.Action.Model.Account;
using Rober.Core.Action;
using Rober.Core.Constants;
using Rober.Core.Domain.Account;
using Rober.IDAL.Repository;

namespace Rober.Action.Account
{
    public class UserEditAction : ActionSupport<UserEditRequest, UserEditResponse>
    {
        #region 构造函数
        private readonly IUserRepository _userRepository;
        private readonly IUserDepartmentMappingRepository _userDepartmentMappingRepository;
        private readonly IUserRuleMappingRepository _userRuleMappingRepository;
        public UserEditAction(IUserRepository userRepository
            , IUserDepartmentMappingRepository userDepartmentMappingRepository
            , IUserRuleMappingRepository userRuleMappingRepository
            )
        {
            _userRepository = userRepository;
            _userDepartmentMappingRepository = userDepartmentMappingRepository;
            _userRuleMappingRepository = userRuleMappingRepository;
        }
        #endregion

        public override int DoExecute(UserEditRequest request, RequestHeader requestHeader, out UserEditResponse response)
        {
            response = new UserEditResponse();
            if (request.User.Id == 0)
            {
                #region 新增
                //#TODO 需要将这些逻辑转移到 JM.Server 端去实现
                if (_userRepository.InsertAll(request.User))
                {
                    response.User = request.User;
                    SetSuccess();
                    return Code;
                }
                #endregion
            }
            else
            {
                #region 修改
                _userRepository.UpdateAll(request.User);
                response.User = request.User;
                SetSuccess();
                return Code;
                #endregion
            }
            SetCode(ResponseCode.DataChangeFail);
            return Code;
        }

    }
}
