using Newtonsoft.Json;

namespace Rober.Core.Log
{
    /// <summary>
    /// 紀錄資料基礎類別
    /// </summary>
    public class BaseLogInfo
    {
        /// <summary>
        /// 序列化內容
        /// </summary>
        /// <returns>物件內容</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject( this );
        }

        /// <summary>
        /// 隱含轉換為string
        /// </summary>
        /// <param name="instance">BaseLogInfo</param>
        public static implicit operator string( BaseLogInfo instance )
        {
            return instance.ToString();
        }
    }
}
