using System;

namespace Rober.Core.Action
{
    /// <summary>
    /// 回應內容
    /// </summary>
    [Serializable]
    //public class ResponsePackage : BaseEntity
    public class ResponsePackage
    {

        /// <summary>
        /// 標頭
        /// </summary>
        public ResponseHeader Header
        {
            get;
            set;
        }

        /// <summary>
        /// 執行結果
        /// </summary>        
        public Object Data
        {
            get;
            set;
        }

        /// <summary>
        /// 建構式
        /// </summary>
        public ResponsePackage()
        {
            Header = new ResponseHeader();
        }
    }
}
