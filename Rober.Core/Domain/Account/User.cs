using System;
using System.Collections.Generic;

namespace Rober.Core.Domain.Account
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; } = string.Empty;
        /// <summary>
        /// 错误次数
        /// </summary>
        public int ErrorCount { get; set; }
        /// <summary>
        /// 登陆失败时间
        /// </summary>
        public virtual DateTime? ErrorTime { get; set; }
        /// <summary>
        /// 登录时间，成功登录再记录时间
        /// </summary>
        public DateTime? LoginTime { get; set; }
        /// <summary>
        /// 登录ip
        /// </summary>
        public string LoginIp { get; set; } = string.Empty;

        public bool Published { get; set; } = true;

        #region Navigation properties

        //public IEnumerable<UserDepartmentMapping> UserDepartmentMappings { get; set; }
        //public IEnumerable<UserRuleMapping> UserRuleMappings { get; set; }
        //public IEnumerable<UserMenuMapping> UserMenuMappings { get; set; }

        private IList<Rule> _customerRoles;
        /// <summary>
        /// Gets or sets the customer roles
        /// </summary>
        public virtual IList<Rule> Roles
        {
            get => _customerRoles ?? (_customerRoles = new List<Rule>());
            set => _customerRoles = value;
        }
        private IList<Department> _customerDepartments;
        /// <summary>
        /// Gets or sets the customer roles
        /// </summary>
        public virtual IList<Department> Departments
        {
            get => _customerDepartments ?? (_customerDepartments = new List<Department>());
            set => _customerDepartments = value;
        }
        
        #endregion

    }
}
