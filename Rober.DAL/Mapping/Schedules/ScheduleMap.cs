using Rober.Core.Domain.Schedules;
using Microsoft.EntityFrameworkCore;

namespace Rober.Model.Mapping.Schedules
{
    public partial class ScheduleMap : JmEntityTypeConfiguration<Schedule>
    {
        public ScheduleMap(ModelBuilder builder)
        {
            builder.Entity<Schedule>()
                .ToTable("Schedule")
                .HasKey(x => x.Id);

            builder.Entity<Schedule>().Property(x => x.Title).HasMaxLength(50).IsRequired();
            builder.Entity<Schedule>().Ignore(x => x.Users);
            builder.Entity<Schedule>().Ignore(x => x.Files);
            builder.Entity<Schedule>().HasIndex(x => x.StatusId);
            builder.Entity<Schedule>().HasIndex(x => x.SubCategoryId);
        }
    }
}
