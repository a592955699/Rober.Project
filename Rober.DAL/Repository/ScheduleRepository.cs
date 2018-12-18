using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public class ScheduleRepository : EfRepository<Schedule>, IScheduleRepository
    {
        #region 构造函数
        private readonly IScheduleMappingRepository _scheduleMappingRepository;
        private readonly IScheduleSubCategoryRepository _scheduleSubCategoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerFileRepository _customerFileRepository;
        public ScheduleRepository(IEfUnitOfWork efUnitOfWork
            , IScheduleSubCategoryRepository scheduleSubCategoryRepository
            , IUserRepository userRepository
            , IScheduleMappingRepository scheduleMappingRepository
            , ICustomerFileRepository customerFileRepository
            ) : base(efUnitOfWork)
        {
            _scheduleSubCategoryRepository = scheduleSubCategoryRepository;
            _userRepository = userRepository;
            _scheduleMappingRepository = scheduleMappingRepository;
            _customerFileRepository = customerFileRepository;
        }
        #endregion

        #region 公有方法
        public PagedList<Schedule> GetList(string title = "", int subCategoryId = 0, int statusId = 0, ScheduleCategory? scheduleCategory = null,
           int pageIndex = 0, int pageSize = Int32.MaxValue)
        {
            var query = Table;
            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(x => x.Title.Contains(title.TrimEnd()));
            }
            if (subCategoryId > 0)
            {
                query = query.Where(x => x.SubCategoryId == subCategoryId);
            }
            if (statusId > 0)
            {
                query = query.Where(x => x.StatusId == statusId);
            }
            if (scheduleCategory.HasValue)
            {
                query = query.Where(x => x.ScheduleCategory == scheduleCategory.Value);
            }

            query = from s in query
                    join sub in _scheduleSubCategoryRepository.Table
                        on s.SubCategoryId equals sub.Id
                    join sta in _scheduleSubCategoryRepository.Table
                        on s.StatusId equals sta.Id
                    join user in _userRepository.Table
                        on s.CreatedUserId equals user.Id
                    select new Schedule()
                    {
                        Id = s.Id,
                        Common = s.Common,
                        CreatedUserId = s.CreatedUserId,
                        StatusId = s.StatusId,
                        SubCategoryId = s.SubCategoryId,
                        Content = s.Content,
                        CreatedTime = s.CreatedTime,
                        CreatedUser = user,
                        EndTime = s.EndTime,
                        Files = s.Files,
                        Remark = s.Remark,
                        ScheduleCategory = s.ScheduleCategory,
                        StartTime = s.StartTime,
                        Status = sta,
                        SubCategory = sub,
                        Title = s.Title,
                        Top = s.Top,
                        Users = s.Users
                    };
            return new PagedList<Schedule>(query, pageIndex, pageSize);
        }
        /// <summary>
        /// 获取我的日历
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<Schedule> GetList(int userId, DateTime start, DateTime end)
        {
            var query = from s in Table
                        join m in _scheduleMappingRepository.Table
                            on s.Id equals m.ScheduleId
                        where ("User".Equals(m.EntityName) && m.EntityId == userId) || s.CreatedUserId == userId
                        select s;
            return query.ToList();
        }
        public List<User> GetScheduleUsers(int scheduleId)
        {
            var query = from m in _scheduleMappingRepository.Table
                        join u in _userRepository.Table
                            on m.EntityId equals u.Id
                        where "User".Equals(m.EntityName) && m.ScheduleId == scheduleId
                        select u;
            return query.ToList();
        }
        public List<CustomerFile> GetScheduleFiles(int scheduleId)
        {
            var query = from m in _scheduleMappingRepository.Table
                        join u in _customerFileRepository.Table
                            on m.EntityId equals u.Id
                        where "CustomerFile".Equals(m.EntityName) && m.ScheduleId == scheduleId
                        select u;

            return query.ToList();
        }
        public bool DeleteScheduleUsers(IEnumerable<int> scheduleIds)
        {
            var query = from m in _scheduleMappingRepository.Table
                        where "User".Equals(m.EntityName) && scheduleIds.Any(x => x == m.ScheduleId)
                        select m;
            var userMappings = query.ToList();
            return _scheduleMappingRepository.Delete(userMappings);
        }
        public override bool Insert(Schedule schedule, bool IsCommit = true)
        {
            base.Insert(schedule, true);

            if (schedule.Users != null && schedule.Users.Any())
            {
                var ruleMappings = schedule.Users.Select(x => new ScheduleMapping()
                {
                    ScheduleId = schedule.Id,
                    EntityId = x.Id,
                    EntityName = "User"
                });
                _scheduleMappingRepository.Insert(ruleMappings, false);
            }
            return IsCommit && UnitOfWork.Commit() > 0;
        }
        public override bool Update(Schedule schedule, bool IsCommit = true, params Expression<Func<Schedule, object>>[] properties)
        {
            //删除角色
            var rules = _scheduleMappingRepository.Table.Where(x => x.ScheduleId == schedule.Id);
            _scheduleMappingRepository.Delete(rules);

            if (schedule.Users != null && schedule.Users.Any())
            {
                var ruleMappings = schedule.Users.Select(x => new ScheduleMapping()
                {
                    ScheduleId = schedule.Id,
                    EntityId = x.Id,
                    EntityName = "User"
                });
                _scheduleMappingRepository.Insert(ruleMappings, false);
                return IsCommit && UnitOfWork.Commit() > 0;
            }

            return base.Update(schedule, IsCommit, properties);
        }
        #endregion
    }
}
