using FluentValidation;

namespace WolfDen.Application.Requests.Commands.Employees.EmployeeUpdateEmployee
{
    public class EmployeeUpdateEmployeeValidator : AbstractValidator<EmployeeUpdateEmployeeCommand>
    {
        public EmployeeUpdateEmployeeValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name cannot be NULL");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email cannot be NULL").EmailAddress().WithMessage("A valid email address is required");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("PhoneNumber cannot be NULL");

        }
    }
}
