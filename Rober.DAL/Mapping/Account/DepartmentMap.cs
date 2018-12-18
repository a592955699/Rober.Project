using Microsoft.EntityFrameworkCore;
using Rober.Core.Domain.Account;
using Rober.Model.Mapping;

namespace Rober.DAL.Mapping.Account
{
    public partial class DepartmentMap : JmEntityTypeConfiguration<Department>
    {
        public DepartmentMap(ModelBuilder builder)
        {
            builder.Entity<Department>()
                .ToTable("Department")
                .HasKey(x => x.Id);

            //builder.Entity<Department>().Property(x => x.Name).HasMaxLength(50).IsRequired();
            //builder.Entity<Department>().Property(x => x.Remark).HasMaxLength(50);
        }
    }
}
