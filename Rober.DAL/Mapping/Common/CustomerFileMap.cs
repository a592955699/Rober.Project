using Rober.Core.Domain.Common;
using Rober.Core.Domain.Schedules;
using Microsoft.EntityFrameworkCore;

namespace Rober.Model.Mapping.Common
{
    public partial class CustomerFileMap : JmEntityTypeConfiguration<CustomerFile>
    {
        public CustomerFileMap(ModelBuilder builder)
        {
            builder.Entity<CustomerFile>()
                .ToTable("CustomerFile")
                .HasKey(x => x.Id);
        }
    }
}
