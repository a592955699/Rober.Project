using System.Collections.Generic;
using Rober.WebApp.Framework.Kendoui;
using Rober.WebApp.Framework.MVC.ModelBinding;
using Rober.WebApp.Framework.MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rober.WebApp.Framework.Models.Areas.Account
{
    public class RuleListModel : BaseModel
    {
        public RuleListModel()
        {
            AvailablePublishedOptions = new List<SelectListItem>();
        }

        public int Id { get; set; }

        [JmDisplayName("角色名称")]
        public string Name { get; set; }

        [JmDisplayName("是否发布")]
        public int PublishedId { get; set; }

        public IList<SelectListItem> AvailablePublishedOptions { get; set; }
    }
}
