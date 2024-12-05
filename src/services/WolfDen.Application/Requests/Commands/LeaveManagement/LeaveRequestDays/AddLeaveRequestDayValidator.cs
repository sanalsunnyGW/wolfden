using FluentValidation;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequestDays
{
    public class AddLeaveRequestDayValidator : AbstractValidator<AddLeaveRequestDayCommand>
    {
        public AddLeaveRequestDayValidator()
        {
            RuleFor(x => x.LeaveRequestId).NotEmpty().WithMessage("Leave Request Id Is Required");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Date Required");
        }
    }
}
