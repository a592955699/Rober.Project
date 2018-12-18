namespace Rober.Core.Action.Interceptors
{
    public abstract class PermissionInterceptor : CommandFilterInterceptor
    {
        public override void Intercept(ActionInvocation invocation)
        {
            if (HasPermission(invocation))
            {
                invocation.Invoke();
            }
            else
            {
                invocation.SetInsufficientPrivileges();
            }
        }

        protected abstract bool HasPermission(ActionInvocation invocation);
    }
}
