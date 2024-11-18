using FluentValidation;

namespace WolfDen.Application.Requests.Commands.Employees.AdminUpdateEmployee
{
    public class AdminUpdateEmployeeValidator : AbstractValidator<AdminUpdateEmployeeCommand>
    {
        public AdminUpdateEmployeeValidator()
        {
            RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("Department Id cannot be NULL");
            RuleFor(x => x.DesignationId).NotEmpty().WithMessage("Designation Id cannot be NULL");
            RuleFor(x => x.IsActive).NotEmpty().WithMessage("Employee Status should not be NULL");
        }

    }
}
