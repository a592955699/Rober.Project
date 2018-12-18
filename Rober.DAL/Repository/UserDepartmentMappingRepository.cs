using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rober.Core;
using Rober.Core.Domain.Account;
using Rober.IDAL;
using Rober.IDAL.Repository;


namespace Rober.DAL.Repository
{
    public class UserDepartmentMappingRepository : EfRepository<UserDepartmentMapping>, IUserDepartmentMappingRepository
    {
        #region 构造函数
        //private readonly IUser
        public UserDepartmentMappingRepository(IEfUnitOfWork efUnitOfWork) : base(efUnitOfWork)
        {
        }
        #endregion
    }
}
