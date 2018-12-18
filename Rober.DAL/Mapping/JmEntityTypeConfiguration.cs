using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rober.Model.Mapping
{
    /// <summary>
    /// Entity configuration
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class JmEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : class
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected JmEntityTypeConfiguration()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {
            
        }

        public void Configure(EntityTypeBuilder<T> builder)
        {
           
        }
    }
}