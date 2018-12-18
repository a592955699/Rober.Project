using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rober.Core.Configuration;
using Rober.Core.Extensions;
using Rober.Core.Infrastructure;
using Rober.DAL;
using Rober.DAL.Repository;
using Rober.IDAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rober.WebApp
{
    public class MvcStartup : IJmStartup
    {
        public int Order => 0;

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
           
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
        }
    }
}
