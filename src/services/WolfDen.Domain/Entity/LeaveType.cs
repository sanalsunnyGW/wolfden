using WolfDen.Domain.Enums;

namespace WolfDen.Domain.Entity
{
    public class LeaveType
    {
        public int Id {  get; private set; }
        public string TypeName { get;private set; }
        public int? MaxDays { get;private set; }
        public bool IsHalfDayAllowed { get; private set; }
        public int IncrementCount { get;private set; }
        public LeaveIncrementGapMonth? IncrementGap { get; private set; }
        public bool CarryForward { get;private set; }
        public int CarryForwardLimit { get; private set; }
        public int? DaysCheck { get; private set; }
        public int DaysChekcMore { get; private set; }
        public int DaysCheckEqualOrLess { get; private set; }
        public int DutyDaysRequired { get; private set; }
        public bool Hidden {  get; private set; }
        public RestrictedLeaveType RestrictionType { get; private set; }
        public bool Sandwich { get; private set; }
        private LeaveType()
        {
            
        }
        public LeaveType(string typeName, int maxDays, bool isHalfDayAllowed, int incrementCount, LeaveIncrementGapMonth incrementGap, bool carryForward, int carryForwardLimit, int daysCheck, int daysChekcMore, int daysCheckEqualOrLess, int dutyDaysRequired, bool hidden, RestrictedLeaveType restrictionType, bool sandwich)
        {
            TypeName = typeName;
            MaxDays = maxDays;
            IsHalfDayAllowed = isHalfDayAllowed;
            IncrementCount = incrementCount;
            IncrementGap = incrementGap;
            CarryForward = carryForward;
            CarryForwardLimit = carryForwardLimit;
            DaysCheck = daysCheck;
            DaysChekcMore = daysChekcMore;
            DaysCheckEqualOrLess = daysCheckEqualOrLess;
            DutyDaysRequired = dutyDaysRequired;
            Hidden = hidden;
            RestrictionType = restrictionType;
            Sandwich = sandwich;
        }
    }
}
