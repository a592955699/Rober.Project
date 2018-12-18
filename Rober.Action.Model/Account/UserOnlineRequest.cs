using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Action;

namespace Rober.Action.Model.Account
{
    public class UserOnlineRequest : SessionPageRequest
    {
        public string UserName { get; set; }
        public string NickName { get; set; }
    }
}
