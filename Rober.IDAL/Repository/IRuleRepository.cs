using Rober.Core;
using Rober.Core.Domain.Account;
using System;
using System.Collections.Generic;

namespace Rober.IDAL.Repository
{
    public partial interface IRuleRepository : IRepository<Rule>
    {
        PagedList<Rule> GetList(int id = 0, string name = "", bool? publiched = null, int pageIndex = 0, int pageSize = int.MaxValue);
        bool RuleUserCreate(int ruleId, int[] userIds);
    }
}
