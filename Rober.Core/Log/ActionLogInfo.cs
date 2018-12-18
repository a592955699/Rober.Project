using System;

namespace Rober.Core.Log
{
    /// <summary>
    /// 動作的內容
    /// </summary>
    public class ActionLogInfo : BaseLogInfo
    {   
        /// <summary>
        /// 追蹤碼
        /// </summary>
        public Guid TraceId
        {
            get;
            set;
        }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 動作的類型
        /// </summary>
        public ActionType Action
        {
            get;
            set;
        }

        /// <summary>
        /// 資訊
        /// </summary>
        public string Info
        {
            get;
            set;
        }

        /// <summary>
        /// 時間
        /// </summary>
        public DateTime Time
        {
            get;
            set;
        }

        /// <summary>
        /// 內容
        /// </summary>
        public string Data
        {
            get;
            set;
        }
    }
}
