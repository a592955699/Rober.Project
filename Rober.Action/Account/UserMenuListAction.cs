using Rober.Core.Action;
using Rober.Action.Model.Account;
using Rober.IDAL.Repository;
using Rober.Core.Session;
using System.Collections.Generic;

namespace Rober.Action.Account
{
    public class UserMenuListAction : ActionSupport<UserMenuListRequest, UserMenuListResponse>
    {
        #region 构造函数
        private readonly IMenuRepository _menuRepository;
        public UserMenuListAction(IMenuRepository menuRepository)
        {
            this._menuRepository = menuRepository;
        }
        #endregion

        public override int DoExecute(UserMenuListRequest request, RequestHeader requestHeader, out UserMenuListResponse response)
        {
            response = new UserMenuListResponse();
            if (SessionValueObject.Current != null)
            {
                var menus = _menuRepository.GetListByUserId(SessionValueObject.Current.User.Id);
                response.Menus = menus;
            }
            else
            {
                response.Menus = new List<Core.Domain.Account.Menu>();
            }
            SetSuccess();
            return Code;
        }
    }
}
