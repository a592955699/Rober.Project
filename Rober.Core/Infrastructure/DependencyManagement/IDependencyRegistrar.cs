using Autofac;
using Rober.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace Rober.Core.Infrastructure.DependencyManagement
{
    /// <summary>
    /// Dependency registrar interface
    /// </summary>
    public interface IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        /// <param name="configuration"></param>
        void Register(ContainerBuilder builder, ITypeFinder typeFinder, JmConfig config, IConfiguration configuration);

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        int Order { get; }
    }
}
