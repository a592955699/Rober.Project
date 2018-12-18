using System;

namespace Rober.Core.Session
{
    /// <summary>
    /// SessionValueObjectWrapper
    /// </summary>
    [Serializable]
    public class SessionValueObjectWrapper
    {
        /// <summary>
        /// SessionId
        /// </summary>
        public string SessionId { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Ip
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// ValueObject
        /// </summary>
        public object ValueObject { get; set; }

        /// <summary>
        /// LastAccessTime
        /// </summary>
        public DateTime LastAccessTime { get; set; }
    }
}
