using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Action;

namespace Rober.Core.Http.Middlewares.SoapMiddleware
{
    /// <summary>
    /// step 1. Startup 中 ConfigureServices 注入：services.AddSingleton<ActionService>();
    /// step 2.Configure 中配置 app.UseSoapMiddleware<ActionService>("/ActionService.svc", new BasicHttpBinding());
    /// </summary>
    public class SoapServer : ISoapServer
    {
        private ActionExecutor _actionExecutor;
        public SoapServer(ActionExecutor actionExecutor)
        {
            this._actionExecutor = actionExecutor;
        }

        public string Execute(string name, string request)
        {
            return _actionExecutor.Execute(name, request);
        }
    }
}
