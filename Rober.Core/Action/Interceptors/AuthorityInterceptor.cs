namespace Rober.Core.Action.Interceptors
{
    public abstract class AuthorityInterceptor : CommandFilterInterceptor
    {
        protected abstract bool IsAuthoritied(ActionInvocation invocation);

        public override void Intercept(ActionInvocation invocation)
        {
            if (IsAuthoritied(invocation))
            {
                invocation.Invoke();
            }
            else
            {
                invocation.SetInvalidSession();
            }
        }
    }
}
