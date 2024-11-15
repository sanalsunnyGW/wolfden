using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WolfDen.Domain.Entity;

namespace WolfDen.Infrastructure.Configuration
{
    public class AttendenceCloseConfiguration : IEntityTypeConfiguration<AttendenceClose>
    {
        public void Configure(EntityTypeBuilder<AttendenceClose> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("AttendenceCloseId"); 
        }
    }
}
