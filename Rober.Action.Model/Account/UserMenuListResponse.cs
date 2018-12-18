using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core;
using Rober.Core.Action;
using Rober.Core.Domain.Account;

namespace Rober.Action.Model.Account
{
    public class UserMenuListResponse : Response
    {
        public List<Menu> Menus { get; set; }
    }
}
