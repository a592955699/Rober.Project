using System;
using System.Collections.Generic;
using System.Text;

namespace Rober.Core.Enums
{
    /// <summary>
    /// 任务相关的枚举类型
    /// </summary>
    public class EnumType
    {
        /// <summary>
        /// 处理类型
        /// </summary>
        public enum ExecuteType
        {
            /// <summary>
            /// 提交
            /// </summary>
            Submit,
            /// <summary>
            /// 保存
            /// </summary>
            Save,
            /// <summary>
            /// 退回
            /// </summary>
            Back,
            /// <summary>
            /// 完成
            /// </summary>
            Completed,
            /// <summary>
            /// 转交
            /// </summary>
            Redirect,
            /// <summary>
            /// 结束流程
            /// </summary>
            Termination
        }
    }
}
