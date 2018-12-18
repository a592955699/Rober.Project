using Newtonsoft.Json;

namespace Rober.Core.Action
{
    public class Response : IResponse
    {
        /// <summary>
        /// 序列化內容
        /// </summary>
        /// <returns>物件內容</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// 隱含轉換為string
        /// </summary>
        /// <param name="instance">Response</param>
        public static implicit operator string(Response instance)
        {
            return instance.ToString();
        }
    }
}
