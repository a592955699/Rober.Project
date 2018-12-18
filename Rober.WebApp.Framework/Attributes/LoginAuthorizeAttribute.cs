using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Rober.Core;
using Rober.Core.Constants;
using Rober.Core.Infrastructure;
using Rober.Core.Log;
using System;
using System.Linq;

namespace JM.Portal.Web.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class LoginAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext.Filters.Any(x => x.GetType() == typeof(AllowAnonymousFilter))) { return; }

            var authenticate = filterContext.HttpContext.AuthenticateAsync(SessionConstants.AuthenticationScheme);
            if (authenticate.Result.Succeeded) { return; }
            
            CreateForbiddenResult(filterContext);
            if (Logger.Instance.IsDebugEnabled)
            {
                var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                Logger.Instance.WarnFormat("{0} {1} author fail.", filterContext.HttpContext.Request.Path, webHelper.GetCurrentIpAddress());
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
                //filterContext.Result = new UnauthorizedResult();
                filterContext.Result = new RedirectResult("/Account/SignIn?ReturnUrl=" + filterContext.HttpContext.Request.Path + filterContext.HttpContext.Request.QueryString);
            }
        }

        private bool IsAjax(AuthorizationFilterContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers.ContainsKey("x-requested-with") &&
                   filterContext.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";
        }
    }
}
