using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Rober.Core.Domain.Account;
using Rober.Core.Domain.Common;
using Rober.Core.Enums;

namespace Rober.Core.Domain.Schedules
{
    public class Schedule : BaseEntity
    {
        public ScheduleCategory ScheduleCategory { get; set; }
        public int SubCategoryId { get; set; }
        public ScheduleSubCategory SubCategory { get; set; }
        public int StatusId { get; set; }
        public ScheduleSubCategory Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Remark { get; set; }
        public string Common { get; set; }
        public int CreatedUserId { get; set; }
        public User CreatedUser { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool Top { get; set; }

        #region Navigation properties

        //public IEnumerable<UserDepartmentMapping> UserDepartmentMappings { get; set; }
        //public IEnumerable<UserRuleMapping> UserRuleMappings { get; set; }
        //public IEnumerable<UserMenuMapping> UserMenuMappings { get; set; }

        private IList<CustomerFile> _customerFiles;
        /// <summary>
        /// Gets or sets the customer roles
        /// </summary>
        public virtual IList<CustomerFile> Files
        {
            get => _customerFiles ?? (_customerFiles = new List<CustomerFile>());
            set => _customerFiles = value;
        }

        private IList<User> _users;
        /// <summary>
        /// Gets or sets the customer roles
        /// </summary>
        public virtual IList<User> Users
        {
            get => _users ?? (_users = new List<User>());
            set => _users = value;
        }
        #endregion
    }
}
