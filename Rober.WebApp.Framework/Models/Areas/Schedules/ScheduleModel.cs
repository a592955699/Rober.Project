using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Rober.Core.Domain.Account;
using Rober.Core.Domain.Schedules;
using Rober.Core.Enums;
using Rober.WebApp.Framework.MVC.ModelBinding;
using Rober.WebApp.Framework.MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rober.WebApp.Framework.Models.Areas.Schedules
{
    public class ScheduleModel : BaseEntityModel
    {
        public ScheduleModel()
        {
            AvailableSubScheduleCategorys = new List<SelectListItem>();
            AvailableStatus = new List<SelectListItem>();
            SelectedUserIds = new List<int>();
            AvailableUsers = new List<SelectListItem>();
        }
        public ScheduleCategory ScheduleCategory { get; set; }
        public string ScheduleCategoryName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [JmDisplayName("类型")]
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        [JmDisplayName("状态")]
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        [Required]
        [JmDisplayName("开始日期")]
        public DateTime StartTime { get; set; }
        [Required]
        [JmDisplayName("结束日期")]
        public DateTime EndTime { get; set; }
        [Required]
        [MaxLength(100)]
        [JmDisplayName("标题")]
        public string Title { get; set; }
        [Required]
        [JmDisplayName("内容")]
        public string Content { get; set; }
        [JmDisplayName("备注")]
        public string Remark { get; set; }
        [JmDisplayName("评价")]
        public string Common { get; set; }
        [JmDisplayName("置顶")]
        public bool Top { get; set; }
        public int CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime CreatedTime { get; set; }
        public List<SelectListItem> AvailableSubScheduleCategorys { get; set; }
        public List<SelectListItem> AvailableStatus { get; set; }
        [JmDisplayName("接收人")]
        public List<int> SelectedUserIds { get; set; }
        public List<SelectListItem> AvailableUsers { get; set; }
    }
}
