using System.Collections.Generic;
using Rober.Core;
using Rober.Core.Action;
using Rober.Core.Domain.Schedules;

namespace Rober.Action.Model.Schedules
{
    public class ScheduleMySchedulerResponse : Response
    {
        public List<Schedule> List { get; set; }
    }
}
