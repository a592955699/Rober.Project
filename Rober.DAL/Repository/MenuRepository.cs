using Rober.Core;
using Rober.Core.Domain.Account;
using Rober.IDAL;
using Rober.IDAL.Repository;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Rober.DAL.Repository
{
    public class MenuRepository : EfRepository<Menu>, IMenuRepository
    {
        #region 构造函数
        public MenuRepository(IEfUnitOfWork efUnitOfWork) : base(efUnitOfWork)
        {
        }
        #endregion

        #region 公有方法
        public List<Menu> GetListByUserId(int userId)
        {
            DbParameter par = DbParameterFactory.GetInt32Parameter("UserId", userId);
            return this.UnitOfWork.Context.ExecuteStoredProcedureList<Menu>("up_GetMenuByUser", par).ToList();
        }
        public PagedList<Menu> GetList(string name = "", bool? publiched = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = Table;
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Name.Contains(name.TrimEnd()));
            }

            if (publiched.HasValue)
            {
                query = query.Where(x => x.Published == publiched);
            }
            return new PagedList<Menu>(query, pageIndex, pageSize);
        }

        #endregion
    }
}
