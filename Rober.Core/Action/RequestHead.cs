using System;

namespace Rober.Core.Action
{
    /// <summary>
    /// 要求標頭
    /// </summary>
    [Serializable]
    public class RequestHeader
    {
        public RequestHeader()
        {
            TraceId = Guid.NewGuid();
        }

        /// <summary>
        /// 追踪识别码
        /// </summary>
        public Guid TraceId
        {
            get;
            set;
        }

        /// <summary>
        /// 來自哪裡的請求(服務名稱)
        /// </summary>
        public string FromApp
        {
            get;
            set;
        }

        /// <summary>
        /// 來自哪裡的請求(Ip)
        /// </summary>
        public string FromIp
        {
            get;
            set;
        }

        /// <summary>
        /// 建立要求的時間
        /// </summary>
        public DateTime Time
        {
            get;
            set;
        }
    }
}
