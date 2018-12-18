using System;
using System.Collections.Generic;
using System.Text;
using Rober.WebApp.Framework.MVC.ModelBinding;
using Rober.WebApp.Framework.MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rober.WebApp.Framework.Models.Areas.Account
{
    public class UserModel : BaseEntityModel
    {
        public UserModel()
        {
            SelectedRoleIds = new List<int>();
            SelectedDepartmentIds = new List<int>();
            AvailableRoles = new List<SelectListItem>();
            AvailableDepartments = new List<SelectListItem>();
        }

        [JmDisplayName("账号")]
        public string UserName { get; set; }
        [JmDisplayName("昵称")]
        public string NickName { get; set; } = string.Empty;
        [JmDisplayName("密码")]
        public string Password { get; set; } = string.Empty;
        [JmDisplayName("错误次数")]
        public int ErrorCount { get; set; }
        [JmDisplayName("错误时间")]
        public virtual DateTime? ErrorTime { get; set; }
        [JmDisplayName("登录时间")]
        public DateTime? LoginTime { get; set; }
        [JmDisplayName("登录IP")]
        public string LoginIp { get; set; } = string.Empty;
        [JmDisplayName("是否发布")]
        public bool Published { get; set; } = true;
     
        public List<SelectListItem> AvailableRoles { get; set; }
        [JmDisplayName("角色", "用户的角色.")]
        public IList<int> SelectedRoleIds { get; set; }
        [JmDisplayName("部门", "用户的部门.")]
        public IList<int> SelectedDepartmentIds { get; set; }
        public List<SelectListItem> AvailableDepartments { get; set; }
    }
}
