namespace Rober.Core.Action
{
    //public class Request : BaseEntity, IRequest
    public class Request : IRequest
    {
        //public RequestHeader Header { get; set; }
        /// <summary>
        /// 隱含轉換為string
        /// </summary>
        /// <param name="instance">Request</param>
        public static implicit operator string(Request instance)
        {
            return instance.ToString();
        }
    }
}
