using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;

namespace WolfDen.Infrastructure.Configuration
{
    public class LeaveTypeConfiguration:IEntityTypeConfiguration<LeaveType>
    {
        public void Configure(EntityTypeBuilder<LeaveType> builder)
        {
            builder.Property(x => x.Id).HasColumnName("LeaveTypeId");
            builder.Property(x => x.TypeName).HasMaxLength(100);
            builder.Property(x => x.IsHalfDayAllowed).HasDefaultValue(false);
            builder.Property(x => x.CarryForward).HasDefaultValue(false);
            builder.Property(x => x.RestrictionType).HasDefaultValue(RestrictedLeaveType.Normal);
            builder.Property(x => x.Sandwich).HasDefaultValue(false);

        }
    }
}
