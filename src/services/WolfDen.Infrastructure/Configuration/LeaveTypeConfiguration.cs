using Microsoft.Data.SqlClient;
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
            builder.Property(x => x.LeaveCategoryId).HasDefaultValue(LeaveCategory.Custom);
            builder.Property(x => x.Sandwich).HasDefaultValue(false);
            builder.Property(x => x.DutyDaysRequired).HasDefaultValue(0);
            builder.Property(x => x.MaxDays).HasDefaultValue(0);
            builder.Property(x => x.DaysCheck).HasDefaultValue(1);
            builder.Property(x => x.DaysCheckMore).HasDefaultValue(1);   
            builder.Property(x => x.DaysCheckEqualOrLess).HasDefaultValue(1);
            builder.Property(x => x.IncrementCount).HasDefaultValue(0);
        }
    }
}
