using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WolfDen.Domain.Entity;

namespace WolfDen.Infrastructure.Configuration
{
    public class LeaveRequestConfiguration:IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            builder.Property(x => x.Id).HasColumnName("LeaveRequestId");
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.HasOne(x => x.Employee).WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict); 
            builder.HasOne(x=>x.LeaveType).WithMany().HasForeignKey(x => x.TypeId);
            builder.HasOne(x => x.Manager).WithMany().HasForeignKey(x => x.ProcessedBy ).OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
