using System;
using Newtonsoft.Json;
using Rober.Core.Action;
using Rober.Core.Log;

namespace Rober.Core.Http.Proxy
{
    /// <summary>
    /// Proxy基礎類別
    /// </summary>
    public abstract class ProxyBase : IProxy
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// 位置
        /// </summary>
        public Uri Uri
        {
            get;
            protected set;
        }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message
        {
            get;
            protected set;
        }

        /// <summary>
        /// 狀態
        /// </summary>
        public int Code
        {
            get;
            protected set;
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
        public abstract bool Action<TRequest, TResponse>(string name, TRequest request, out TResponse response)
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
        public virtual bool Action<TRequest, TResponse>(RequestHeader header, string name, TRequest request, out TResponse response)
                where TRequest : Request
                where TResponse : Response
        {
            var reqPackage = new RequestPackage {Header = header, Data = request};

            return DoAction( name, reqPackage, out response);
        }

        /// <summary>
        /// 執行動作
        /// </summary>
        /// <typeparam name="TResponse">回應的內容型別</typeparam>
        /// <param name="package">要求的內容</param>
        /// <param name="name">要執行的動作名稱</param>        
        /// <param name="response">回應內容</param>
        /// <returns>是否執行成功</returns>
        protected abstract bool DoAction<TResponse>( string name, RequestPackage package, out TResponse response)
                where TResponse : Response;

        /// <summary>
        /// 釋放資源
        /// </summary>
        public virtual void Dispose()
        {            
        }

        /// <summary>
        ///　紀錄要求內容
        /// </summary>
        /// <param name="name">命令名稱</param>
        /// <param name="request">要求內容</param>
        protected virtual void LogRequest(string name, RequestPackage request)
        {
            if (!Logger.Instance.IsDebugEnabled) return;
            var info = new ActionLogInfo()
            {
                TraceId = request.Header.TraceId,
                Name = name,
                Action = ActionType.Request,
                Time = request.Header.Time,
                Info = string.Format("SendTo :{0}", Uri),
                Data = JsonConvert.SerializeObject(request.Data)
            };

            Logger.Instance.Debug(info);
        }

        /// <summary>
        ///　紀錄回應內容
        /// </summary>
        /// <param name="name">命令名稱</param>
        /// <param name="response">回應內容</param>
        protected virtual void LogResponse(string name, ResponsePackage response)
        {
            if (!Logger.Instance.IsDebugEnabled) return;
            ResponseHeader header = response.Header;
            var info = new ActionLogInfo()
            {
                TraceId = header.TraceId,
                Name = name,
                Action = ActionType.Response,
                Time = DateTime.Now,
                Info = $"Status :{header.Status} Code :{header.Code} Message :{header.Message}",
                Data = JsonConvert.SerializeObject(response.Data)
            };

            Logger.Instance.Debug(info);
        }
    }
}
