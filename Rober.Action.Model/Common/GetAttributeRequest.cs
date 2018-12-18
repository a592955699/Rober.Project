using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core;
using Rober.Core.Action;
using Rober.Core.Domain.Common;

namespace Rober.Action.Model.Common
{
    public class GetAttributeRequest : SessionRequest
    {
        public string KeyGroup { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
        public int EntityId { get; set; }
    }
}
