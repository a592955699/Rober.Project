using System.Collections.Generic;
using Rober.Core.Action;
using Rober.Core.Domain.Account;

namespace Rober.Action.Model.Account
{
    public class RuleEditResponse : Response
    {
        public Rule Rule { get; set; }
    }
}
