using System.Collections.Generic;
using Rober.Core.Constants;

namespace Rober.Core.Action
{
    /// <summary>
    /// Action 的调用器, 将 action 的调用封装,完成调用
    /// </summary>
    public class ActionInvocation
    {
        /// <summary>
        /// 完成调用
        /// </summary>
        public void Invoke()
        {
            if (Interceptors != null && Interceptors.MoveNext())
            {
                var interceptor = Interceptors.Current;
                interceptor.Intercept(this);
            }
            else
            {
                Code = Action.Execute(Request, RequestHeader, out _response);
            }
        }

        #region Properties
        public string Command { get; set; }
        public Rober.Core.Action.IAction Action { get; set; }
        public object Request { get; set; }
        public RequestHeader RequestHeader { get; set; }
        private object _response;
        public object Response
        {
            get => _response;
            set => _response = value;
        }
        public int Code { get; set; }
        public CommandConfig Config { get; set; }
        public IEnumerator<INterceptor> Interceptors { get; set; }
        #endregion
    }
}
