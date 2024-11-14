using MediatR;
using WolfDen.Domain.Enum;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveTypeCommand
{
    public class LeaveTypeAddCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public int MaxDays { get; set; }
        public bool HalfDays { get; set; }
        public int IncrementCount { get; set; }
        public LeaveIncrementGapMonth IncrementGap { get; set; }
        public bool CarryForward { get; set; }
        public int CarryForwardLimit { get; set; }
        public int DaysCheck { get; set; }
        public int DaysChekcMore { get; set; }
        public int DaysCheckEqualOrLess { get; set; }
        public int DutyDaysRequired { get; set; }
        public bool Hidden { get; set; }
        public RestrictedLeaveType RestrictionType { get; set; }

    }
}
