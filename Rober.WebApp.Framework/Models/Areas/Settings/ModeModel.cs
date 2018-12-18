using Rober.WebApp.Framework.MVC.Models;

namespace Rober.WebApp.Framework.Models.Areas.Settings
{
    public partial class ModeModel : BaseModel
    {
        public string ModeName { get; set; }
        public bool Enabled { get; set; }
    }
}