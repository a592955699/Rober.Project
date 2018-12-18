using Rober.Core.Constants;
using Rober.Core.Domain.Account;
using Rober.WebApp.Framework.Components;
using Rober.WebApp.Framework.Extensions;
using Rober.WebApp.Framework.Models.Areas.Settings;
using Microsoft.AspNetCore.Mvc;
using Rober.Core.Http.Context;

namespace Rober.WebApp.Areas.Admin.Components
{
    public class SettingModeViewComponent : JmViewComponent
    {
        public IViewComponentResult Invoke(string modeName = "settings-advanced-mode")
        {
            var model = new ModeModel
            {
                ModeName = modeName,
                Enabled = HttpContext.Session.Get<User>(SessionConstants.UserScheme).GetAttribute<bool>(modeName)
            };
            return View(model);
        }
    }
}