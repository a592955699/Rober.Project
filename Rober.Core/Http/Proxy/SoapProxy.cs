using System;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using Rober.Core.Action;
using Rober.Core.Constants;
using Rober.Core.Log;
using Newtonsoft.Json;

namespace Rober.Core.Http.Proxy
{
    /// <summary>
    /// 綁定在Tcp通道的Wcf代理
    /// </summary>
    public class SoapProxy : ProxyBase
    {
        private SoapClient _client;

        private string _ipAddress;
        private string _remoteIp;
        private EndpointAddress _endPoint;


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
            _endPoint = new EndpointAddress(Uri);
            _client = new SoapClient(_endPoint);
            _remoteIp = Uri.ToString();

            var ipList = Array.FindAll(Dns.GetHostEntry(string.Empty).AddressList, a => a.AddressFamily == AddressFamily.InterNetwork);
            _ipAddress = string.Join<IPAddress>(",", ipList);
            var reqPackage = new RequestPackage
            {
                Header = { FromApp = "JmProject", FromIp = _ipAddress, Time = DateTime.Now },
                Data = request
            };
            return DoAction(name, reqPackage, out response);
        }


        /// <summary>
        /// 執行動作
        /// </summary>        
        /// <typeparam name="TResponse">回應的內容型別</typeparam>
        /// <param name="name">要執行的動作名稱</param>        
        /// <param name="package">要求的內容</param>
        /// <param name="response">回應內容</param>
        /// <returns>是否執行成功</returns>
        protected override bool DoAction<TResponse>(string name, RequestPackage package, out TResponse response)
        {
            response = null;
            ResponsePackage resPackage = null;
            bool isSucess = false;
            try
            {
                //switch (_client.State)
                //{
                //    case CommunicationState.Created:
                //        _client.Open();
                //        break;
                //    default:
                //        _client.Abort();
                //        _client = new WcfTcpClient(_endPoint);
                //        _client.Open();
                //        break;
                //}

                resPackage = _client.Action(name, JsonConvert.SerializeObject(package));

                Message = resPackage.Header.Message;
                Code = resPackage.Header.Code;
                isSucess = resPackage.Header.Status;
            }
            catch (Exception ex)
            {
                Message = "Execute error";
                Code = ResponseCode.SystemError;
                Logger.Instance.Error($"Server :{Name} {_remoteIp} can't connection{ex.Message}");
            }

            try
            {
                LogRequest(name, package);

                if (isSucess)
                {
                    if (null == resPackage.Data)
                    {
                        response = null;
                        return true;
                    }
                    try
                    {
                        response = JsonConvert.DeserializeObject<TResponse>(resPackage.Data.ToString());
                    }
                    catch (Exception ex)
                    {
                        Code = ResponseCode.DeserializeError;
                        Message = "Deserialize data fail";
                        Logger.Instance.Error($"Action :{name} Deserialize data fail :{resPackage.Data} Data :{ex.Message}");
                    }
                }
            }
            finally
            {
                LogResponse(name, resPackage);
            }
            return isSucess;
        }


        /// <summary>
        /// 釋放資源
        /// </summary>
        public override void Dispose()
        {
            try
            {
                if (null != _client)
                {
                    _client.Close();
                    _client = null;
                }
                _endPoint = null;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex);
            }
        }
    }
}
