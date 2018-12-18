using System.Collections.Generic;

namespace Rober.Core.Action
{
    /// <summary>
    /// Command 配置
    /// </summary>
    public class CommandConfig
    {
        /// <summary>
        /// 增加一个拦截器
        /// </summary>
        /// <param name="interceptor"></param>
        public void AddInterceptor(INterceptor interceptor)
        {
            if (Interceptors == null)
            {
                Interceptors = new List<INterceptor>();
            }
            Interceptors.Add(interceptor);
        }
        #region Properties
        public Rober.Core.Action.IAction Action { get; set; }
        public ICollection<INterceptor> Interceptors { get; set; }
        #endregion
    }
}
