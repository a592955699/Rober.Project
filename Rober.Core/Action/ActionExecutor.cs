using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Rober.Core.Configuration;
using Rober.Core.Constants;
using Rober.Core.Infrastructure;
using Rober.Core.Log;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Rober.Core.Action
{
    public class ActionExecutor
    {
        public ActionExecutor()
        {
            OnInit();
            AfterInit();
        }

        private void OnInit()
        {
            Commands = new Dictionary<string, CommandConfig>();
            Interceptors = new HashSet<INterceptor>();
            Actions = new Dictionary<string, IAction>();

            #region Actions 根据 dll 类中的自动加载 ActionSupport<,>
            var typeFinder = EngineContext.Current.Resolve<ITypeFinder>();
            var assemblies = typeFinder.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(x => x.BaseType != null && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == typeof(ActionSupport<,>));
                foreach (var type in types)
                {
                    try
                    {
                        var action = CreateInstance(type);

                        if (action == null)
                        {
                            Logger.Instance.ErrorFormat("type {0} Object instantiation failure.", type.FullName);
                            continue;
                        }

                        Actions.Add(type.Name, action);
                        Commands.Add(type.Name, new CommandConfig()
                        {
                            Action = action,
                            Interceptors = new List<INterceptor>()
                        });
                    }
                    catch (Exception e)
                    {
                        Logger.Instance.Error(e);
                    }
                }
            }
            #endregion

            #region Interceptors
            var interceptors = EngineContext.Current.Resolve<List<InterceptorConfig>>();
            foreach (var interceptorConfig in interceptors)
            {
                try
                {
                    Type type = Type.GetType(interceptorConfig.Type, true, true);
                    var interceptor = (INterceptor)Activator.CreateInstance(type);
                    Interceptors.Add(interceptor);

                    CommonHelper.SetProperty(interceptor, "ExcludeCommands", interceptorConfig.ExcludeCommands);
                    CommonHelper.SetProperty(interceptor, "IncluedCommands", interceptorConfig.IncluedCommands);
                }
                catch (Exception e)
                {
                    Logger.Instance.Error(e);
                }
            }
            #endregion
        }

        private IAction CreateInstance(Type type)
        {
            IAction action;
            ConstructorInfo[] ci = type.GetConstructors();    //获取类的所有构造函数
            if (ci.Length > 1)
            {
                Logger.Instance.ErrorFormat("type {0} too many contructors", type.FullName);
                return null;
            }
            else if (ci.Length == 1)
            {
                var c = ci[0];
                ParameterInfo[] ps = c.GetParameters();    //取出每个构造函数的所有参数
                var consPars = new object[ps.Length];
                for (int i = 0; i < ps.Length; i++)
                {
                    ParameterInfo pi = ps[i];
                    consPars[i] = EngineContext.Current.Resolve(pi.ParameterType);
                }

                action = (IAction)c.Invoke(consPars);
            }
            else
            {
                action = (IAction)Activator.CreateInstance(type);
            }
            return action;
        }

        private void AfterInit()
        {
            if (Interceptors == null || !Interceptors.Any()) return;
            foreach (INterceptor interceptor in Interceptors)
            {
                foreach (KeyValuePair<String, CommandConfig> entry in Commands)
                {
                    bool isApply = true;

                    if (interceptor is CommandFilterInterceptor)
                    {
                        String commandName = entry.Key;
                        isApply = ((CommandFilterInterceptor)interceptor).ApplyInterceptor(commandName);
                    }

                    if (isApply)
                    {
                        CommandConfig config = entry.Value;
                        config.AddInterceptor(interceptor);
                    }
                }
            }
        }

        public ActionInvocation DoExecute(string command, RequestHeader requestHeader, object request)
        {
            Commands.TryGetValue(command, out var config);
            if (config == null)
            {
                throw new Exception("command not found! command: " + command);
            }

            config.Action = CreateInstance(config.Action.GetType());//ActionExecutor 单例，DbContext 非单例模式下，需要重新实例化 Action 

            var invocation = new ActionInvocation
            {
                Action = config.Action,
                Command = command,
                Config = config,
                Request = request,
                RequestHeader = requestHeader
            };

            if (config.Interceptors != null)
            {
                invocation.Interceptors = config.Interceptors.GetEnumerator();
            }

            invocation.Invoke();

            if (invocation.Response == null && invocation.Code == ResponseCode.Success)
                invocation.Code = ResponseCode.SystemError;

            return invocation;
        }

        /// <summary>
        /// 執行服務內容
        /// </summary>
        /// <param name="request">要求的內容</param>
        /// <param name="command">要執行的服務名稱</param>
        /// <returns>回應的內容。ResponsePackage.Data是序列化后的字符串</returns>
        public string Execute(string command, string request)
        {
            var response = new ResponsePackage();
            try
            {
                var requestPackage = JsonConvert.DeserializeObject<RequestPackage>(request);
                response.Header.TraceId = requestPackage.Header.TraceId;

                var actionInvocation = DoExecute(command, requestPackage.Header, requestPackage.Data);
                response.Header.Code = actionInvocation.Code;
                response.Data = JsonConvert.SerializeObject(actionInvocation.Response);
            }
            catch (Exception ex)
            {
                response.Header.Message = ex.Message;
                response.Header.Code = ResponseCode.SystemError;
                Logger.Instance.Error(ex);
            }
            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// 執行服務內容
        /// </summary>
        /// <param name="command"></param>
        /// <param name="requestPackage"></param>
        /// <returns>回應的內容。ResponsePackage.Data直接是对象而不是序列化后的字符串</returns>
        public ResponsePackage Execute(string command, RequestPackage requestPackage)
        {
            var response = new ResponsePackage();
            try
            {
                response.Header.TraceId = requestPackage.Header.TraceId;

                var actionInvocation = DoExecute(command, requestPackage.Header, requestPackage.Data);
                response.Header.Code = actionInvocation.Code;
                response.Data = actionInvocation.Response;
            }
            catch (Exception ex)
            {
                response.Header.Message = ex.Message;
                response.Header.Code = ResponseCode.SystemError;
                Logger.Instance.Error(ex);
            }
            return response;
        }

        public TResponse Execute<TRequest, TResponse>(string command, TRequest request)
            where TRequest : Request, new()
            where TResponse : Response, new()
        {
            Commands.TryGetValue(command, out var config);
            if (config == null)
            {
                throw new Exception("command not found! command: " + command);
            }

            var invocation = new ActionInvocation
            {
                Action = config.Action,
                Command = command,
                Config = config,
                Request = request,
                RequestHeader = null
            };
            if (config.Interceptors != null)
            {
                invocation.Interceptors = config.Interceptors.GetEnumerator();
            }

            invocation.Invoke();

            if (invocation.Response == null && invocation.Code == ResponseCode.Success)
                invocation.Code = ResponseCode.SystemError;

            return (TResponse)invocation.Response;
        }

        public IDictionary<string, CommandConfig> Commands { get; set; }
        public ISet<INterceptor> Interceptors { get; set; }
        public IDictionary<string, IAction> Actions { get; set; }
    }
}
