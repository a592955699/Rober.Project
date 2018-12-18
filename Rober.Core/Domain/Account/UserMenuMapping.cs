using System;
using System.Collections.Generic;
using System.Text;

namespace Rober.Core.Domain.Account
{
    public class UserMenuMapping : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int MenuId { get; set; }
        public virtual Menu Menu { get; set; }
    }
}
