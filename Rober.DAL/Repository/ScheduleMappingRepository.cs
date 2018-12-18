using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rober.Core;
using Rober.Core.Domain.Account;
using Rober.Core.Domain.Common;
using Rober.Core.Domain.Schedules;
using Rober.Core.Enums;
using Rober.IDAL;
using Rober.IDAL.Repository;


namespace Rober.DAL.Repository
{
    public class ScheduleMappingRepository : EfRepository<ScheduleMapping>, IScheduleMappingRepository
    {
        #region 构造函数
        public ScheduleMappingRepository(IEfUnitOfWork efUnitOfWork) : base(efUnitOfWork)
        {
        }
        #endregion

    }
}
