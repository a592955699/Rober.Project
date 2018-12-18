using Rober.Core.Http.Proxy;
using Newtonsoft.Json;
using Rober.Core.Action;
using Rober.Core.Constants;
using Rober.Core.Log;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Rober.Core.Http.Proxy
{
    public class DefaultProxy : ProxyBase
    {
        private string _ipAddress;
        private ActionExecutor _actionExecutor;
        public DefaultProxy(ActionExecutor actionExecutor)
        {
            this._actionExecutor = actionExecutor;
        }
      
        public override bool Action<TRequest, TResponse>(string name, TRequest request, out TResponse response)
        {
            var ipList = Array.FindAll(Dns.GetHostEntry(string.Empty).AddressList, a => a.AddressFamily == AddressFamily.InterNetwork);
            _ipAddress = string.Join<IPAddress>(",", ipList);
            var reqPackage = new RequestPackage
            {
                Header = { FromApp = "JmProject", FromIp = _ipAddress, Time = DateTime.Now },
                Data = request
            };
            return DoAction(name, reqPackage, out response);
        }

        protected override bool DoAction<TResponse>(string name, RequestPackage package, out TResponse response)
        {
            response = null;
            ResponsePackage resPackage = null;
            bool isSucess = false;
            try
            {
                //resPackage = _client.Action(name, JsonConvert.SerializeObject(package));
                resPackage = _actionExecutor.Execute(name, package);
                Message = resPackage.Header.Message;
                Code = resPackage.Header.Code;
                isSucess = resPackage.Header.Status;
            }
            catch (Exception ex)
            {
                Message = "Execute error";
                Code = ResponseCode.SystemError;
                Logger.Instance.Error($"Server :{Name} Exception:{ex.ToString()}");
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
                        response =resPackage.Data as TResponse;
                    }
                    catch (Exception ex)
                    {
                        Code = ResponseCode.DeserializeError;
                        Message = "Convert data fail";
                        Logger.Instance.Error($"Action :{name} Convert data fail. Exception:{ex.ToString()}");
                    }
                }
            }
            finally
            {
                LogResponse(name, resPackage);
            }
            return isSucess;
        }
    }
}
