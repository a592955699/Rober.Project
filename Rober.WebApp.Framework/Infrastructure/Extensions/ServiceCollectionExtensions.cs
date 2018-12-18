using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rober.Core.Configuration;
using Rober.Core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Rober.Core.Constants;
using Rober.WebApp.Framework.MVC.ModelBinding;
using JM.Portal.Web.Framework.Attributes;

namespace Rober.WebApp.Framework.Infrastructure.Extensions
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
            services.AddCookiePolicyOptions();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(LoginAuthorizeAttribute)); // by type

                //options.Filters.Add(new SampleGlobalActionFilter()); // an instance

                options.ModelMetadataDetailsProviders.Add(new JmMetadataProvider());

                //自定义ModelBinder
                //options.ModelBinderProviders.Insert(0, new JmModelBinderProvider());

            })//add custom display metadata provider
            .AddJsonOptions(options => { options.SerializerSettings.ContractResolver = new DefaultContractResolver(); })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);



            // Add cookie authentication so that it's possible to sign-in to test the 
            // custom authorization policy behavior of the sample
            services.AddAuthentication(SessionConstants.AuthenticationScheme)
                .AddCookie(SessionConstants.AuthenticationScheme, options =>
                {
                    options.AccessDeniedPath = "/Account/Denied";
                    options.LoginPath = "/Account/SignIn";
                    options.LogoutPath = "/Account/Signout";
                });

            services.AddConfigOptions(configuration);

            //内存中存储session
            services.AddDistributedMemoryCache();

            //redis 中存储session
            //var redisCachingConnectionString = configuration.GetValue<string>("Jm:RedisCachingConnectionString");
            //services.AddDistributedRedisCache(option => option.Configuration = redisCachingConnectionString);

            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(20); });

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
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }


        public static void AddConfigOptions(this IServiceCollection services, IConfiguration configuration)
        {
            //add NopConfig configuration parameters
            services.ConfigureStartupConfig<JmConfig>(configuration.GetSection("Jm"));

            //add hosting configuration parameters
            services.ConfigureStartupConfig<HostingConfig>(configuration.GetSection("Hosting"));

            #region 如果使用 soap 调用 action 此处则不需要
            //add action configuration parameters
            services.ConfigureStartupConfig<List<ActionConfig>>(configuration.GetSection("Actions"));

            //add action configuration parameters
            services.ConfigureStartupConfig<List<InterceptorConfig>>(configuration.GetSection("Interceptors")); 
            #endregion

            //add net configuration parameters
            services.ConfigureStartupConfig<List<NetConfig>>(configuration.GetSection("Nets"));
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
