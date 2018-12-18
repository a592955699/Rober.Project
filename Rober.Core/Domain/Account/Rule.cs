using System;
using System.Collections.Generic;
using System.Text;

namespace Rober.Core.Domain.Account
{
    public class Rule : BaseEntity
    {
        public string Name { get; set; }
        public string Remark { get; set; } = string.Empty;

        public bool Published { get; set; } = true;
        //public List<RuleMenuMapping> RuleMenuMappings { get; set; }
    }
}
