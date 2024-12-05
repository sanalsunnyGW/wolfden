using FluentValidation;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.AddLeaveRequestForEmployeeByAdmin
{
    public class AddLeaveRequestForEmployeeByAdminValidator : AbstractValidator<AddLeaveRequestForEmployeeByAdmin>
    {
        public AddLeaveRequestForEmployeeByAdminValidator()
        {
            RuleFor(x => x.AdminId).NotEmpty().WithMessage("Admin Id Required");
            RuleFor(x => x.EmployeeCode).NotEmpty().WithMessage("Employee Code required");
            RuleFor(x => x.TypeId).NotEmpty().WithMessage("Type Id required");
            RuleFor(x => x.FromDate).NotEmpty().WithMessage("From Date required");
            RuleFor(x => x.ToDate).NotEmpty().WithMessage("To Date required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Leave Description required");
        }
    }
}
