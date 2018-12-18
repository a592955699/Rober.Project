using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Constants;
using Newtonsoft.Json;

namespace Rober.Core.Action
{
    /// <summary>
    /// Action 扩展类
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public abstract class ActionSupport<TIn, TOut> : IAction
        where TIn : Request, new()
        where TOut : Response, new()
    {
        /// <summary>
        /// 进行业务处理后,返回响应消息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestHeader"></param>
        /// <param name="response"></param>
        /// <returns>Response Code Status</returns>
        public abstract int DoExecute(TIn request, RequestHeader requestHeader, out TOut response);

        #region Action Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestHeader"></param>
        /// <param name="response"></param>
        /// <returns>Response Code Status</returns>
        public int Execute(object request,RequestHeader requestHeader,out object response)
        {
            //#TODO JObject 转 Request 优化
            if(request is TIn)
            {
                var code = DoExecute(request as TIn, requestHeader, out var rspOut);
                response = rspOut;
                return code;
            }
            else
            {
                var code = DoExecute(JsonConvert.DeserializeObject<TIn>(request.ToString()), requestHeader, out var rspOut);
                response = rspOut;
                return code;
            }
        }
        #endregion

        #region 
        public int Code { get; set; }

        public void SetSuccess()
        {
            this.Code = ResponseCode.Success;
        }
        public void SetCode(int code)
        {
            this.Code = code;
        }
        public void SetSystemError()
        {
            this.Code = ResponseCode.SystemError;
        }
        public bool Success => this.Code == ResponseCode.Success;
        #endregion
    }
}
