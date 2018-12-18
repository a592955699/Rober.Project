using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Action;

namespace Rober.Action.Model.Account
{
    public class RuleUserCreateRequest : SessionPageRequest
    {
        public int RuleId { get; set; }
        public int[] UserIds { get; set; }
    }
}
