using Rober.Core;
using Rober.Core.Domain.Account;
using Rober.Core.Domain.Common;
using Rober.Core.Domain.Schedules;
using Rober.Core.Enums;
using System;
using System.Collections.Generic;

namespace Rober.IDAL.Repository
{
    public partial interface IScheduleRepository : IRepository<Schedule>
    {
        PagedList<Schedule> GetList(string title = "", int subCategoryId = 0, int statusId = 0, ScheduleCategory? scheduleCategory = null, int pageIndex = 0, int pageSize = int.MaxValue);
        List<Schedule> GetList(int userId, DateTime start, DateTime end);
        List<User> GetScheduleUsers(int scheduleId);
        List<CustomerFile> GetScheduleFiles(int scheduleId);
        bool DeleteScheduleUsers(IEnumerable<int> scheduleIds);
    }
}
