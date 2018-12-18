using System;
using System.Collections.Generic;
using System.Text;

namespace Rober.Core.Enums
{
    public enum WorkFlowSignatureType
    {
        /// <summary>
        /// 无签批意见
        /// </summary>
        None = 0,
        /// <summary>
        /// 有签批意见，有签章
        /// </summary>
        Signature = 1,
        /// <summary>
        /// 有签批意见，无需签章
        /// </summary>
        NoSignature = 2
    }
}
