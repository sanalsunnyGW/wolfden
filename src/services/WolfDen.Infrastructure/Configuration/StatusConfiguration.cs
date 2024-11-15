using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WolfDen.Domain.Entity;

namespace WolfDen.Infrastructure.Configuration
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("StatusId");
            builder.HasOne(x => x.Employee).WithMany().HasForeignKey(x => x.EmployeeId);
            builder.HasOne(x=>x.StatusType).WithMany().HasForeignKey(x=>x.StatusId);    
        }
    }
}
