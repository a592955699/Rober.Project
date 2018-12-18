using Rober.Core.Action;
using Rober.Core.Domain.Schedules;
using System.Collections.Generic;

namespace Rober.Action.Model.Schedules
{
    public class ScheduleDestroyRequest : SessionRequest
    {
        public List<Schedule> Schedules { get; set; }
    }
}
