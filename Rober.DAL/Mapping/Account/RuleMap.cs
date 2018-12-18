using Microsoft.EntityFrameworkCore;
using Rober.Core.Domain.Account;
using Rober.Model.Mapping;
namespace Rober.DAL.Mapping.Account
{
    public partial class RuleMap : JmEntityTypeConfiguration<Rule>
    {
        public RuleMap(ModelBuilder builder)
        {
            builder.Entity<Rule>()
                .ToTable("Rule")
                .HasKey(x => x.Id);

            builder.Entity<Rule>().Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.Entity<Rule>().Property(x => x.Remark).HasMaxLength(50);
            builder.Entity<Rule>().Property(x => x.Published).IsRequired();
        }
    }
}
