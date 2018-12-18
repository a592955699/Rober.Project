using Rober.Core;
using Rober.Core.Domain.Account;
using Rober.Core.Domain.Common;
using Rober.Core.Domain.Schedules;
using Rober.Core.Enums;
using System;
using System.Collections.Generic;

namespace Rober.IDAL.Repository
{
    public partial interface IScheduleSubCategoryRepository : IRepository<ScheduleSubCategory>
    {
        PagedList<ScheduleSubCategory> GetList(ScheduleCategory? scheduleCategory = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
