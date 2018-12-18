using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Constants;

namespace Rober.Core.Action
{
    public static class ActionInvocationExtensions
    {
        public static T GetResponse<T>(this ActionInvocation invocation) where T : Response, new()
        {
            if (invocation.Request != null)
                return (T)invocation.Response;
            else
                return null;
        }

        /// <summary>
        /// 设置响应成功
        /// </summary>
        public static void SetSuccess(this ActionInvocation invocation)
        {
            SetResposneCode(invocation, ResponseCode.Success);
        }
        /// <summary>
        /// 设置系统错误
        /// </summary>
        /// <param name="invocation"></param>
        public static void SetSystemError(this ActionInvocation invocation)
        {
            SetResposneCode(invocation, ResponseCode.SystemError);
        }

        /// <summary>
        /// session无效
        /// </summary>
        /// <param name="invocation"></param>
        public static void SetInvalidSession(this ActionInvocation invocation)
        {
            SetResposneCode(invocation, ResponseCode.InvalidSession);
        }

        /// <summary>
        /// 权限不足
        /// </summary>
        /// <param name="invocation"></param>
        public static void SetInsufficientPrivileges(this ActionInvocation invocation)
        {
            SetResposneCode(invocation, ResponseCode.InsufficientPrivileges);
        }

        /// <summary>
        /// 设置响应code
        /// </summary>
        /// <param name="invocation"></param>
        /// <param name="code"></param>
        public static void SetResposneCode(this ActionInvocation invocation, int code)
        {
            invocation.Code = code;
        }
    }
}
