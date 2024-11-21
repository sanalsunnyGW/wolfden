using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WolfDen.Domain.Entity;

namespace WolfDen.Infrastructure.Configuration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("NotificationId");
            builder.Property(x => x.Message).HasMaxLength(255);
            builder.HasOne(x => x.Employee).WithMany().HasForeignKey(x => x.EmployeeId);

        }
    }
}
