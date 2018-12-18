using Rober.Core.Http.Proxy;
using Rober.Action.Model.Account;
using System;
using System.Collections.Generic;
using System.Text;
using Rober.Action.Model.Common;
using Rober.Action.Model.Schedules;

namespace Rober.WebApp.Framework.Proxy
{
    public interface IActionProxy
    {
        IProxy Proxy { get; set; }
        #region 用户管理
        bool Login(LoginRequest request, out LoginResponse response, out int responseCode);
        bool Logout(LogoutRequest request, out LogoutResponse response, out int responseCode);
        bool UserMenuList(UserMenuListRequest request, out UserMenuListResponse response, out int responseCode);
        bool RuleList(RuleListRequest request, out RuleListResponse response, out int responseCode);
        bool RuleEdit(RuleEditRequest request, out RuleEditResponse response, out int responseCode);
        bool UserEdit(UserEditRequest request, out UserEditResponse response, out int responseCode);
        bool RuleDestroy(RuleDestroyRequest request, out RuleDestroyResponse response, out int responseCode);
        bool RuleGet(RuleGetRequest request, out RuleGetResponse response, out int responseCode);
        bool RuleUserList(RuleUserListRequest request, out RuleUserListResponse response, out int responseCode);
        bool RuleUserCreate(RuleUserCreateRequest request, out RuleUserCreateResponse response, out int responseCode);
        bool RuleUserDestroy(RuleUserDestroyRequest request, out RuleUserDestroyResponse response, out int responseCode);
        bool MenuList(MenuListRequest request, out MenuListResponse response, out int responseCode);
        bool UserOnline(UserOnlineRequest request, out UserOnlineResponse response, out int responseCode);
        bool UserList(UserListRequest request, out UserListResponse response, out int responseCode);
        bool UserDestroy(UserDestroyRequest request, out UserDestroyResponse response, out int responseCode);
        bool UserGet(UserGetRequest request, out UserGetResponse response, out int responseCode);
        #endregion
        #region GenericAttribute
        bool SaveAttribute(SaveAttributeRequest request, out SaveAttributeResponse response, out int responseCode);
        bool GetAttribute(GetAttributeRequest request, out GetAttributeResponse response, out int responseCode);
        bool DepartmentList(DepartmentListRequest request, out DepartmentListResponse response, out int responseCode);
        #endregion

        #region Schedule
        bool ScheduleSubCategoryList(ScheduleSubCategoryListRequest request, out ScheduleSubCategoryListResponse response, out int responseCode);
        bool ScheduleEdit(ScheduleEditRequest request, out ScheduleEditResponse response, out int responseCode);
        bool ScheduleGet(ScheduleGetRequest request, out ScheduleGetResponse response, out int responseCode);
        bool ScheduleList(ScheduleListRequest request, out ScheduleListResponse response, out int responseCode);
        bool ScheduleDestroy(ScheduleDestroyRequest request, out ScheduleDestroyResponse response, out int responseCode);
        bool ScheduleMyScheduler(ScheduleMySchedulerRequest request, out ScheduleMySchedulerResponse response, out int responseCode);
        #endregion
    }
}
