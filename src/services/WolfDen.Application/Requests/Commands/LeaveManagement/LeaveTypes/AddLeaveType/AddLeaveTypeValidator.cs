using FluentValidation;
using WolfDen.Domain.Entity;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveTypes.AddLeaveType
{
    public class AddLeaveTypeValidator : AbstractValidator<AddLeaveTypeCommand>
    {
        public AddLeaveTypeValidator()
        {
            RuleFor(x => x.TypeName).NotEmpty().WithMessage("Type Name Required");
        }

    }
}
