using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Rober.Core.Http.Middlewares.SoapMiddleware
{
    /// <summary>
    /// 服務介面
    /// </summary>
    [ServiceContract]
    public interface ISoapServer
    {
        /// <summary>
        /// 執行服務內容
        /// </summary>
        /// <param name="name">要執行的服務名稱</param>
        /// <param name="request">要求的封包內容</param>
        /// <returns>回應的封包</returns>
        [OperationContract]
        string Execute(string name, string request);
    }
}
