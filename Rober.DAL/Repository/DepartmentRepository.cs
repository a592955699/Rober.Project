using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rober.Core;
using Rober.Core.Domain.Account;
using Rober.IDAL;
using Rober.IDAL.Repository;


namespace Rober.DAL.Repository
{
    public class DepartmentRepository : EfRepository<Department>, IDepartmentRepository
    {
        #region 构造函数
        public DepartmentRepository(IEfUnitOfWork efUnitOfWork) : base(efUnitOfWork)
        {
        }
        #endregion

        #region 公有方法
        public PagedList<Department> GetList(int id = 0, string name = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = TableNoTracking;
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Name.Contains(name.TrimEnd()));
            }

            if (id != 0)
            {
                query = query.Where(x => x.Id == id);
            }
            return new PagedList<Department>(query, pageIndex, pageSize);
        }
        #endregion
    }
}
