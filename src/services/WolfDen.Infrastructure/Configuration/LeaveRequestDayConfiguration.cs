using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WolfDen.Domain.Entity;

namespace WolfDen.Infrastructure.Configuration
{
    public class LeaveRequestDayConfiguration:IEntityTypeConfiguration<LeaveRequestDay>
    {
        public void Configure(EntityTypeBuilder<LeaveRequestDay> builder)
        {
            builder.Property(x => x.Id).HasColumnName("LeaveRequestDayId");
            builder.HasOne(x => x.LeaveRequest).WithMany().HasForeignKey(x => x.LeaveRequestId);

        }
    }
}
