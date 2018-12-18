using Rober.Core.Action;
using Rober.Core.Enums;

namespace Rober.Action.Model.Schedules
{
    public class ScheduleSubCategoryListRequest : SessionPageRequest
    {
        public ScheduleCategory ScheduleCategory { get; set; }
    }
}
