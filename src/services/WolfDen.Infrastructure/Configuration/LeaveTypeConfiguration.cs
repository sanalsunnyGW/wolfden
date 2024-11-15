using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enum;

namespace WolfDen.Infrastructure.Configuration
{
    public class LeaveTypeConfiguration:IEntityTypeConfiguration<LeaveType>
    {
        public void Configure(EntityTypeBuilder<LeaveType> builder)
        {
            builder.Property(x => x.Id).HasColumnName("LeaveTypeId");
            builder.Property(x => x.TypeName).HasMaxLength(100);
            builder.Property(x => x.MaxDays).HasDefaultValue(null);
            builder.Property(x => x.IncrementCount).HasDefaultValue(0);
            builder.Property(x => x.IncrementGap).HasDefaultValue(null);
            builder.Property(x => x.DaysCheck).HasDefaultValue(null);
            builder.Property(x => x.DaysCheckEqualOrLess).HasDefaultValue(0);
            builder.Property(x => x.DaysChekcMore).HasDefaultValue(0);
            builder.Property(x => x.DutyDaysRequired).HasDefaultValue(0);
            builder.Property(x => x.HalfDay).HasDefaultValue(false);
            builder.Property(x => x.CarryForward).HasDefaultValue(false);
            builder.Property(x => x.Hidden).HasDefaultValue(false);
            builder.Property(x => x.RestrictionType).HasDefaultValue(RestrictedLeaveType.Normal);

        }
    }
}
