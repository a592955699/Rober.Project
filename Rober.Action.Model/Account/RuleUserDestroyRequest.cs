using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Action;
using Rober.Core.Domain.Account;

namespace Rober.Action.Model.Account
{
    public class RuleUserDestroyRequest : SessionRequest
    {
        public int RuleId { get; set; }
        public int UserId { get; set; }
    }
}
