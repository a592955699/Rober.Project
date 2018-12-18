using Rober.Action.Model.Account;
using Rober.Core.Action;
using Rober.IDAL.Repository;

namespace Rober.Action.Account
{
    public class MenuListAction : ActionSupport<MenuListRequest, MenuListResponse>
    {
        #region 构造函数
        private readonly IMenuRepository _menuServer;
        public MenuListAction(IMenuRepository menuServer)
        {
            this._menuServer = menuServer;
        }
        #endregion

        public override int DoExecute(MenuListRequest request, RequestHeader requestHeader, out MenuListResponse response)
        {
            response = new MenuListResponse();
            var list = _menuServer.GetList(name: request.Name, publiched: request.Published, pageSize: request.PageSize, pageIndex: request.PageIndex);
            response.PagedList = list;
            SetSuccess();
            return Code;
        }
    }
}
