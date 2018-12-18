namespace Rober.Core.Action
{
    /// <summary>
    /// 拦截器
    /// </summary>
    public interface INterceptor
    {
        /// <summary>
        /// 拦截
        /// </summary>
        /// <param name="invocation"></param>
        void Intercept(ActionInvocation invocation);
    }
}
