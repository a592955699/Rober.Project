using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Domain.Account;
using Microsoft.EntityFrameworkCore;
using Rober.Model.Mapping;
namespace Rober.DAL.Mapping.Account
{
    public partial class UserMenuMappingMap : JmEntityTypeConfiguration<UserMenuMapping>
    {
        public UserMenuMappingMap(ModelBuilder builder)
        {
            builder.Entity<UserMenuMapping>()
                .ToTable("UserMenuMapping")
                .HasKey(x => x.Id);

            //多对多关系
            //builder.Entity<UserMenuMapping>().HasOne(p => p.User)
            //    .WithMany(p => p.UserMenuMappings)
            //    .HasForeignKey(k => k.UserId);
        }
    }
}
