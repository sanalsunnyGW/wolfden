using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WolfDen.Domain.Entity;

namespace WolfDen.Infrastructure.Configuration
{
    public class AttendenceLogConfiguration : IEntityTypeConfiguration<AttendenceLog>
    {

        public void Configure(EntityTypeBuilder<AttendenceLog> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("LogId");
            builder.Property(x => x.Direction).HasMaxLength(3);

        }
    }
}
