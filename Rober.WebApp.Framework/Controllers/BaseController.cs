using JM.Portal.App.Framework.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rober.Core.Constants;
using Rober.Core.Domain.Account;
using Rober.Core.Infrastructure;
using Rober.Core.Log;
using Rober.Core.Session;
using Rober.WebApp.Framework.Extensions;
using Rober.WebApp.Framework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
namespace Rober.WebApp.Framework.Controllers
{
    public class BaseController : Controller
    {

        #region 用户登陆信息
        private SessionValueObject _currentSessionValueObject;

        public SessionValueObject CurrentSessionValueObject
        {
            get
            {
                if (_currentSessionValueObject != null) return _currentSessionValueObject;

                _currentSessionValueObject = HttpContext.Session.Get<SessionValueObject>(SessionConstants.SessionValueObjectScheme);

                //session 中没有相关信息，去 SessionServer 重新加载 SessionValueObject
                if (_currentSessionValueObject==null)
                {
                    var sidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == SessionConstants.SessionKeyScheme);
                    var tidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == SessionConstants.TokenKeyScheme);
                    var sessionServer = EngineContext.Current.Resolve<ISessionServer>();
                    _currentSessionValueObject = sessionServer.GetSession<SessionValueObject>(sidClaim.Value);
                    //SessionServer 中有，写入session中
                    if (_currentSessionValueObject != null)
                    {
                        HttpContext.Session.Set<SessionValueObject>(SessionConstants.SessionValueObjectScheme, _currentSessionValueObject);
                    }
                }

                SetSessionValueObject(_currentSessionValueObject);
                return _currentSessionValueObject;
            }
            set
            {
                _currentSessionValueObject = value;
                SetSessionValueObject(_currentSessionValueObject);
            }
        }

        private void SetSessionValueObject(SessionValueObject currentSessionValueObject)
        {
            if (currentSessionValueObject != null)
            {
                HttpContext.Session.Set<SessionValueObject>(SessionConstants.SessionValueObjectScheme, currentSessionValueObject);
                HttpContext.Session.Set<User>(SessionConstants.UserScheme, currentSessionValueObject.User);
            }
            else
            {
                HttpContext.Session.Remove(SessionConstants.SessionValueObjectScheme);
                HttpContext.Session.Remove(SessionConstants.UserScheme);
                HttpContext.Session.Remove(SessionConstants.UserMenusScheme);
            }
        }

        public User CurrentUser => CurrentSessionValueObject?.User;

        public List<Menu> Menus
        {
            get
            {
                return HttpContext.Session.Get<List<Menu>>(SessionConstants.UserMenusScheme);
            }
            set
            {
                HttpContext.Session.Set<List<Menu>>(SessionConstants.UserMenusScheme, value);
            }
        }

        public void SetSession(SessionValueObjectWrapper sessionValueObjectWrapper, List<Menu> menus)
        {
            var sessionValueObject = (SessionValueObject)sessionValueObjectWrapper.ValueObject;
            this.CurrentSessionValueObject = sessionValueObject;
            this.Menus = menus;
            HttpContext.Response.Cookies.Append(SessionConstants.SessionKeyScheme, sessionValueObjectWrapper.SessionId);
            HttpContext.Response.Cookies.Append(SessionConstants.TokenKeyScheme, sessionValueObjectWrapper.Token);
        }

        public void RemoveSession()
        {
            this.CurrentSessionValueObject = null;
            HttpContext.Response.Cookies.Delete(SessionConstants.TokenKeyScheme);
            HttpContext.Response.Cookies.Delete(SessionConstants.SessionKeyScheme);
        }
        #endregion

        #region Notifications

        /// <summary>
        /// Display success notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Display warning notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void WarningNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Warning, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        /// <param name="logException">A value indicating whether exception should be logged</param>
        protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true, bool logException = true)
        {
            if (logException)
                LogException(exception);

            AddNotification(NotifyType.Error, exception.Message, persistForTheNextRequest);
        }

        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="exception">Exception</param>
        protected void LogException(Exception exception)
        {
            //var workContext = EngineContext.Current.Resolve<IWorkContext>();
            //var logger = EngineContext.Current.Resolve<ILogger>();

            //var customer = workContext.CurrentCustomer;
            //logger.Error(exception.Message, exception, customer);
            Logger.Instance.Error(exception);
        }

        /// <summary>
        /// Display notification
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        {
            var dataKey = $"nop.notifications.{type}";

            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }
        #endregion

        #region selected tab name
        /// <summary>
        /// Save selected tab name
        /// </summary>
        /// <param name="tabName">Tab name to save; empty to automatically detect it</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request. Pass null to ignore</param>
        protected virtual void SaveSelectedTabName(string tabName = "", bool persistForTheNextRequest = true)
        {
            //default root tab
            SaveSelectedTabName(tabName, "selected-tab-name", null, persistForTheNextRequest);
            //child tabs (usually used for localization)
            //Form is available for POST only
            if (Request.Method.Equals(WebRequestMethods.Http.Post, StringComparison.InvariantCultureIgnoreCase))
                foreach (var key in this.Request.Form.Keys)
                    if (key.StartsWith("selected-tab-name-", StringComparison.InvariantCultureIgnoreCase))
                        SaveSelectedTabName(null, key, key.Substring("selected-tab-name-".Length), persistForTheNextRequest);
        }

        /// <summary>
        /// Save selected tab name
        /// </summary>
        /// <param name="tabName">Tab name to save; empty to automatically detect it</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request. Pass null to ignore</param>
        /// <param name="formKey">Form key where selected tab name is stored</param>
        /// <param name="dataKeyPrefix">A prefix for child tab to process</param>
        protected virtual void SaveSelectedTabName(string tabName, string formKey, string dataKeyPrefix, bool persistForTheNextRequest)
        {
            //keep this method synchronized with
            //"GetSelectedTabName" method of \Nop.Web.Framework\Extensions\HtmlExtensions.cs
            if (string.IsNullOrEmpty(tabName))
            {
                tabName = Request.Form[formKey];
            }

            if (!string.IsNullOrEmpty(tabName))
            {
                var dataKey = "nop.selected-tab-name";
                if (!string.IsNullOrEmpty(dataKeyPrefix))
                    dataKey += $"-{dataKeyPrefix}";

                if (persistForTheNextRequest)
                {
                    TempData[dataKey] = tabName;
                }
                else
                {
                    ViewData[dataKey] = tabName;
                }
            }
        }
        #endregion
    }
}
