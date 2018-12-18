using Rober.Core;
using Rober.Core.Domain.Account;
using System.Collections.Generic;

namespace Rober.IDAL.Repository
{
    public partial interface IMenuRepository : IRepository<Menu>
    {
        List<Menu> GetListByUserId(int userId);
        PagedList<Menu> GetList(string name = "", bool? publiched = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
