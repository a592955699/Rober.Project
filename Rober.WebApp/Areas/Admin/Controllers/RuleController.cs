using System;
using System.Collections.Generic;
using System.Linq;
using Rober.Core;
using Rober.Core.Action;
using Rober.Core.Domain.Account;
using Rober.Core.Log;
using Rober.WebApp.Framework.Controllers;
using Rober.WebApp.Framework.Infrastructure.Mapper;
using Rober.WebApp.Framework.Kendoui;
using Rober.WebApp.Framework.Models.Areas.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rober.WebApp.Framework.Proxy;
using Rober.Action.Model.Account;

namespace Rober.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RuleController : BaseController
    {
        #region 构造函数
        private readonly IActionProxy _proxyAction;

        public RuleController(IActionProxy proxyAction)
        {
            this._proxyAction = proxyAction;
        }
        #endregion

        #region List
        public IActionResult Index()
        {
            var model = new RuleListModel();

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
        public ActionResult<BaseResult> Read([FromQuery]RuleListModel model, [FromQuery]DataSourceRequest command)
        {
            var result = new BaseResult();
            try
            {
                var request = MessageFactory.CreateApiRequest<RuleListRequest>(HttpContext);
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

                    request.Id = model.Id;
                    request.Name = model.Name;
                    request.Published = pubished;
                    request.PageIndex = command.Page - 1;
                    request.PageSize = command.PageSize;
                }
                if (_proxyAction.RuleList(request, out RuleListResponse response, out var responseCode))
                {
                    result.Data = new DataSourceResult()
                    {
                        Data = response.PagedList.Select(x => x.ToModel()),
                        Total = response.PagedList.TotalCount
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

        [HttpGet]
        public ActionResult<BaseResult> ReadUsers(int id, [FromQuery]DataSourceRequest command)
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
                request.RuleId = id;
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

        #region Create or Edit
        public IActionResult Edit(int id)
        {
            var request = MessageFactory.CreateApiRequest<RuleGetRequest>(HttpContext);
            request.Id = id;

            if (_proxyAction.RuleGet(request, out RuleGetResponse response, out var responseCode))
            {
                return View(response.Rule.ToModel());
            }
            else
            {
                return View(new RuleModel());
            }
        }
        [HttpPost]
        public IActionResult Edit(RuleModel model)
        {
            if (ModelState.IsValid)
            {
                var request = MessageFactory.CreateApiRequest<RuleEditRequest>(HttpContext);
                request.Rule = model.ToEntity();

                if (_proxyAction.RuleEdit(request, out RuleEditResponse response, out var responseCode))
                {
                    SuccessNotification("The rule has been updated successfully.");
                    return View(response.Rule.ToModel());
                }
                else
                {
                    ErrorNotification("The rule has been updated unsuccessful.");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult<BaseResult> Create([FromBody]RuleModel model)
        {
            var result = new BaseResult()
            {
                Data = new DataResult()
                {
                    Errors = "数据更新失败"
                }
            };
            try
            {
                var request = MessageFactory.CreateApiRequest<RuleEditRequest>(HttpContext);
                request.Rule = model.ToEntity();
                if (_proxyAction.RuleEdit(request, out RuleEditResponse response, out var responseCode))
                {
                    result.Data = new DataResult()
                    {
                        Data = new List<RuleModel>() { response.Rule.ToModel() }
                    };
                    result.SetSuccess();
                }
                else
                {
                    result.SetCode(responseCode);
                }
            }
            catch (Exception ex)
            {
                result.SetError();
                result.Message = ex.Message;
                Logger.Instance.Error(ex);
            }
            return result;
        }

        [HttpPost]
        public ActionResult<BaseResult> Update([FromBody]RuleModel model)
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
                var request = MessageFactory.CreateApiRequest<RuleEditRequest>(HttpContext);
                request.Rule = model.ToEntity();
                if (_proxyAction.RuleEdit(request, out RuleEditResponse response, out var responseCode))
                {
                    result.Data = new DataResult()
                    {
                        Data = new List<RuleModel>() { response.Rule.ToModel() }
                    };
                    result.SetSuccess();
                }
                else
                {
                    result.SetCode(responseCode);
                }
            }
            catch (Exception ex)
            {
                result.SetError();
                result.Message = ex.Message;
                Logger.Instance.Error(ex);
            }
            return result;
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

        [HttpPost]
        [FormValueRequired("save")]
        public virtual IActionResult UserAddPopub(int ruleId,RuleModel.AddRuleUserModel model)
        {
            var result = new BaseResult()
            {
                Data = new DataResult()
                {
                    Errors = "数据更新失败"
                }
            };
            if (model.SelectedUserIds != null)
            {
                var request = MessageFactory.CreateApiRequest<RuleUserCreateRequest>(HttpContext);
                request.RuleId = ruleId;
                request.UserIds = model.SelectedUserIds;
                if (_proxyAction.RuleUserCreate(request, out RuleUserCreateResponse response, out var responseCode))
                {
                    result.Data = new DataResult();
                    result.SetSuccess();
                    SuccessNotification("The user has been added successfully.");
                }
                else
                {
                    ErrorNotification("The user has been added unsuccessfully.");
                    result.SetCode(responseCode);
                }
            }
            else
            {
                ErrorNotification("No user is selected.");
            }
            ViewBag.RefreshPage = true;
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

        #region Delete
        public IActionResult Delete(int id)
        {
            var request = MessageFactory.CreateApiRequest<RuleDestroyRequest>(HttpContext);
            request.Rules = new List<Rule>()
            {
                new Rule() { Id = id }
            };
            if (_proxyAction.RuleDestroy(request, out RuleDestroyResponse response, out var responseCode))
            {
                SuccessNotification("The rule has been deleted successfully.");
            }
            else
            {
                ErrorNotification("The user has been deleted unsuccessful.");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult<BaseResult> Destroy([FromBody]RuleModel models)
        {
            var result = new BaseResult() {
                Data = new DataResult()
                {
                    Errors = "数据更新失败."
                }
            };
            try
            {
                var request = MessageFactory.CreateApiRequest<RuleDestroyRequest>(HttpContext);
                request.Rules = new List<Rule>()
                {
                   models.ToEntity()
                };
                if (_proxyAction.RuleDestroy(request, out RuleDestroyResponse response, out var responseCode))
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
  
        public ActionResult<BaseResult> DestroyUser(int userId, int ruleId)
        {
            var result = new BaseResult() {
                Data = new DataResult()
                {
                    Errors = "数据更新失败."
                }
            };
            try
            {
                var request = MessageFactory.CreateApiRequest<RuleUserDestroyRequest>(HttpContext);
                request.UserId = userId;
                request.RuleId = ruleId;
                if (_proxyAction.RuleUserDestroy(request, out RuleUserDestroyResponse response, out var responseCode))
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
                result.SetError();
                result.Message = ex.Message;
                Logger.Instance.Error(ex);
            }
            return result;
        }
        #endregion
    }
}