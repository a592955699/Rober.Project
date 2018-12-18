using System;
using System.Collections.Generic;

namespace Rober.Core.Action
{
    /// <summary>
    /// 支持 command 名过滤的 拦截器
    /// </summary>
    public abstract class CommandFilterInterceptor : INterceptor
    {
        public HashSet<string> ExcludeCommands { get; set; }

        public HashSet<string> IncluedCommands { get; set; }

        public abstract void Intercept(ActionInvocation invocation);

        public bool ApplyInterceptor(String command)
        {
            if (IncluedCommands != null)
            {
                return IncluedCommands.Contains(command);
            }
            if (ExcludeCommands != null)
            {
                return !ExcludeCommands.Contains(command);
            }
            return true;
        }

    }
}
