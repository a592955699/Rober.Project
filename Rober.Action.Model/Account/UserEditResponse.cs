using System.Collections.Generic;
using Rober.Core.Action;
using Rober.Core.Domain.Account;

namespace Rober.Action.Model.Account
{
    public class UserEditResponse : Response
    {
        public User User { get; set; }
    }
}
