using Autofac;
using JM.Portal.App.Framework.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Rober.Core;
using Rober.Core.Action;
using Rober.Core.Caching;
using Rober.Core.Configuration;
using Rober.Core.Infrastructure;
using Rober.Core.Infrastructure.DependencyManagement;
using Rober.Core.Plugins;
using Rober.Core.Session;
using Rober.DAL;
using Rober.DAL.Repository;
using Rober.IDAL;
using Rober.IDAL.Repository;
using Rober.WebApp.Framework.Proxy;
using Rober.WebApp.Framework.UI;

namespace Rober.WebApp.Framework.Infrastructure
{
    public class DependencyRegistrar: IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, JmConfig config, IConfiguration configuration)
        {
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            builder.RegisterType<PageHeadBuilder>().As<IPageHeadBuilder>().InstancePerLifetimeScope();

            //plugins
            builder.RegisterType<PluginFinder>().As<IPluginFinder>().InstancePerLifetimeScope();
            builder.RegisterType<OfficialFeedManager>().As<IOfficialFeedManager>().InstancePerLifetimeScope();

            //cache manager
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().InstancePerLifetimeScope();

            //static cache manager
            if (config.RedisCachingEnabled)
            {
                builder.RegisterType<RedisConnectionWrapper>().As<IRedisConnectionWrapper>().SingleInstance();
                builder.RegisterType<RedisCacheManager>().As<IStaticCacheManager>().InstancePerLifetimeScope();
                builder.RegisterType<UserSessionCacheManager>().As<IUserSessionCacheManager>().InstancePerLifetimeScope();
            }
            else
                builder.RegisterType<MemoryCacheManager>().As<IStaticCacheManager>().SingleInstance();

            builder.RegisterType<ActionProxy>().As<IActionProxy>().InstancePerLifetimeScope();

            builder.Register<ISessionServer>(c => new SessionServer(new SessionServerOptions()
            {
                SessionListener = new RedisSessionListener(),
                ExpiredTime = 20
            })).SingleInstance();

            #region 如果是使用 Soap 调用 Action,则此处不需要
            builder.RegisterType<ActionExecutor>().SingleInstance();

            var optionsBuilder = new DbContextOptionsBuilder<JmObjectContext>();
            //sql server 高版本用这行默认分页方式
            //optionsBuilder.UseSqlServer(configuration.GetConnectionString("JmObjectContext"));
            //sql server 低版本用这样，设置分页方式
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("JmObjectContext"), option => option.UseRowNumberForPaging());
            //builder.Register<IDbContext>(c => new JmObjectContext(optionsBuilder.Options)).InstancePerLifetimeScope();
            builder.Register(x => new JmObjectContext(optionsBuilder.Options)).As<IDbContext>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<EfUnitOfWork>().As<IEfUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserRuleMappingRepository>().As<IUserRuleMappingRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserDepartmentMappingRepository>().As<IUserDepartmentMappingRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MenuRepository>().As<IMenuRepository>().InstancePerLifetimeScope();
            builder.RegisterType<GenericAttributeRepository>().As<IGenericAttributeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RuleRepository>().As<IRuleRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DepartmentRepository>().As<IDepartmentRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerFileRepository>().As<ICustomerFileRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ScheduleRepository>().As<IScheduleRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ScheduleSubCategoryRepository>().As<IScheduleSubCategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType <ScheduleMappingRepository>().As<IScheduleMappingRepository>().InstancePerLifetimeScope();
            #endregion
        }

        public int Order => 0;
    }
}
