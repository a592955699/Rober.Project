using System.Collections.Generic;
using Rober.WebApp.Framework.Kendoui;
using Rober.WebApp.Framework.MVC.ModelBinding;
using Rober.WebApp.Framework.MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rober.WebApp.Framework.Models.Areas.Account
{
    public class UserListModel : BaseModel
    {
        public UserListModel()
        {
            AvailablePublishedOptions = new List<SelectListItem>();
        }

        [JmDisplayName("账号")]
        public string UserName { get; set; }

        [JmDisplayName("昵称")]
        public string NickName { get; set; }

        [JmDisplayName("是否发布")]
        public int PublishedId { get; set; }

        public IList<SelectListItem> AvailablePublishedOptions { get; set; }
    }
}
