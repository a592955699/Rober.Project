using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Rober.Core.Infrastructure;

namespace Rober.WebApp.Framework.Infrastructure.Extensions
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
            app.UseSession();
            app.UseException(env);
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(env);
            EngineContext.Current.ConfigureRequestPipeline(app, env);
        }

        #region
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
        public static void UseMvc(this IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=SignIn}/{id?}");
            });
        }
        #endregion
    }
}
