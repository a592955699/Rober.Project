using System;
using System.Collections.Generic;
using Rober.Core.Action;
using Rober.Core.Domain.Account;

namespace Rober.Action.Model.Account
{
    [Serializable]
    public class LoginResponse : Response
    {
        /// <summary>
        ///  Session ID
        /// </summary>    
        public string SessionId { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        public List<Menu> Menus { get; set; }
        public User User { get; set; }
    }
}
