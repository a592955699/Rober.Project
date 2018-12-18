using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Action;
using Rober.Core.Domain.Account;

namespace Rober.Action.Model.Account
{
    public class RuleDestroyRequest : SessionRequest
    {
        public List<Rule> Rules { get; set; }
    }
}
