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
    public class ScheduleSubCategoryRepository : EfRepository<ScheduleSubCategory>, IScheduleSubCategoryRepository
    {
        #region 构造函数
        public ScheduleSubCategoryRepository(IEfUnitOfWork efUnitOfWork
            ) : base(efUnitOfWork)
        {
        }
        #endregion

        #region 公有方法
        public PagedList<ScheduleSubCategory> GetList(ScheduleCategory? scheduleCategory = null, int pageIndex = 0, int pageSize = Int32.MaxValue)
        {
            var query = Table;
            if (scheduleCategory.HasValue)
            {
                query = query.Where(x => x.ScheduleCategory == scheduleCategory.Value);
            }
            return new PagedList<ScheduleSubCategory>(query, pageIndex, pageSize);
        }
        #endregion
    }
}
