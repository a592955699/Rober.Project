using System;
using System.Collections.Generic;
using System.Text;

namespace Rober.Core.Configuration
{
    public class InterceptorConfig
    {
        public string Type { get; set; }
        public HashSet<string> IncluedCommands { get; set; }
        public HashSet<string> ExcludeCommands { get; set; }
    }
}
