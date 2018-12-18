using Microsoft.EntityFrameworkCore;
using Rober.Core.Domain.Account;
using Rober.Model.Mapping;

namespace Rober.DAL.Mapping.Account
{
    public partial class MenuMap : JmEntityTypeConfiguration<Menu>
    {
        public MenuMap(ModelBuilder builder)
        {
            builder.Entity<Menu>()
                .ToTable("Menu")
                .HasKey(x => x.Id);

            builder.Entity<Menu>().Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.Entity<Menu>().Property(x => x.Remark).HasMaxLength(500);
            builder.Entity<Menu>().Property(x => x.Link).HasMaxLength(200);
            builder.Entity<Menu>().Property(x => x.Icon).HasMaxLength(200);
            builder.Entity<Menu>().Property(x => x.Published);
        }
    }
}
