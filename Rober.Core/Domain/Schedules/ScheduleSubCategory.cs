using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Enums;

namespace Rober.Core.Domain.Schedules
{
    public class ScheduleSubCategory : BaseEntity
    {
        public ScheduleCategory ScheduleCategory { get; set; }
        public string Title { get; set; }
        public bool IsCategory { get; set; }
        public int Order { get; set; }
        public string Template { get; set; }
    }
}
