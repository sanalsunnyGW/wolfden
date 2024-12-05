using FluentValidation;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.EditLeaveRequest
{
    public class EditLeaveRequestValidator : AbstractValidator<EditLeaveRequestCommand>
    {
        public EditLeaveRequestValidator()
        {
            RuleFor(x => x.EmpId).NotEmpty().WithMessage("Employee Id required");
            RuleFor(x => x.TypeId).NotEmpty().WithMessage("Type Id required");
            RuleFor(x => x.FromDate).NotEmpty().WithMessage("From Date required");
            RuleFor(x => x.ToDate).NotEmpty().WithMessage("To Date required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Leave Description required");
        }
    }
}
