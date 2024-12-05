using FluentValidation;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.ApproveOrRejectLeaveRequest
{
    public class ApproveOrRejectLeaveValidator : AbstractValidator<ApproveOrRejectLeaveRequestCommand>
    {
        public ApproveOrRejectLeaveValidator()
        {
            RuleFor(x => x.LeaveRequestId).NotEmpty().WithMessage("Leave Request Required");
            RuleFor(x => x.statusId).NotEmpty().WithMessage("Status Required");
            RuleFor(x => x.SuperiorId).NotEmpty().WithMessage("Superior Required");
        }
    }
}
