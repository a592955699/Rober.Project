using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Action;

namespace Rober.Action.Model.Account
{
    public class RuleUserListRequest : SessionPageRequest
    {
        public int RuleId { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public bool? Published { get; set; }
    }
}
