using System;
using System.Collections.Generic;
using System.Text;

namespace Rober.Core.Domain.Schedules
{
    public class ScheduleMapping : BaseEntity
    {
        public int ScheduleId { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; }
    }
}
