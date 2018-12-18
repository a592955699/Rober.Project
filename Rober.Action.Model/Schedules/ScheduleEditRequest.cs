using Rober.Core.Action;
using Rober.Core.Domain.Schedules;

namespace Rober.Action.Model.Schedules
{
    public class ScheduleEditRequest : SessionRequest
    {
        public Schedule Schedule { get; set; }
    }
}
