using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Action;
using Rober.Core.Domain.Account;

namespace Rober.Action.Model.Account
{
    public class UserDestroyRequest : SessionRequest
    {
        public List<User> Users { get; set; }
    }
}
