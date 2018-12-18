using System;
using System.Collections.Generic;
using System.Text;
using Rober.WebApp.Framework.MVC.ModelBinding;
using Rober.WebApp.Framework.MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rober.WebApp.Framework.Models.Areas.Account
{
    public class RuleModel : BaseEntityModel
    {
        [JmDisplayName("角色")]
        public string Name { get; set; }
        [JmDisplayName("备注")]
        public string Remark { get; set; } = string.Empty;
        [JmDisplayName("是否发布")]
        public bool Published { get; set; } = true;

        public class AddRuleUserModel : BaseModel
        {
            public AddRuleUserModel()
            {
                AvailablePublishedOptions = new List<SelectListItem>();
            }

            //public int UserId { get; set; }
            //public int RuleId { get; set; }

            [JmDisplayName("账号")]
            public string UserName { get; set; }

            [JmDisplayName("是否发布")]
            public int PublishedId { get; set; }

            public IList<SelectListItem> AvailablePublishedOptions { get; set; }

            public int[] SelectedUserIds { get; set; }
        }
    }
}
