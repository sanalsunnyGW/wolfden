using FluentValidation;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveTypes.UpdateLeaveType
{
    public class UpdateLeaveTypeValidator: AbstractValidator<UpdateLeaveTypeCommand>
    {
        public UpdateLeaveTypeValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Leave Type is Required");
        }
    }
}



