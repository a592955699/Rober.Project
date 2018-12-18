using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rober.Core.Domain.Account
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }

        public string Remark { get; set; } = string.Empty;
        //public IEnumerable<UserDepartmentMapping> UserDepartmentMappings { get; set; }
    }
}
