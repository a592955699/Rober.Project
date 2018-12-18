using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rober.Action.Model.Account;
using Rober.Action.Model.Schedules;
using Rober.Core;
using Rober.Core.Action;
using Rober.Core.Domain.Account;
using Rober.Core.Domain.Schedules;
using Rober.Core.Enums;
using Rober.Core.Log;
using Rober.WebApp.Framework.Controllers;
using Rober.WebApp.Framework.Infrastructure.Mapper;
using Rober.WebApp.Framework.Kendoui;
using Rober.WebApp.Framework.Models.Areas.Schedules;
using Rober.WebApp.Framework.MVC.Filters;
using Rober.WebApp.Framework.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rober.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ScheduleController : BaseController
    {
        #region 构造函数
        private readonly IActionProxy _proxyAction;

        public ScheduleController(IActionProxy proxyAction)
        {
            this._proxyAction = proxyAction;
        }
        #endregion

        #region list
        public IActionResult Index(int scheduleCategory)
        {
            var scheduleCategoryEnum = (ScheduleCategory)Enum.Parse(typeof(ScheduleCategory), scheduleCategory.ToString());
            ScheduleListModel model = new ScheduleListModel()
            {
                ScheduleCategory= scheduleCategoryEnum
            };
            PrepareScheduleListModel(model);
            return View(model);
        }
        [HttpGet]
        public ActionResult<BaseResult> Read([FromQuery]ScheduleListModel model, [FromQuery]DataSourceRequest command)
        {
            var result = new BaseResult();
            try
            {
                var request = MessageFactory.CreateApiRequest<ScheduleListRequest>(HttpContext);
                if (command != null)
                {
                    request.Title = model.Title;
                    request.ScheduleCategory = model.ScheduleCategory;
                    request.StatusId = model.StatusId;
                    request.SubCategoryId = model.SubCategoryId;
                    request.PageIndex = command.Page - 1;
                    request.PageSize = command.PageSize;
                }
                if (_proxyAction.ScheduleList(request, out ScheduleListResponse response, out var responseCode))
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
        protected virtual void PrepareScheduleListModel(ScheduleListModel model)
        {
            #region 类型状态下拉框
            var requestDepartment = MessageFactory.CreateApiRequest<ScheduleSubCategoryListRequest>(HttpContext);
            requestDepartment.ScheduleCategory = model.ScheduleCategory;
            List<ScheduleSubCategory> allSubCategories = new List<ScheduleSubCategory>();
            if (_proxyAction.ScheduleSubCategoryList(requestDepartment, out ScheduleSubCategoryListResponse responseDepartment, out var responseCode2))
            {
                allSubCategories = responseDepartment.PagedList.ToList();
            }
            model.AvailableSubScheduleCategorys.Add(new SelectListItem
            {
                Text = "请选择",
                Value = "0"
            });
            model.AvailableStatus.Add(new SelectListItem
            {
                Text = "请选择",
                Value = "0"
            });
            foreach (var item in allSubCategories.Where(x => x.IsCategory))
            {
                model.AvailableSubScheduleCategorys.Add(new SelectListItem
                {
                    Text = item.Title,
                    Value = item.Id.ToString(),
                    Selected = model.SubCategoryId == item.Id
                });
            }
            foreach (var item in allSubCategories.Where(x => !x.IsCategory))
            {
                model.AvailableStatus.Add(new SelectListItem
                {
                    Text = item.Title,
                    Value = item.Id.ToString(),
                    Selected = model.StatusId == item.Id
                });
            }
            #endregion
        }
        #endregion

        #region create or edit
        public IActionResult Create(int scheduleCategory)
        {
            var scheduleCategoryEnum =(ScheduleCategory)Enum.Parse(typeof(ScheduleCategory), scheduleCategory.ToString());
            var model = new ScheduleModel()
            {
                ScheduleCategory = scheduleCategoryEnum
            };
            PrepareScheduleModel(model, null, false);
            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public IActionResult Create(ScheduleModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var request = MessageFactory.CreateApiRequest<ScheduleEditRequest>(HttpContext);
                request.Schedule = model.ToEntity();
                request.Schedule.CreatedTime=DateTime.UtcNow;
                request.Schedule.CreatedUserId = CurrentUser.Id;
                if (model.SelectedUserIds != null)
                    request.Schedule.Users = model.SelectedUserIds.Select(x => new User() { Id = x }).ToList();
               
                if (_proxyAction.ScheduleEdit(request, out ScheduleEditResponse response, out var responseCode))
                {
                    SuccessNotification("The user has been updated successfully.");
                    if (continueEditing)
                    {
                        //selected tab
                        SaveSelectedTabName();

                        return RedirectToAction("Edit", new { ScheduleCategory = (int)model.ScheduleCategory,Id = response.Schedule.Id });
                    }

                    return RedirectToAction("Index", new { ScheduleCategory = (int)model.ScheduleCategory });
                }
                else
                {
                    ErrorNotification("The user has been updated unsuccessful.");
                    PrepareScheduleModel(model, null, false);
                    return View(model);
                }
            }
            else
            {
                PrepareScheduleModel(model, null, false);
                return View(model);
            }
        }

        public IActionResult Edit(int scheduleCategory,int id)
        {
            var request = MessageFactory.CreateApiRequest<ScheduleGetRequest>(HttpContext);
            request.Id = id;

            if (_proxyAction.ScheduleGet(request, out ScheduleGetResponse response, out var responseCode))
            {
                var model = response.Schedule.ToModel();
                PrepareScheduleModel(model, response.Schedule, false);
                return View(model);
            }
            else
            {
                ErrorNotification("Can't find Schedule by id.");
                return RedirectToAction("Index", new { ScheduleCategory = scheduleCategory });
            }
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public IActionResult Edit(ScheduleModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var request = MessageFactory.CreateApiRequest<ScheduleEditRequest>(HttpContext);
                request.Schedule = model.ToEntity();
                if (model.SelectedUserIds != null)
                    request.Schedule.Users = model.SelectedUserIds.Select(x => new User() { Id = x }).ToList();
                if (_proxyAction.ScheduleEdit(request, out ScheduleEditResponse response, out var responseCode))
                {
                    SuccessNotification("数据更新成功！");
                    if (continueEditing)
                    {
                        //selected tab
                        SaveSelectedTabName();

                        return RedirectToAction("Edit", new { ScheduleCategory = (int)model.ScheduleCategory, id = model.Id });
                    }

                    return RedirectToAction("Index", new { ScheduleCategory = (int)model.ScheduleCategory });
                }
                else
                {
                    ErrorNotification("数据更新失败！");
                    PrepareScheduleModel(model, null, false);
                    return View(model);
                }
            }
            else
            {
                PrepareScheduleModel(model, null, false);
                return View(model);
            }
        }
        protected virtual void PrepareScheduleModel(ScheduleModel model, Schedule entity, bool excludeProperties)
        {
            if (entity != null)
            {
                model.Id = entity.Id;
                if (entity.Users != null)
                {
                    model.SelectedUserIds = entity.Users.Select(x => x.Id).ToList();
                    model.AvailableUsers = entity.Users.Select(x => new SelectListItem()
                    {
                        Text = x.UserName,
                        Value = x.Id.ToString(),
                        Selected = true
                    }).ToList();
                }
            }

            #region 加载接收人下拉选择
            if (model.SelectedUserIds != null && model.SelectedUserIds.Any() && (entity == null || entity.Users == null || !entity.Users.Any()))
            {
                var request = MessageFactory.CreateApiRequest<UserListRequest>(HttpContext);
                request.UserIds = model.SelectedUserIds;
                List<User> allUser = new List<User>();
                if (_proxyAction.UserList(request, out UserListResponse response, out var responseCode3))
                {
                    allUser = response.PagedList.ToList();
                }
                foreach (var item in allUser)
                {
                    model.AvailableUsers.Add(new SelectListItem
                    {
                        Text = item.UserName,
                        Value = item.Id.ToString(),
                        Selected = true
                    });
                }
            } 
            #endregion

            #region 类型状态下拉框
            var requestDepartment = MessageFactory.CreateApiRequest<ScheduleSubCategoryListRequest>(HttpContext);
            requestDepartment.ScheduleCategory = model.ScheduleCategory;
            List<ScheduleSubCategory> allSubCategories = new List<ScheduleSubCategory>();
            if (_proxyAction.ScheduleSubCategoryList(requestDepartment, out ScheduleSubCategoryListResponse responseDepartment, out var responseCode2))
            {
                allSubCategories = responseDepartment.PagedList.ToList();
            }
            foreach (var item in allSubCategories.Where(x=>x.IsCategory))
            {
                model.AvailableSubScheduleCategorys.Add(new SelectListItem
                {
                    Text = item.Title,
                    Value = item.Id.ToString(),
                    Selected = model.SubCategoryId == item.Id
                });
            }
            foreach (var item in allSubCategories.Where(x => !x.IsCategory))
            {
                model.AvailableStatus.Add(new SelectListItem
                {
                    Text = item.Title,
                    Value = item.Id.ToString(),
                    Selected = model.StatusId == item.Id
                });
            }
            #endregion
        }
        #endregion

        #region Delete
        public IActionResult Delete(int scheduleCategory, int id)
        {
            var request = MessageFactory.CreateApiRequest<ScheduleDestroyRequest>(HttpContext);
            request.Schedules = new List<Schedule>()
            {
                new Schedule() { Id = id }
            };
            if (_proxyAction.ScheduleDestroy(request, out ScheduleDestroyResponse response, out var responseCode))
            {
                SuccessNotification("The Schedule has been deleted successfully.");
            }
            else
            {
                ErrorNotification("The Schedule has been deleted unsuccessful.");
            }
            return RedirectToAction("Index",new{ ScheduleCategory = scheduleCategory });
        }

        public IActionResult DeleteSelected(int scheduleCategory, List<int> selectedIds)
        {
            var request = MessageFactory.CreateApiRequest<ScheduleDestroyRequest>(HttpContext);
            request.Schedules = new List<Schedule>();
            request.Schedules.AddRange(selectedIds.Select(x => new Schedule() { Id = x }));
            if (_proxyAction.ScheduleDestroy(request, out ScheduleDestroyResponse response, out var responseCode))
            {
                SuccessNotification("The Schedule has been deleted successfully.");
            }
            else
            {
                ErrorNotification("The Schedule has been deleted unsuccessful.");
            }
            return RedirectToAction("Index", new { ScheduleCategory = scheduleCategory });
        }
        [HttpPost]
        public ActionResult<BaseResult> Destroy([FromBody]ScheduleModel models)
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
                var request = MessageFactory.CreateApiRequest<ScheduleDestroyRequest>(HttpContext);
                request.Schedules = new List<Schedule>()
                {
                    models.ToEntity()
                };
                if (_proxyAction.ScheduleDestroy(request, out ScheduleDestroyResponse response, out var responseCode))
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

        #region MyScheduler
        public IActionResult MyScheduler()
        {
            return View();
        }
        [HttpGet]
        public ActionResult<BaseResult> MySchedulerRead([FromQuery]string view, [FromQuery]string date)
        {
            var result = new BaseResult();
            try
            {
                #region start end 赋值
                DateTime day = Convert.ToDateTime(date);
                DateTime start = day;
                DateTime end = day.AddDays(1);

                switch (view)
                {
                    case "day":
                        start = day;
                        end = day.AddDays(1);
                        break;
                    case "week":
                        start = day.AddDays(1 - Convert.ToInt32(day.DayOfWeek.ToString("d")));  //本周周一  
                        end = start.AddDays(7);  //本周周日  
                        break;
                    case "month":
                        start = day.AddDays(1 - day.Day);  //本月月初  
                        end = start.AddMonths(1);  //本月月末  
                        break;
                    case "agenda":

                        break;
                    case "timeline":

                        break;
                }
                #endregion

                var request = MessageFactory.CreateApiRequest<ScheduleMySchedulerRequest>(HttpContext);
                request.Start = start;
                request.End = end;
                if (_proxyAction.ScheduleMyScheduler(request, out ScheduleMySchedulerResponse response, out var responseCode))
                {
                    result.Data = new DataSourceResult()
                    {
                        Data = response.List.Select(x => x.ToModel()),
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