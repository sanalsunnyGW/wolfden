using WolfDen.Domain.Enums;

namespace WolfDen.Domain.Entity
{
    public class LeaveType
    {
        public int Id {  get; private set; }
        public string TypeName { get;private set; }
        public int? MaxDays { get;private set; }
        public bool? IsHalfDayAllowed { get; private set; }
        public int? IncrementCount { get;private set; }
        public LeaveIncrementGapMonth? IncrementGapId { get; private set; }
        public bool? CarryForward { get;private set; }
        public int? CarryForwardLimit { get; private set; }
        public int? DaysCheck { get; private set; }
        public int? DaysCheckMore { get; private set; }
        public int? DaysCheckEqualOrLess { get; private set; }
        public int? DutyDaysRequired { get; private set; }
        public bool? Sandwich { get; private set; }
        public LeaveCategory? LeaveCategoryId { get; private set; }

        private LeaveType()
        {
            
        }
        public LeaveType(string typeName, int? maxDays, bool? isHalfDayAllowed, int? incrementCount, LeaveIncrementGapMonth? incrementGapId, bool? carryForward, int? carryForwardLimit, int? daysCheck, int? daysCheckMore, int? daysCheckEqualOrLess, int? dutyDaysRequired, bool? sandwich)
        {
            TypeName = typeName;
            MaxDays = maxDays;
            IsHalfDayAllowed = isHalfDayAllowed;
            IncrementCount = incrementCount;
            IncrementGapId = incrementGapId;
            CarryForward = carryForward;
            CarryForwardLimit = carryForwardLimit;
            DaysCheck = daysCheck;
            DaysCheckMore = daysCheckMore;
            DaysCheckEqualOrLess = daysCheckEqualOrLess;
            DutyDaysRequired = dutyDaysRequired;
            Sandwich = sandwich;
        }
    }
}
