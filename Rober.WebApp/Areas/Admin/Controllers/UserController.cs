using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rober.Action.Model.Account;
using Rober.Core;
using Rober.Core.Action;
using Rober.Core.Domain.Account;
using Rober.Core.Log;
using Rober.WebApp.Framework.Controllers;
using Rober.WebApp.Framework.Infrastructure.Mapper;
using Rober.WebApp.Framework.Kendoui;
using Rober.WebApp.Framework.Models.Areas.Account;
using Rober.WebApp.Framework.MVC.Filters;
using Rober.WebApp.Framework.Proxy;

namespace Rober.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : BaseController
    {
        #region 构造函数
        private readonly IActionProxy _proxyAction;

        public UserController(IActionProxy proxyAction)
        {
            this._proxyAction = proxyAction;
        }
        #endregion

        #region 在线用户
        public IActionResult Online()
        {
            var model = new UserListModel();

            //"published" property
            //0 - all (according to "ShowHidden" parameter)
            //1 - published only
            //2 - unpublished only
            model.AvailablePublishedOptions.Add(new SelectListItem { Text = "All", Value = "0" });
            model.AvailablePublishedOptions.Add(new SelectListItem { Text = "PublishedOnly", Value = "1" });
            model.AvailablePublishedOptions.Add(new SelectListItem { Text = "UnpublishedOnly", Value = "2" });

            return View(model);
        }
        [HttpGet]
        public ActionResult<BaseResult> ReadOnline([FromQuery]UserListModel model, [FromQuery]DataSourceRequest command)
        {
            var result = new BaseResult();
            try
            {
                var request = MessageFactory.CreateApiRequest<UserOnlineRequest>(HttpContext);
                if (command != null)
                {
                    request.UserName = model.UserName;
                    request.NickName = model.NickName;
                    request.PageIndex = command.Page - 1;
                    request.PageSize = command.PageSize;
                }
                if (_proxyAction.UserOnline(request, out UserOnlineResponse response, out var responseCode))
                {
                    result.Data = new DataSourceResult()
                    {
                        Data = response.PagedList.Select(x => x.ToModel()),
                        Total = response.TotalCount
                    };
                    result.SetSuccess();
                }
                else
                {
                    result.Data = new DataSourceResult();
                    result.SetCode(responseCode);
                }
            }
            catch (Exception ex)
            {
                result.SetError();
                result.Message = ex.Message;
                result.Data = new DataSourceResult()
                {
                    Errors = "数据查询失败。"
                };
                Logger.Instance.Error(ex);
            }
            return result;
        }
        #endregion

        #region List
        public IActionResult Index()
        {
            var model = new UserListModel();

            //"published" property
            //0 - all (according to "ShowHidden" parameter)
            //1 - published only
            //2 - unpublished only
            model.AvailablePublishedOptions.Add(new SelectListItem { Text = "请选择", Value = "0" });
            model.AvailablePublishedOptions.Add(new SelectListItem { Text = "已发布", Value = "1" });
            model.AvailablePublishedOptions.Add(new SelectListItem { Text = "未发布", Value = "2" });

            return View(model);
        }

        [HttpGet]
        public ActionResult<BaseResult> Read([FromQuery]UserListModel model, [FromQuery]DataSourceRequest command)
        {
            var result = new BaseResult();
            try
            {
                var request = MessageFactory.CreateApiRequest<UserListRequest>(HttpContext);
                if (command != null)
                {
                    bool? pubished = null;
                    switch (model.PublishedId)
                    {
                        case 1:
                            pubished = true;
                            break;
                        case 2:
                            pubished = false;
                            break;
                    }

                    request.UserName = model.UserName;
                    request.NickName = model.NickName;
                    request.Published = pubished;
                    request.PageIndex = command.Page - 1;
                    request.PageSize = command.PageSize;
                }
                if (_proxyAction.UserList(request, out UserListResponse response, out var responseCode))
                {
                    result.Data = new DataSourceResult()
                    {
                        Data = response.PagedList.Select(x => x.ToModel()),
                        Total = response.TotalCount
                    };
                    result.SetSuccess();
                }
                else
                {
                    result.Data = new DataSourceResult();
                    result.SetCode(responseCode);
                }
            }
            catch (Exception ex)
            {
                result.SetError();
                result.Message = ex.Message;
                result.Data = new DataSourceResult()
                {
                    Errors = "数据查询失败。"
                };
                Logger.Instance.Error(ex);
            }
            return result;
        }
        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {
            var request = MessageFactory.CreateApiRequest<UserDestroyRequest>(HttpContext);
            request.Users = new List<User>()
            {
                new User() { Id = id }
            };
            if (_proxyAction.UserDestroy(request, out UserDestroyResponse response, out var responseCode))
            {
                SuccessNotification("The user has been deleted successfully.");
            }
            else
            {
                ErrorNotification("The user has been deleted unsuccessful.");
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteSelected(List<int> selectedIds)
        {
            var request = MessageFactory.CreateApiRequest<UserDestroyRequest>(HttpContext);
            request.Users = new List<User>();
            request.Users.AddRange(selectedIds.Select(x => new User() { Id = x }));
            if (_proxyAction.UserDestroy(request, out UserDestroyResponse response, out var responseCode))
            {
                SuccessNotification("The user has been deleted successfully.");
            }
            else
            {
                ErrorNotification("The user has been deleted unsuccessful.");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult<BaseResult> Destroy([FromBody]UserModel models)
        {
            var result = new BaseResult()
            {
                Data = new DataResult()
                {
                    Errors = "数据更新失败."
                }
            };
            try
            {
                var request = MessageFactory.CreateApiRequest<UserDestroyRequest>(HttpContext);
                request.Users = new List<User>()
                {
                    models.ToEntity()
                };
                if (_proxyAction.UserDestroy(request, out UserDestroyResponse response, out var responseCode))
                {
                    result.Data = new DataResult();
                    result.SetSuccess();
                }
                else
                {
                    result.SetCode(responseCode);
                }
            }
            catch (Exception ex)
            {
                result.Data = new DataResult()
                {
                    Errors = "数据更新失败"
                };
                result.SetError();
                result.Message = ex.Message;
                Logger.Instance.Error(ex);
            }
            return result;
        }
        #endregion

        #region create or edit
        public IActionResult Create()
        {
            var model = new UserModel();
            PrepareUserModel(model, null, false);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public IActionResult Create(UserModel model, bool continueEditing)
        {
            #region 数据非空验证
            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                ModelState.AddModelError("", "Username is null or empty.");
            }
            if (string.IsNullOrWhiteSpace(model.NickName))
            {
                ModelState.AddModelError("", "NickName is null or empty.");
            }
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("", "Password is null or empty.");
            }
            if (model.SelectedRoleIds == null || !model.SelectedRoleIds.Any())
            {
                ModelState.AddModelError("", "Role is null or empty.");
            }
            #endregion

            #region 数据有效性验证
            ////验证邮箱
            //if (!string.IsNullOrWhiteSpace(model.Email))
            //{
            //    var cust2 = _customerService.GetCustomerByEmail(model.Email);
            //    if (cust2 != null)
            //        ModelState.AddModelError("", "Email is already registered");
            //}
            ////验证用户名
            //if (!string.IsNullOrWhiteSpace(model.Username) & _customerSettings.UsernamesEnabled)
            //{
            //    var cust2 = _customerService.GetCustomerByUsername(model.Username);
            //    if (cust2 != null)
            //        ModelState.AddModelError("", "Username is already registered");
            //} 
            #endregion

            if (ModelState.IsValid)
            {
                var request = MessageFactory.CreateApiRequest<UserEditRequest>(HttpContext);
                request.User = model.ToEntity();
                if (model.SelectedRoleIds != null)
                    request.User.Roles = model.SelectedRoleIds.Select(x => new Rule() { Id = x }).ToList();
                if (model.SelectedDepartmentIds != null)
                    request.User.Departments = model.SelectedDepartmentIds.Select(x => new Department() { Id = x }).ToList();
                if (_proxyAction.UserEdit(request, out UserEditResponse response, out var responseCode))
                {
                    SuccessNotification("The user has been updated successfully.");
                    if (continueEditing)
                    {
                        //selected tab
                        SaveSelectedTabName();

                        return RedirectToAction("Edit", new { id = model.Id });
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    ErrorNotification("The user has been updated unsuccessful.");
                    PrepareUserModel(model, null, false);
                    return View(model);
                }
            }
            else
            {
                PrepareUserModel(model, null, false);
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            var request = MessageFactory.CreateApiRequest<UserGetRequest>(HttpContext);
            request.Id = id;

            if (_proxyAction.UserGet(request, out UserGetResponse response, out var responseCode))
            {
                var model = response.User.ToModel();
                PrepareUserModel(model, response.User, false);
                return View(model);
            }
            else
            {
                ErrorNotification("Can't find user by id.");
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public IActionResult Edit(UserModel model, bool continueEditing)
        {
            #region 数据非空验证
            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                ModelState.AddModelError("", "Username is null or empty.");
            }
            if (string.IsNullOrWhiteSpace(model.NickName))
            {
                ModelState.AddModelError("", "NickName is null or empty.");
            }
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("", "Password is null or empty.");
            }
            if (model.SelectedRoleIds == null || !model.SelectedRoleIds.Any())
            {
                ModelState.AddModelError("", "Role is null or empty.");
            }
            #endregion

            #region 数据有效性验证
            ////验证邮箱
            //if (!string.IsNullOrWhiteSpace(model.Email))
            //{
            //    var cust2 = _customerService.GetCustomerByEmail(model.Email);
            //    if (cust2 != null)
            //        ModelState.AddModelError("", "Email is already registered");
            //}
            ////验证用户名
            //if (!string.IsNullOrWhiteSpace(model.Username) & _customerSettings.UsernamesEnabled)
            //{
            //    var cust2 = _customerService.GetCustomerByUsername(model.Username);
            //    if (cust2 != null)
            //        ModelState.AddModelError("", "Username is already registered");
            //} 
            #endregion

            if (ModelState.IsValid)
            {
                var request = MessageFactory.CreateApiRequest<UserEditRequest>(HttpContext);
                request.User = model.ToEntity();
                if (model.SelectedRoleIds != null)
                    request.User.Roles = model.SelectedRoleIds.Select(x => new Rule() { Id = x }).ToList();
                if (model.SelectedDepartmentIds != null)
                    request.User.Departments = model.SelectedDepartmentIds.Select(x => new Department() { Id = x }).ToList();
                if (_proxyAction.UserEdit(request, out UserEditResponse response, out var responseCode))
                {
                    SuccessNotification("The user has been updated successfully.");
                    if (continueEditing)
                    {
                        //selected tab
                        SaveSelectedTabName();

                        return RedirectToAction("Edit", new { id = model.Id });
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    ErrorNotification("The user has been updated unsuccessful.");
                    PrepareUserModel(model, null, false);
                    return View(model);
                }
            }
            else
            {
                PrepareUserModel(model, null, false);
                return View(model);
            }
        }

        protected virtual void PrepareUserModel(UserModel model, User customer, bool excludeProperties)
        {
            if (customer != null)
            {
                model.Id = customer.Id;
                if (customer.Roles != null)
                    model.SelectedRoleIds = customer.Roles.Select(x => x.Id).ToList();
                if (customer.Departments != null)
                    model.SelectedDepartmentIds = customer.Departments.Select(x => x.Id).ToList();
            }

            #region rule下拉初始化
            var requestRule = MessageFactory.CreateApiRequest<RuleListRequest>(HttpContext);

            List<Rule> allRoles = new List<Rule>();
            if (_proxyAction.RuleList(requestRule, out RuleListResponse responseRule, out var responseCode))
            {
                allRoles = responseRule.PagedList.ToList();
            }
            foreach (var role in allRoles)
            {
                model.AvailableRoles.Add(new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Id.ToString(),
                    Selected = model.SelectedRoleIds.Contains(role.Id)
                });
            }
            #endregion

            #region Department下拉初始化
            var requestDepartment = MessageFactory.CreateApiRequest<DepartmentListRequest>(HttpContext);
            List<Department> allDepartments = new List<Department>();
            if (_proxyAction.DepartmentList(requestDepartment, out DepartmentListResponse responseDepartment, out var responseCode2))
            {
                allDepartments = responseDepartment.PagedList.ToList();
            }
            foreach (var department in allDepartments)
            {
                model.AvailableDepartments.Add(new SelectListItem
                {
                    Text = department.Name,
                    Value = department.Id.ToString(),
                    Selected = model.SelectedDepartmentIds.Contains(department.Id)
                });
            }
            #endregion
        }
        #endregion

        #region UserAddPopub
        public IActionResult UserAddPopub()
        {
            var model = new RuleModel.AddRuleUserModel();

            //"published" property
            //0 - all (according to "ShowHidden" parameter)
            //1 - published only
            //2 - unpublished only
            model.AvailablePublishedOptions.Add(new SelectListItem { Text = "请选择", Value = "0" });
            model.AvailablePublishedOptions.Add(new SelectListItem { Text = "已发布", Value = "1" });
            model.AvailablePublishedOptions.Add(new SelectListItem { Text = "未发布", Value = "2" });

            return View(model);
        }

        [HttpGet]
        public ActionResult<BaseResult> UserAddPopubList(RuleModel.AddRuleUserModel model, [FromQuery]DataSourceRequest command)
        {
            var result = new BaseResult();
            try
            {
                var request = MessageFactory.CreateApiRequest<RuleUserListRequest>(HttpContext);
                if (command != null)
                {
                    request.PageIndex = command.Page - 1;
                    request.PageSize = command.PageSize;
                }
                bool? pubished = null;
                switch (model.PublishedId)
                {
                    case 1:
                        pubished = true;
                        break;
                    case 2:
                        pubished = false;
                        break;
                }

                request.Published = pubished;
                request.UserName = model.UserName;

                if (_proxyAction.RuleUserList(request, out RuleUserListResponse response, out var responseCode))
                {
                    result.Data = new DataSourceResult()
                    {
                        Data = response.PagedList.Select(x => x.ToModel()),
                        Total = response.TotalCount
                    };
                    result.SetSuccess();
                }
                else
                {
                    result.Data = new DataSourceResult();
                    result.SetCode(responseCode);
                }
            }
            catch (Exception ex)
            {
                result.SetError();
                result.Message = ex.Message;
                result.Data = new DataSourceResult()
                {
                    Errors = "数据查询失败。"
                };
                Logger.Instance.Error(ex);
            }
            return result;
        }
        #endregion
    }
}