using FluentValidation;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.RevokeLeaveRequest
{
    public class RevokeLeaveRequestValidator : AbstractValidator<RevokeLeaveRequestCommand>
    {
        public RevokeLeaveRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Leave Request Id required");
        }
    }
}
