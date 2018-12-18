using System;
using System.ServiceModel;
using Rober.Core.Action;
using Rober.Core.Constants;
using Rober.Core.Http.Middlewares.SoapMiddleware;
using Rober.Core.Log;
using Newtonsoft.Json;

namespace Rober.Core.Http.Proxy
{
    /// <summary>
    /// 綁定在Tcp通道的Wcf用戶端
    /// </summary>
    public class SoapClient : ClientBase<ISoapServer>
    {
        public SoapClient(string remoteAddress) : base(new BasicHttpBinding(), new EndpointAddress(remoteAddress))
        {
        }
        public SoapClient(EndpointAddress remoteAddress) : base(new BasicHttpBinding(), remoteAddress)
        {
        }
        public SoapClient(BasicHttpBinding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
        }

        /// <summary>
        /// 執行服務內容
        /// </summary>
        /// <param name="name">要執行的服務名稱</param>
        /// <param name="request">要求的封包</param>
        /// <returns>是否執行成功</returns>
        public ResponsePackage Action(string name, string request)
        {
            try
            {
                var result = Channel.Execute(name, request);
                var response = JsonConvert.DeserializeObject<ResponsePackage>(result);
                return response;
            }
            catch (Exception e)
            {
                Logger.Instance.Error(e);

                return new ResponsePackage()
                {
                    Header = new ResponseHeader()
                    {
                        Code = ResponseCode.SystemError,
                        Message = e.Message
                    }
                };
            }
        }
    }
}
