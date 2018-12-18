using System.IO;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace Rober.Core.Log
{
    public class Logger
    {
        private static readonly object LockObj = new object();
        private static ILog _instance;
        /// <summary>
        /// 录器实体
        /// </summary>
        public static ILog Instance
        {
            get
            {
                if (null != _instance) return _instance;
                lock (LockObj)
                {
                    if (null == _instance)
                    {
                        _instance = CreateLogger();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 根据配置文档创建记录器实体，目前直接写死 Log4NetAdapter
        /// </summary>
        /// <returns>录器实体</returns>
        public static ILog CreateLogger()
        {
            ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
            // 默认简单配置，输出至控制台
            //BasicConfigurator.Configure(repository);
            //log4net 配置
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            var log = LogManager.GetLogger(repository.Name, "NETCorelog4net");
            log.Debug("Logger CreateLogger");
            return log;
        }
    }
}
