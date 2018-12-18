using System;
using Rober.Core.Log;
using log4net;

namespace Rober.Core.Action.Interceptors
{
    /// <summary>
    /// ÈÕÖ¾À¹½ØÆ÷
    /// </summary>
    public class LogInterceptor : CommandFilterInterceptor
    {
        private static readonly String beforeLogTemplate =
            "command executing...\n" +
            "\t\ttrace id = {0}\n " +
            "\t\tcommand = {1}\n " +
            "\t\trequest = {2}";

        private static readonly String afterLogTemplate =
            "command executed\n" +
            "\t\ttrace id = {0}\n " +
            "\t\tcommand = {1}\n " +
            "\t\trequest = {2}";

        public override void Intercept(ActionInvocation invocation)
        {

            try
            {
                if (Logger.Instance.IsDebugEnabled)
                {
                    Logger.Instance.Debug(String.Format(beforeLogTemplate, GetTraceId(invocation), invocation.Command,
                        invocation.Request));
                }
                invocation.Invoke();
            }
            finally
            {
                if (Logger.Instance.IsDebugEnabled)
                {
                    Logger.Instance.Debug(String.Format(afterLogTemplate, GetTraceId(invocation), invocation.Command, invocation.Response));
                }
            }
          
        }

        private string GetTraceId(ActionInvocation invocation)
        {
            return invocation.RequestHeader.TraceId.ToString();
        }
    }
}
