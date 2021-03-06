﻿using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core;
using Rober.Core.Action;
using Rober.Core.Domain.Account;

namespace Rober.Action.Model.Account
{
    public class RuleUserListResponse : Response
    {
        public PagedList<User> PagedList { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
