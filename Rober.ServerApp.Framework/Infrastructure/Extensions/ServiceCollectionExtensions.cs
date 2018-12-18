using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rober.Core.Configuration;
using Rober.Core.Infrastructure;
using System;
using System.Collections.Generic;

namespace Rober.ServerApp.Framework.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        /// <returns>Configured service provider</returns>
        public static IServiceProvider ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCookiePolicyOptions();

            services.AddConfigOptions(configuration);

            var serviceProvider = services.AddEngineContext(configuration);

            return serviceProvider;
        }

        #region 
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

        public static void AddCookiePolicyOptions(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        public static void AddConfigOptions(this IServiceCollection services, IConfiguration configuration)
        {
            //add NopConfig configuration parameters
            services.ConfigureStartupConfig<JmConfig>(configuration.GetSection("Jm"));

            //add hosting configuration parameters
            services.ConfigureStartupConfig<HostingConfig>(configuration.GetSection("Hosting"));

            //add action configuration parameters
            services.ConfigureStartupConfig<List<ActionConfig>>(configuration.GetSection("Actions"));

            //add action configuration parameters
            services.ConfigureStartupConfig<List<InterceptorConfig>>(configuration.GetSection("Interceptors"));
        }

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
        #endregion
    }
}
