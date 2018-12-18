using System;
using System.Collections.Generic;
using System.Text;

namespace Rober.Core.Domain.Account
{
    public class RuleMenuMapping : BaseEntity
    {
        public int RuleId { get; set; }
        public Rule Rule { get; set; }

        public int MenuId { get; set; }
        public Menu Menu { get; set; }
    }
}
