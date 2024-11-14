using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WolfDen.Domain.Entity;

namespace WolfDen.Infrastructure.Configuration
{
    public class LeaveIncrementLogConfiguration:IEntityTypeConfiguration<LeaveIncrementLog>
    {
        public void Configure(EntityTypeBuilder<LeaveIncrementLog> Builder)
        {
            Builder.Property(x => x.Id).HasColumnName("LeaveIncrementLogId");
            Builder.HasOne(x => x.LeaveBalance).WithMany().HasForeignKey(x => x.LeaveBalanceId);
        }


    }
}
