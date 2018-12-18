using Microsoft.AspNetCore.Mvc;
using Rober.Action.Model.Common;
using Rober.Core;
using Rober.Core.Action;
using Rober.Core.Log;
using Rober.WebApp.Framework.Controllers;
using Rober.WebApp.Framework.Kendoui;
using Rober.WebApp.Framework.Proxy;
using System;

namespace Rober.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PreferencesController : BaseController
    {
        #region 构造函数
        private readonly IActionProxy _proxyAction;

        public PreferencesController(IActionProxy proxyAction)
        {
            this._proxyAction = proxyAction;
        }
        #endregion
        public ActionResult<BaseResult> SavePreference(string name, bool value)
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
                var request = MessageFactory.CreateApiRequest<SaveAttributeRequest>(HttpContext);
                request.EntityId = CurrentUser.Id;
                request.KeyGroup = CurrentUser.GetType().Name;
                request.Key = name;
                request.Value = value;
                if (_proxyAction.SaveAttribute(request, out SaveAttributeResponse response, out var responseCode))
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
    }
}