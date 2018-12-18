using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Domain.Account;
using Microsoft.EntityFrameworkCore;
using Rober.Model.Mapping;
namespace Rober.DAL.Mapping.Account
{
    public partial class RuleMenuMappingMap : JmEntityTypeConfiguration<RuleMenuMapping>
    {
        public RuleMenuMappingMap(ModelBuilder builder)
        {
            builder.Entity<RuleMenuMapping>()
                .ToTable("RuleMenuMapping")
                .HasKey(x => x.Id);

            //多对多关系
            //builder.Entity<RuleMenuMapping>().HasOne(p => p.Rule)
            //    .WithMany(p => p.RuleMenuMappings)
            //    .HasForeignKey(k => k.RuleId);
        }
    }
}
