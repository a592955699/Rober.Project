using System;
using Rober.Core.Action;

namespace Rober.Action.Model.Account
{
    [Serializable]
    public class LoginRequest : SessionRequest
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Referrer { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string ClientIp { get; set; }
    }
}
