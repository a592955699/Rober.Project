using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Rober.Core.Http.Middlewares.SoapMiddleware;
using Rober.Core.Infrastructure;
using System.ServiceModel;

namespace Rober.ServerApp.Framework.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure the application HTTP request pipeline
        /// </summary>
        /// <param name="app">Builder for configuring an application's request pipeline</param>
        /// <param name="env"></param>
        public static void ConfigureRequestPipeline(this IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseException(env);
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMvc();
            app.UseSoapMiddleware();
            EngineContext.Current.ConfigureRequestPipeline(app, env);
        }


        #region

        public static void UseSoapMiddleware(this IApplicationBuilder app)
        {
            app.UseSoapMiddleware<SoapServer>("/SoapService.svc", new BasicHttpBinding());
        }
        public static void UseException(this IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
        }
        public static void UseMvc(this IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        #endregion
    }
}
