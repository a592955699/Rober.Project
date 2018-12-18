using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Constants;

namespace Rober.Core
{
    [Serializable]
    public class BaseResult
    {
        #region 构造函数
        public BaseResult()
        {
        }

        public BaseResult(int code)
        {
            Code = code;
        }
        public BaseResult(int code, string message) : this(code)
        {
            Message = message;
        }
        public BaseResult(int code, string message, object data) : this(code, message)
        {
            Data = data;
        } 
        #endregion

        public bool Status => Code == ResponseCode.Success;
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        #region 
        /// <summary>
        /// success
        /// </summary>
        public void SetSuccess()
        {
            Code = ResponseCode.Success;
        }

        /// <summary>
        /// error
        /// </summary>
        public void SetError()
        {
            Code = ResponseCode.SystemError;
        }
        /// <summary>
        /// error
        /// </summary>
        /// <param name="code"></param>
        public void SetCode(int code)
        {
            Code = code;
        }
        #endregion
    }
}
