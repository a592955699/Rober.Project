using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Enums;
using Rober.WebApp.Framework.MVC.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rober.WebApp.Framework.Models.Areas.Schedules
{
    public class ScheduleListModel
    {
        public ScheduleListModel()
        {
            AvailableStatus = new List<SelectListItem>();
            AvailableSubScheduleCategorys = new List<SelectListItem>();
        }
        [JmDisplayName("标题")]
        public string Title { get; set; }
        [JmDisplayName("类型")]
        public int SubCategoryId { get; set; }
        public ScheduleCategory ScheduleCategory { get; set; }
        [JmDisplayName("状态")]
        public int StatusId { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedTime { get; set; }
        public List<SelectListItem> AvailableSubScheduleCategorys { get; set; }
        public List<SelectListItem> AvailableStatus { get; set; }
    }
}
