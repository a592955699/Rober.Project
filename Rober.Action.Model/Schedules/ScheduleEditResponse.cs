using Rober.Core.Action;
using Rober.Core.Domain.Schedules;

namespace Rober.Action.Model.Schedules
{
    public class ScheduleEditResponse : Response
    {
        public Schedule Schedule { get; set; }
    }
}
