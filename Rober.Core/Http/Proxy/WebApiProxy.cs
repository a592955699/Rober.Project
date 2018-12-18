using System;
using System.IO;
using System.Net;
using System.Text;
using Rober.Core.Action;
using Rober.Core.Constants;
using Rober.Core.Log;
using Newtonsoft.Json;

namespace Rober.Core.Http.Proxy
{
    /// <summary>
    /// 使用WebApi架構的代理
    /// </summary>
    public class WebApiProxy : ProxyBase
    {

        /// <summary>
        /// 執行動作
        /// </summary>
        /// <param name="name">要執行的動作名稱</param>
        /// <typeparam name="TRequest">要求的內容型別</typeparam>
        /// <typeparam name="TResponse">回應的內容型別</typeparam>
        /// <param name="request">要求內容</param>
        /// <param name="response">回應內容</param>
        /// <returns>是否執行成功</returns>
        public override bool Action<TRequest, TResponse>(string name, TRequest request, out TResponse response)
        {
            var reqPackage = new RequestPackage
            {
                Header =
                {
                    FromApp = "JmProejct",
                    FromIp = "",
                    Time = DateTime.Now
                }
            };
            //會在server端重新取得
            reqPackage.Data = request + "";
            return DoAction<TResponse>(name, reqPackage, out response);
        }

        /// <summary>
        /// 執行動作
        /// </summary>
        /// <typeparam name="TResponse">回應的內容型別</typeparam>
        /// <param name="package">要求的內容</param>
        /// <param name="name">要執行的動作名稱</param>        
        /// <param name="response">回應內容</param>
        /// <returns>是否執行成功</returns>
        protected override bool DoAction<TResponse>(string name, RequestPackage package, out TResponse response)
        {
            response = null;
            ResponsePackage resPackage = null;
            bool isSucess = false;
            var responseString = string.Empty;
            try
            {
                var param = package.ToString();
                var bytes = Encoding.ASCII.GetBytes(param);
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Uri);
                req.Method = "POST";
                req.ContentType = "application/x-json";
                req.ContentLength = bytes.Length;
                req.Headers["API"] = name;

                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bytes, 0, bytes.Length);
                }
                using (WebResponse wr = req.GetResponse())
                {
                    StreamReader sr = new StreamReader(wr.GetResponseStream());
                    responseString = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Message = "Execute error";
                //Logger<>.Instance.Error(string.Format("Server :{0} {1} can't connection{2}", Name, Uri, ex.Message));
                Code = ResponseCode.SystemError;
            }

            ResponsePackage responsePackage = null;
            bool isSuccess = false;
            try
            {
                LogRequest(name, package);

                if (string.IsNullOrWhiteSpace(responseString))
                {
                    return false;
                }

                responsePackage = JsonConvert.DeserializeObject<ResponsePackage>(responseString);
                if (responsePackage.Header.Status)
                {

                    if (null == responsePackage.Data)
                    {
                        response = null;
                        return true;
                    }
                    try
                    {
                        response = JsonConvert.DeserializeObject<TResponse>(responsePackage.Data.ToString());
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        Code = ResponseCode.DeserializeError;
                        Message = "Deserialize data fail";
                        Logger.Instance.Error(string.Format("Action :{0} Deserialize data fail :{1} Data :{2}", name, responsePackage.Data, ex.Message));
                    }
                    finally
                    {
                        LogResponse(name, responsePackage);
                    }
                }
                else
                {
                    Code = responsePackage.Header.Code;
                    Message = responsePackage.Header.Message;
                }
            }
            catch (Exception ex)
            {
                Code = ResponseCode.SystemError;
                Message = "Deserialize data fail";
                Logger.Instance.Error(string.Format("Action :{0} Deserialize data fail :{1} Data :{2}", name, responsePackage.Data, ex.Message));
            }
            finally
            {
                LogResponse(name, responsePackage);
            }

            return isSuccess;
        }
    }
}
