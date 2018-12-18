using System;
using Rober.Core.Action;

namespace Rober.Core.Http.Proxy
{
    /// <summary>
    /// 代理介面
    /// </summary>
    public interface IProxy : IDisposable
    {
        /// <summary>
        /// 名稱
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// 位置
        /// </summary>
        Uri Uri
        {
            get;
        }

        /// <summary>
        /// 訊息
        /// </summary>
        string Message
        {
            get;
        }

        /// <summary>
        /// 狀態
        /// </summary>
        int Code
        {
            get;
        }

        /// <summary>
        /// 執行動作
        /// </summary>
        /// <param name="name">要執行的動作名稱</param>
        /// <typeparam name="TRequest">要求的內容型別</typeparam>
        /// <typeparam name="TResponse">回應的內容型別</typeparam>
        /// <param name="request">要求內容</param>
        /// <param name="response">回應內容</param>
        /// <returns>是否執行成功</returns>
        bool Action<TRequest, TResponse>(string name, TRequest request, out TResponse response)
                where TRequest : Request
                where TResponse : Response;

        /// <summary>
        /// 執行動作
        /// </summary>
        /// <typeparam name="TRequest">要求的內容型別</typeparam>
        /// <typeparam name="TResponse">回應的內容型別</typeparam>
        /// <param name="header"></param>
        /// <param name="name">要執行的動作名稱</param>
        /// <param name="request">要求內容</param>
        /// <param name="response">回應內容</param>
        /// <returns>是否執行成功</returns>
        bool Action<TRequest, TResponse>(RequestHeader header, string name, TRequest request, out TResponse response)
                where TRequest : Request
                where TResponse : Response;
    }
}
