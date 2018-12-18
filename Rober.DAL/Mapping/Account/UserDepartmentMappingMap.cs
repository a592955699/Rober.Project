using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Domain.Account;
using Microsoft.EntityFrameworkCore;
using Rober.Model.Mapping;
namespace Rober.DAL.Mapping.Account
{
    public partial class UserDepartmentMappingMap : JmEntityTypeConfiguration<UserDepartmentMapping>
    {
        public UserDepartmentMappingMap(ModelBuilder builder)
        {
            builder.Entity<UserDepartmentMapping>()
                .ToTable("UserDepartmentMapping")
                .HasKey(x => x.Id);

            //多对多关系
            //builder.Entity<UserDepartmentMapping>().HasOne(p => p.User)
            //    .WithMany(p => p.UserDepartmentMappings)
            //    .HasForeignKey(k => k.UserId);
        }
    }
}
