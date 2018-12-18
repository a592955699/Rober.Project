using Rober.Action.Model.Account;
using Rober.Action.Model.Common;
using Rober.Action.Model.Schedules;
using Rober.Core.Action;
using Rober.Core.Constants;
using Rober.Core.Http.Proxy;

namespace Rober.WebApp.Framework.Proxy
{
    public class ActionProxy: IActionProxy
    {
        #region 私有方法
        public IProxy Proxy { get; set; }
        public ActionProxy()
        {
            Proxy = ProxyFactory.GetDefaultProxy();
        }
        private bool IsSuccess(IProxy proxy)
        {
            return proxy != null && proxy.Code == ResponseCode.Success;
        }
        private bool DoAction<TIn, TOut>(string actionName, TIn request, out TOut response, out int code)
            where TIn : Request, new()
            where TOut : Response, new()
        {
            if (!actionName.EndsWith("Action"))
                actionName = $"{actionName}Action";
            if (Proxy != null)
            {
                var flag = Proxy.Action<TIn, TOut>(actionName, request, out response) && IsSuccess(Proxy);
                code = Proxy.Code;
                return flag;
            }
            response = default(TOut);
            code = ResponseCode.SystemError;
            return false;
        }
        #endregion

        #region 公共方法
        #region 公共方法 - 用户管理
        public bool Login(LoginRequest request, out LoginResponse response, out int responseCode)
        {
            return DoAction("Login", request, out response, out responseCode);
        }
        public bool Logout(LogoutRequest request, out LogoutResponse response, out int responseCode)
        {
            return DoAction("Logout", request, out response, out responseCode);
        }
        public bool UserMenuList(UserMenuListRequest request, out UserMenuListResponse response, out int responseCode)
        {
            return DoAction("UserMenuList", request, out response, out responseCode);
        }
        public bool RuleList(RuleListRequest request, out RuleListResponse response, out int responseCode)
        {
            return DoAction("RuleList", request, out response, out responseCode);
        }
        public bool RuleEdit(RuleEditRequest request, out RuleEditResponse response, out int responseCode)
        {
            return DoAction("RuleEdit", request, out response, out responseCode);
        }
        public bool RuleGet(RuleGetRequest request, out RuleGetResponse response, out int responseCode)
        {
            return DoAction("RuleGet", request, out response, out responseCode);
        }
        public bool RuleUserList(RuleUserListRequest request, out RuleUserListResponse response, out int responseCode)
        {
            return DoAction("RuleUserList", request, out response, out responseCode);
        }
        public bool RuleUserCreate(RuleUserCreateRequest request, out RuleUserCreateResponse response, out int responseCode)
        {
            return DoAction("RuleUserCreate", request, out response, out responseCode);
        }
        public bool RuleUserDestroy(RuleUserDestroyRequest request, out RuleUserDestroyResponse response, out int responseCode)
        {
            return DoAction("RuleUserDestroy", request, out response, out responseCode);
        }
        public bool RuleDestroy(RuleDestroyRequest request, out RuleDestroyResponse response, out int responseCode)
        {
            return DoAction("RuleDestroy", request, out response, out responseCode);
        }
        public bool MenuList(MenuListRequest request, out MenuListResponse response, out int responseCode)
        {
            return DoAction("MenuList", request, out response, out responseCode);
        }
        public bool UserOnline(UserOnlineRequest request, out UserOnlineResponse response, out int responseCode)
        {
            return DoAction("UserOnline", request, out response, out responseCode);
        }
        public bool UserList(UserListRequest request, out UserListResponse response, out int responseCode)
        {
            return DoAction("UserList", request, out response, out responseCode);
        }
        public bool UserDestroy(UserDestroyRequest request, out UserDestroyResponse response, out int responseCode)
        {
            return DoAction("UserDestroy", request, out response, out responseCode);
        }
        public bool UserGet(UserGetRequest request, out UserGetResponse response, out int responseCode)
        {
            return DoAction("UserGet", request, out response, out responseCode);
        }
        public bool UserEdit(UserEditRequest request, out UserEditResponse response, out int responseCode)
        {
            return DoAction("UserEdit", request, out response, out responseCode);
        }
        public bool DepartmentList(DepartmentListRequest request, out DepartmentListResponse response, out int responseCode)
        {
            return DoAction("DepartmentList", request, out response, out responseCode);
        }
        #endregion

        #region 公共方法 - GenericAttribute
        public bool SaveAttribute(SaveAttributeRequest request, out SaveAttributeResponse response, out int responseCode)
        {
            return DoAction("SaveAttribute", request, out response, out responseCode);
        }
        public bool GetAttribute(GetAttributeRequest request, out GetAttributeResponse response, out int responseCode)
        {
            return DoAction("GetAttribute", request, out response, out responseCode);
        }
        #endregion
        #endregion

        #region Schedule
        public bool ScheduleSubCategoryList(ScheduleSubCategoryListRequest request, out ScheduleSubCategoryListResponse response,
            out int responseCode)
        {
            return DoAction("ScheduleSubCategoryList", request, out response, out responseCode);
        }

        public bool ScheduleEdit(ScheduleEditRequest request, out ScheduleEditResponse response, out int responseCode)
        {
            return DoAction("ScheduleEdit", request, out response, out responseCode);
        }

        public bool ScheduleGet(ScheduleGetRequest request, out ScheduleGetResponse response, out int responseCode)
        {
            return DoAction("ScheduleGet", request, out response, out responseCode);
        }

        public bool ScheduleList(ScheduleListRequest request, out ScheduleListResponse response, out int responseCode)
        {
            return DoAction("ScheduleList", request, out response, out responseCode);
        }

        public bool ScheduleDestroy(ScheduleDestroyRequest request, out ScheduleDestroyResponse response, out int responseCode)
        {
            return DoAction("ScheduleDestroy", request, out response, out responseCode);
        }

        public bool ScheduleMyScheduler(ScheduleMySchedulerRequest request, out ScheduleMySchedulerResponse response,
            out int responseCode)
        {
            return DoAction("ScheduleMyScheduler", request, out response, out responseCode);
        }
        #endregion
    }
}
