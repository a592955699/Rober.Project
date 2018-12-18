using System;
using Rober.Core.Constants;

namespace Rober.Core.Action
{
    /// <summary>
    /// 回應標頭
    /// </summary>
    [Serializable]
    public class ResponseHeader
    {
        /// <summary>
        /// 追蹤識別碼
        /// </summary>
        public Guid TraceId
        {
            get;
            set;
        }

        /// <summary>
        /// 是否執行成功
        /// </summary>
        public bool Status => this.Code == 0;

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 狀態
        /// </summary>
        public int Code
        {
            get;
            set;
        }

        /// <summary>
        /// 建構式
        /// </summary>
        public ResponseHeader()
        {
            Message = string.Empty;
            Code = ResponseCode.SystemError;
        }
    }
}
