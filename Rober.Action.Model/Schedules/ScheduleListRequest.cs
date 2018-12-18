using Rober.Core.Action;
using Rober.Core.Enums;

namespace Rober.Action.Model.Schedules
{
    public class ScheduleListRequest : SessionPageRequest
    {
        public string Title { get; set; }
        public int SubCategoryId { get; set; }
        public int StatusId { get; set; }
        public ScheduleCategory ScheduleCategory { get; set; }
    }
}
