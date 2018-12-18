using Rober.Core.Enums;
using System.Collections.Generic;

namespace Rober.Core.Extensions
{
    public static class ScheduleCategoryExtensions
    {
        public static string ScheduleCategoryName(this ScheduleCategory scheduleCategory)
        {
            var dirDictionary=new Dictionary<string, string>()
            {
                { "Schedule","日程计划"},
                { "Task","任务计划"},
                { "WorkPlan","工作计划"}
            };
            string name = "";
            dirDictionary.TryGetValue(scheduleCategory.ToString(), out name);
            return name;
        }
    }
}
