using System;
using Rober.Core.Action;
using Rober.Core.Constants;
using Rober.Action.Model.Account;
using Rober.IDAL.Repository;
using Rober.ServerApp.Framework.Session;
using Rober.Core.Domain.Account;
using Rober.Core.Session;
using System.Collections.Generic;
using System.Linq;

namespace Rober.Action.Account
{
    public class LoginAction : ActionSupport<LoginRequest, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly ISessionServer _sessionServer;
        public LoginAction(IUserRepository userRepository, IMenuRepository menuRepository, ISessionServer sessionServer)
        {
            this._userRepository = userRepository;
            this._menuRepository = menuRepository;
            this._sessionServer = sessionServer;
        }

        public override int DoExecute(LoginRequest request, RequestHeader requestHeader, out LoginResponse response)
        {
            response = new LoginResponse();

            var user = _userRepository.GetByUserName(request.UserName);
            if (user == null)
            {
                SetCode(ResponseCode.InvalidAccount);
                return Code;
            }

            if (user.Password != request.PassWord)
            {
                //修改错误次数，修改错误时间

                user.ErrorCount++;
                user.ErrorTime = DateTime.UtcNow;
                _userRepository.Update(user);
                //_userRepository.SaveChanges();
                SetCode(ResponseCode.InvalidPassowrd);
                return Code;
            }

            //检查错误次数以及错误时间，判断是否允许登陆
            if (user.ErrorTime.HasValue && user.ErrorTime.Value > DateTime.UtcNow.AddMinutes(-5) &&
                user.ErrorCount >= 5)
            {
                user.ErrorCount++;
                user.ErrorTime = DateTime.UtcNow;
                _userRepository.Update(user);
                //_userRepository.SaveChanges();
                return Code;
            }

            user.LoginTime = DateTime.UtcNow;
            user.LoginIp = request.ClientIp;
            user.ErrorTime = null;
            user.ErrorCount = 0;
            _userRepository.Update(user);
            //_userRepository.SaveChanges();

            //#TODO 登陆成功，写session
            var menus = _menuRepository.GetListByUserId(user.Id);
            response.SessionId = Guid.NewGuid().ToString();
            response.Token = Guid.NewGuid().ToString();
            response.Menus = menus;
            response.User = user;

            KickOff(user, response.SessionId, response.Token, request.ClientIp, menus, new SessionCreatedParameter()
            {
                ClientIp = request.ClientIp,
                Referrer = request.Referrer,
            });

            SetSuccess();
            return Code;
        }

        /// <summary>
        /// 1、删除之前的session
        /// 2、写入新session
        /// </summary>
        /// <param name="user"></param>
        /// <param name="sessionId"></param>
        /// <param name="token"></param>
        /// <param name="ip"></param>
        /// <param name="menus"></param>
        /// <param name="parameter"></param>
        private void KickOff(User user, string sessionId, string token, string ip, List<Menu> menus, SessionCreatedParameter parameter)
        {
            user.UserName = user.UserName.ToUpper();
            var sessionWrapper = _sessionServer.GetSessionWrapperByAcctId(user.UserName);
            //移除历史session
            int count = 0;
            while (sessionWrapper != null && ++count < 10)
            {
                _sessionServer.RemoveSession(sessionWrapper.SessionId);
                sessionWrapper = _sessionServer.GetSessionWrapperByAcctId(user.UserName);

                //#TODO 推送踢人事件
            }

            var sessionValueObject = new SessionValueObject()
            {
                Menus = menus == null ? new List<int>() : menus.Select(x => x.Id).ToList(),
                User = user,
            };
            var sessionValueObjectWrapper = new SessionValueObjectWrapper()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Ip = ip,
                SessionId = sessionId,
                Token = token,
                LastAccessTime = DateTime.Now,
                ValueObject = sessionValueObject
            };
            //添加新session
            _sessionServer.AddSession(sessionValueObjectWrapper, parameter);
            SessionValueObject.Current = sessionValueObject;
            _sessionServer.CurrentSessionKey = sessionValueObjectWrapper.SessionId;
        }
    }
}
