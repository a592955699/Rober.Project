using System;
using System.Collections.Generic;
using System.Text;

namespace Rober.Core.Domain.Account
{
    public class UserDepartmentMapping : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}
