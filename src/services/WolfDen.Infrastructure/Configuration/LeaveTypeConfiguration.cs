using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;

namespace WolfDen.Infrastructure.Configuration
{
    public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
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
            builder.HasData(
                    new
                    {
                        Id = 1,
                        TypeName = "Casual Leave",
                        MaxDays = 12,
                        IsHalfDayAllowed = true,
                        IncrementCount = 1,
                        IncrementGapId = LeaveIncrementGapMonth.Month,
                        CarryForward = false,
                        CarryForwardLimit = 0,
                        DaysCheck = 2,
                        DaysCheckMore = 7,
                        DaysCheckEqualOrLess = 2,
                        DutyDaysRequired = 0,
                        Sandwich = false,
                        LeaveCategoryId = LeaveCategory.CasualLeave
                    },
                    new
                    {
                        Id = 2,
                        TypeName = "Priveleged Leave",
                        MaxDays = 12,
                        IsHalfDayAllowed = false,
                        IncrementCount = 0,
                        IncrementGapId = (LeaveIncrementGapMonth?)null,
                        CarryForward = true,
                        CarryForwardLimit = 24,
                        DaysCheck = 3,
                        DaysCheckMore = 21,
                        DaysCheckEqualOrLess = 7,
                        DutyDaysRequired = 365,
                        Sandwich = true,
                        LeaveCategoryId = LeaveCategory.PrivilegeLeave
                    },
                    new
                    {
                        Id = 3,
                        TypeName = "Bereavement Leave",
                        MaxDays = 1,
                        IsHalfDayAllowed = false,
                        IncrementCount = 0,
                        IncrementGapId = (LeaveIncrementGapMonth?)null,
                        CarryForward = false,
                        CarryForwardLimit = 0,
                        DaysCheck = (int?)null,
                        DaysCheckMore = (int?)null,
                        DaysCheckEqualOrLess = (int?)null,
                        DutyDaysRequired = 0,
                        Sandwich = false,
                        LeaveCategoryId = LeaveCategory.BereavementLeave
                    },
                    new
                    {
                        Id = 4,
                        TypeName = "Emergency Leave",
                        MaxDays = 2,
                        IsHalfDayAllowed = false,
                        IncrementCount = 2,
                        IncrementGapId = LeaveIncrementGapMonth.Quarter,
                        CarryForward = false,
                        CarryForwardLimit = 0,
                        DaysCheck = (int?)null,
                        DaysCheckMore = (int?)null,
                        DaysCheckEqualOrLess = (int?)null,
                        DutyDaysRequired = 0,
                        Sandwich = false,
                        LeaveCategoryId = LeaveCategory.EmergencyLeave
                    },
                    new
                    {
                        Id = 5,
                        TypeName = "Restricted Leave",
                        MaxDays = 2,
                        IsHalfDayAllowed = false,
                        IncrementCount = 0,
                        IncrementGapId = (LeaveIncrementGapMonth?)null,
                        CarryForward = false,
                        CarryForwardLimit = 0,
                        DaysCheck = 1,
                        DaysCheckMore = 2,
                        DaysCheckEqualOrLess = 2,
                        DutyDaysRequired = 0,
                        Sandwich = false,
                        LeaveCategoryId = LeaveCategory.RestrictedHoliday
                    },
                    new
                    {
                        Id = 6,
                        TypeName = "WorK From Home",
                        MaxDays = 0,
                        IsHalfDayAllowed = false,
                        IncrementCount = 0,
                        IncrementGapId = (LeaveIncrementGapMonth?)null,
                        CarryForward = false,
                        CarryForwardLimit = 0,
                        DaysCheck = 1,
                        DaysCheckMore = 1,
                        DaysCheckEqualOrLess = 1,
                        DutyDaysRequired = 0,
                        Sandwich = false,
                        LeaveCategoryId = LeaveCategory.WorkFromHome
                    },
                    new
                    {
                        Id = 7,
                        TypeName = "Maternity Leave",
                        MaxDays = 184,
                        IsHalfDayAllowed = false,
                        IncrementCount = 0,
                        IncrementGapId = (LeaveIncrementGapMonth?)null,
                        CarryForward = false,
                        CarryForwardLimit = 0,
                        DaysCheck = 1,
                        DaysCheckMore = 1,
                        DaysCheckEqualOrLess = 1,
                        DutyDaysRequired = 80,
                        Sandwich = true,
                        LeaveCategoryId = LeaveCategory.Maternity
                    },
                    new
                    {
                        Id = 8,
                        TypeName = "Paternity Leave",
                        MaxDays = 2,
                        IsHalfDayAllowed = false,
                        IncrementCount = 0,
                        IncrementGapId = (LeaveIncrementGapMonth?)null,
                        CarryForward = false,
                        CarryForwardLimit = 0,
                        DaysCheck = 1,
                        DaysCheckMore = 1,
                        DaysCheckEqualOrLess = 1,
                        DutyDaysRequired = 0,
                        Sandwich = false,
                        LeaveCategoryId = LeaveCategory.Paternity
                    }
               );
        }
    }
}
