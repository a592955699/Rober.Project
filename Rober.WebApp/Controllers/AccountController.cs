using JM.Portal.App.Framework.Session;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rober.Action.Model.Account;
using Rober.Core;
using Rober.Core.Action;
using Rober.Core.Constants;
using Rober.Core.Domain.Account;
using Rober.Core.Session;
using Rober.WebApp.Framework.Controllers;
using Rober.WebApp.Framework.Infrastructure.Mapper;
using Rober.WebApp.Framework.Models.Account;
using Rober.WebApp.Framework.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rober.WebApp.Controllers
{
    public class AccountController: BaseController
    {
        #region 
        private readonly ISessionServer _sessionServer;
        private readonly IActionProxy _proxyAction;
        private IWebHelper _webHelper;
        public AccountController(IWebHelper webHelper, IActionProxy proxyAction, ISessionServer sessionServer)
        {
            this._proxyAction = proxyAction;
            this._webHelper = webHelper;
            this._sessionServer = sessionServer;
        }
        #endregion

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn(string returnUrl = null)
        {
            var model = new LoginModel()
            {
                ReturnUrl= returnUrl
            };
            return View(model);
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult SignIn(LoginModel model)
        {
            if (model == null) return BadRequest("A user name is required");

            //var cookie = HttpContext.Request.Cookies["VerificationCode"];
            //if (cookie == null || CommonHelper.MD5Encrypt(model.VerificationCode.ToUpper()) != cookie)
            //{
            //    ViewData["ResponseCode"] = "验证码不正确";
            //    return View(model);
            //}

           var request = model.ToRequest();
            request.Referrer = _webHelper.GetUrlReferrer();
            request.ClientIp = _webHelper.GetCurrentIpAddress();

            if (_proxyAction.Login(request, out LoginResponse response, out var responseCode))
            {
                #region 存储到公用 session 中
                var sessionValueObject = new SessionValueObject()
                {
                    User = response.User,
                    Menus = response.Menus?.Select(x => x.Id).ToList() ?? new List<int>()
                };
                var sessionValueObjectWrapper = new SessionValueObjectWrapper()
                {
                    SessionId = response.SessionId,
                    UserId = response.User.Id,
                    UserName = request.UserName,
                    Ip = request.ClientIp,
                    Token = response.Token,
                    ValueObject = sessionValueObject,
                    LastAccessTime = DateTime.Now
                };

                //移除历史session
                int count = 0;
                SessionValueObjectWrapper tempSession;
                do
                {
                    tempSession = _sessionServer.GetSessionWrapperByAcctId(request.UserName);
                    if (tempSession != null)
                        _sessionServer.RemoveSession(tempSession.SessionId);
                } while (tempSession != null && ++count < 10);

                //添加新session
                _sessionServer.AddSession(sessionValueObjectWrapper);
                SessionValueObject.Current = sessionValueObject;
                _sessionServer.CurrentSessionKey = sessionValueObjectWrapper.SessionId;
                #endregion

                this.Menus = response.Menus ?? new List<Menu>();

                #region 写入身份信息
                var user = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim(SessionConstants.SessionKeyScheme,response.SessionId),
                        new Claim(SessionConstants.TokenKeyScheme,response.Token),
                        new Claim(SessionConstants.UserMenusScheme,JsonConvert.SerializeObject(sessionValueObject.Menus))
                    }, CookieAuthenticationDefaults.AuthenticationScheme)
                );
                if (model.Remember)
                {
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user, new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.Now.Add(TimeSpan.FromDays(7)), // 有效时间
                        IsPersistent = true,
                        AllowRefresh = false
                    });
                }
                else
                {
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
                }
                #endregion

                if (string.IsNullOrWhiteSpace(model.ReturnUrl))
                    return new RedirectResult("~/Admin/Home/Index");//登录成功
                else
                    return new RedirectResult(model.ReturnUrl);//登录成功
            }
            else
            {
                ViewData["ResponseCode"] = responseCode;
            }
            return View(model);
        }

        public async Task<IActionResult> Signout()
        {
            var request = MessageFactory.CreateApiRequest<LogoutRequest>(HttpContext);

            await HttpContext.SignOutAsync();
            _sessionServer.RemoveSession(request.SessionHead.Sid);
            _sessionServer.CurrentSessionKey = null;
            SessionValueObject.Current = null;

            if (_proxyAction.Logout(request, out LogoutResponse response, out var responseCode))
            {
                //#TODO log
            }
            else
            {
                //#TODO log
            }
            return Redirect("/Account/SignIn");
        }
        [AllowAnonymous]
        public IActionResult Denied()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult VerificationCode()
        {
            var code = CommonHelper.GenerateRandom(4);
            var cookieValue = CommonHelper.MD5Encrypt(code.ToUpper());
            HttpContext.Response.Cookies.Append("VerificationCode", cookieValue);
            var bytes = CommonHelper.GenerateRandomGraphic(code,70,23);
            return File(bytes, "image/Png");
        }
    }
}
