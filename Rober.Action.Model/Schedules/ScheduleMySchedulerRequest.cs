using System;
using Rober.Core.Action;
using Rober.Core.Enums;

namespace Rober.Action.Model.Schedules
{
    public class ScheduleMySchedulerRequest : SessionRequest
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
