using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Action;
using Rober.Core.Domain.Common;

namespace Rober.Action.Model.Common
{
    public class GetAttributeResponse : Response
    {
        public GenericAttribute GenericAttribute { get; set; }
    }
}
