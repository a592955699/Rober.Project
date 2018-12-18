using System;
using System.Collections.Generic;
using System.Text;

namespace Rober.Core.Domain.Account
{
    public class UserRuleMapping : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int RuleId { get; set; }
        public virtual Rule Rule { get; set; }
    }
}
