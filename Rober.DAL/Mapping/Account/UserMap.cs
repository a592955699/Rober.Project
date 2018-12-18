using Microsoft.EntityFrameworkCore;
using Rober.Core.Domain.Account;
using Rober.Model.Mapping;
namespace Rober.DAL.Mapping.Account
{
    public partial class UserMap : JmEntityTypeConfiguration<User>
    {
        public UserMap(ModelBuilder builder)
        {
            builder.Entity<User>()
                .ToTable("User")
                .HasKey(x => x.Id);

            builder.Entity<User>().Property(x => x.UserName).HasMaxLength(50).IsRequired();
            builder.Entity<User>().Property(x => x.Password).HasMaxLength(50).IsRequired();
            builder.Entity<User>().Property(x => x.NickName).HasMaxLength(50);
            builder.Entity<User>().Property(x => x.LoginIp).HasMaxLength(20);
            builder.Entity<User>().Ignore(x => x.Roles);
            builder.Entity<User>().Ignore(x => x.Departments);
        }
    }
}
