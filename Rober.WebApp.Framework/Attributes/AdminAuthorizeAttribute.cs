using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Rober.Core;
using Rober.Core.Constants;
using Rober.Core.Infrastructure;
using Rober.Core.Log;
using Rober.Core.Session;
using Rober.WebApp.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rober.WebApp.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AdminAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public int MenuId { get; set; }

        public AdminAuthorizeAttribute(int menuId)
        {
            this.MenuId = menuId;
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext.Filters.Any(x => x.GetType() == typeof(AllowAnonymousFilter)))
            {
                return;
            }

            var authenticate = filterContext.HttpContext.AuthenticateAsync(SessionConstants.AuthenticationScheme);
            if (!authenticate.Result.Succeeded)
            {
                CreateForbiddenResult(filterContext);
                return;
            }

            var menusClaim = filterContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == SessionConstants.UserMenusScheme);

            var menus = JsonConvert.DeserializeObject<List<int>>(menusClaim.Value);
            if (menus != null && menus.Any(x => x == MenuId))
            {
                return;
            }
            CreateForbiddenResult(filterContext);

            if(Logger.Instance.IsDebugEnabled)
            {
                var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                Logger.Instance.WarnFormat("{0} {1} {2} author fail.", MenuId, filterContext.HttpContext.Request.Path, webHelper.GetCurrentIpAddress());
            }
        }

        private void CreateForbiddenResult(AuthorizationFilterContext filterContext)
        {
            if (IsAjax(filterContext))
            {
                BaseResult result = new BaseResult(ResponseCode.InvalidSession);
                filterContext.Result = new JsonResult(result);
            }
            else
            {
                //filterContext.Result = filterContext.Result = new UnauthorizedResult();
                filterContext.Result = new RedirectResult("/Account/Denied?url=" + filterContext.HttpContext.Request.Path);
            }
        }

        private bool IsAjax(AuthorizationFilterContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers.ContainsKey("x-requested-with") &&
                   filterContext.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";
        }
    }
}
