using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WolfDen.Domain.Entity;

namespace WolfDen.Infrastructure.Configuration
{
    public class LeaveSettingConfiguration:IEntityTypeConfiguration<LeaveSetting>
    {
        public void Configure(EntityTypeBuilder<LeaveSetting> builder)
        {
            builder.Property(x => x.Id).HasColumnName("LeaveSettingId");
        }
    }
}
