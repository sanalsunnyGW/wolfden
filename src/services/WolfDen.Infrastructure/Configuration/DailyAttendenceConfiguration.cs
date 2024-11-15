using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WolfDen.Domain.Entity;

namespace WolfDen.Infrastructure.Configuration
{
    public class DailyAttendenceConfiguration : IEntityTypeConfiguration<DailyAttendence>
    {
        public void Configure(EntityTypeBuilder<DailyAttendence> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("DailyAttendenceId");
          
            builder.Property(x=>x.MissedPunch).IsRequired(false);
          
            
        }
    }
}
