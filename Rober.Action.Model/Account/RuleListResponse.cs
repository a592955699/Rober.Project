using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core;
using Rober.Core.Action;
using Rober.Core.Domain.Account;

namespace Rober.Action.Model.Account
{
    public class RuleListResponse : Response
    {
        public PagedList<Rule> PagedList { get; set; }
        //public int TotalPages { get; set; }
        //public int TotalCount { get; set; }
    }
}
