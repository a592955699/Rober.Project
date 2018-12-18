using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rober.Core.Configuration;
using Rober.Core.Extensions;
using Rober.Core.Log;
using Rober.DAL;
using Rober.DAL.Repository;
using Rober.IDAL.Repository;
using Rober.WebApp.Framework.Infrastructure.Extensions;

namespace Rober.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<JmObjectContext>(options =>
            //   options.UseSqlServer(Configuration.GetConnectionString("JmObjectContext"), x => x.UseRowNumberForPaging())
            //);

            var serviceProvider = services.ConfigureApplicationServices(Configuration);
            Rober.Core.Http.Context.HttpContext.ServiceProvider = serviceProvider;
            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            lifetime.ApplicationStarted.Register(RegService);//1:应用启动时加载配置,2:应用启动后注册服务中心
            app.ConfigureRequestPipeline(env);
            lifetime.ApplicationStopped.Register(UnRegService);//应用停止后从服务中心注销
        }

        #region
        private void RegService()
        {
            //先判断是否已经注册过了
            //this code is called when the application stops
            Logger.Instance.Debug("ApplicationStarted:RegService");
        }
        private void UnRegService()
        {
            //this code is called when the application stops
            Logger.Instance.Debug("ApplicationStopped:UnRegService");
        }
        #endregion
    }
}
