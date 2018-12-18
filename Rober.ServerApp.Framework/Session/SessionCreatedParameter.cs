namespace Rober.ServerApp.Framework.Session
{
    /// <summary>
    /// 
    /// </summary>
    public class SessionCreatedParameter
    {
        ///// <summary>
        ///// 
        ///// </summary>
        //public string Browser { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public string Version { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public string UserAgent { get; set; }
        public string ClientIp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Referrer { get; set; }
    }
}