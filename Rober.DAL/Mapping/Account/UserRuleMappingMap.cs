using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Domain.Account;
using Microsoft.EntityFrameworkCore;
using Rober.Model.Mapping;
namespace Rober.DAL.Mapping.Account
{
    public partial class UserRuleMappingMap : JmEntityTypeConfiguration<UserRuleMapping>
    {
        public UserRuleMappingMap(ModelBuilder builder)
        {
            builder.Entity<UserRuleMapping>()
                .ToTable("UserRuleMapping")
                .HasKey(x => x.Id);

            //多对多关系
            //builder.Entity<UserRuleMapping>().HasOne(p => p.User)
            //    .WithMany(p => p.UserRuleMappings)
            //    .HasForeignKey(k => k.UserId);
        }
    }
}
