using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Action;

namespace Rober.Action.Model.Account
{
    public class RuleListRequest : SessionPageRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Published { get; set; }
    }
}
