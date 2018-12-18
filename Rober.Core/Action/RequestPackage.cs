namespace Rober.Core.Action
{
    /// <summary>
    /// 要求的封包
    /// </summary>    
    //public class RequestPackage : BaseEntity
    public class RequestPackage
    {
        /// <summary>
        /// 標頭
        /// </summary>        
        public RequestHeader Header
        {
            get;
            set;
        }

        /// <summary>
        /// 要求的內容
        /// </summary>
        public object Data
        {
            get;
            set;
        }

        /// <summary>
        /// 建構式
        /// </summary>
        public RequestPackage()
        {
            Header = new RequestHeader();
        }
    }
}
