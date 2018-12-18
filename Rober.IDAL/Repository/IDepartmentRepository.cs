using Rober.Core;
using Rober.Core.Domain.Account;
using System;
using System.Collections.Generic;

namespace Rober.IDAL.Repository
{
    public partial interface IDepartmentRepository : IRepository<Department>
    {
        PagedList<Department> GetList(int id = 0, string name = "", int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
