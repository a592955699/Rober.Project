using Rober.Core;
using Rober.Core.Domain.Account;
using Rober.Core.Domain.Common;
using Rober.IDAL;
using Rober.IDAL.Repository;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Rober.DAL.Repository
{
    public class CustomerFileRepository : EfRepository<CustomerFile>, ICustomerFileRepository
    {
        #region 构造函数
        public CustomerFileRepository(IEfUnitOfWork efUnitOfWork) : base(efUnitOfWork)
        {
        }
        #endregion

        #region 公有方法
        
        #endregion
    }
}
