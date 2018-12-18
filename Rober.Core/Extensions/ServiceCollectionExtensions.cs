using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rober.Core.Configuration;
using Rober.Core.Infrastructure;

namespace Rober.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceProvider AddEngineContext(this IServiceCollection services, IConfiguration configuration)
        {
            //create, initialize and configure the engine
            var engine = EngineContext.Create();
            engine.Initialize(services);
            var serviceProvider = engine.ConfigureServices(services, configuration);

            //if (DataSettingsHelper.DatabaseIsInstalled())
            //{
            //    //implement schedule tasks
            //    //database is already installed, so start scheduled tasks
            //    TaskManager.Instance.Initialize();
            //    TaskManager.Instance.Start();

            //    //log application start
            //    EngineContext.Current.Resolve<ILogger>().Information("Application started", null, null);
            //}
            return serviceProvider;
        }

        /// <summary>
        /// Create, bind and register as service the specified configuration parameters 
        /// </summary>
        /// <typeparam name="TConfig">Configuration parameters</typeparam>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Set of key/value application configuration properties</param>
        /// <returns>Instance of configuration parameters</returns>
        public static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //create instance of config
            var config = new TConfig();

            //bind it to the appropriate section of configuration
            configuration.Bind(config);

            //and register it as a service
            services.AddSingleton(config);

            return config;
        }

    }
}
