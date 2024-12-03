using FluentValidation;

namespace WolfDen.Application.Requests.Commands.Employees.AddSuperAdmin
{
    public class CreateSuperAdminValidator : AbstractValidator<AddSuperAdminCommand>
    {
        public CreateSuperAdminValidator()
        {
            RuleFor(x => x.EmployeeCode).NotEmpty().WithMessage("Employee Code cannot be NULL");
            RuleFor(x => x.RFId).NotEmpty().WithMessage("RFId cannot be NULL");
        }
    }
}
