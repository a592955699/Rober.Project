using System;
using System.Collections.Generic;
using System.Text;

namespace Rober.Core.Enums
{
    public enum WorkFlowStatus
    {
        /// <summary>
        /// 等待中
        /// </summary>
        Wait = -1,
        /// <summary>
        /// 待处理
        /// </summary>
        WaitHandle = 1,
        /// <summary>
        /// 已打开
        /// </summary>
        Opened = 2,
        /// <summary>
        /// 已完成
        /// </summary>
        Done = 3,
        /// <summary>
        /// 已退回
        /// </summary>
        Returned = 4,
        /// <summary>
        /// 他人已处理
        /// </summary>
        OthersHandle = 5,
        /// <summary>
        /// 他人已退回
        /// </summary>
        OthersReturned = 6,
        /// <summary>
        /// 结束终止
        /// </summary>
        Termination
    }
}
