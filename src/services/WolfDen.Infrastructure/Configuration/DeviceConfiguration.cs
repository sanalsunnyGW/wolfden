using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WolfDen.Domain.Entity;

namespace WolfDen.Infrastructure.Configuration
{
    public class DeviceConfiguration:IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("DeviceId");
            builder.Property(x => x.Name).HasMaxLength(20);
        }
    }
}
