using System.Collections.Generic;

namespace Rober.Core.Domain.Account
{
    public class Menu : BaseEntity
    {
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; } = string.Empty;
        public bool IsMenu { get; set; } = true;
        public string Link { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Script { get; set; } = string.Empty;
        public int Order { get; set; } = 0;
        public bool OpenUrlInNewTab { get; set; }

        public bool Published { get; set; } = true;
        //public IEnumerable<UserMenuMapping> UserMenuMappings { get; set; }
    }
}
