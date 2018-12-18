using Rober.Core.Action;
using Rober.Core.Configuration;
using Rober.Core.Http.Proxy;
using Rober.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rober.Core.Http.Proxy
{
    /// <summary>
    /// 代理工廠
    /// </summary>
    public class ProxyFactory
    {
        /// <summary>
        /// 取得代理實體
        /// </summary>
        /// <param name="name">代理名稱</param>
        /// <returns>代理實體</returns>
        public static IProxy GetProxy(string name)
        {
            var nets = EngineContext.Current.Resolve<List<NetConfig>>();

            NetConfig setting = nets.Find(x => name == x.Name);
            Type tProxy = Type.GetType(setting.Porxy);
            IProxy iproxy = (IProxy)Activator.CreateInstance(tProxy);

            PropertyInfo namePi = tProxy.GetProperty("Name");
            namePi.SetValue(iproxy, setting.Name);

            PropertyInfo uriPi = tProxy.GetProperty("Uri");
            uriPi.SetValue(iproxy, setting.Uri);

            return iproxy;
        }
        public static IProxy GetDefaultProxy()
        {
            //return GetProxy("SoapProxy");
            return new DefaultProxy(EngineContext.Current.Resolve<ActionExecutor>());
        }
    }
}
