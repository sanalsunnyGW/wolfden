using FluentValidation;

namespace WolfDen.Application.Requests.Commands.Employees.AddEmployee
{
    public class CreateEmployeeValidator : AbstractValidator<AddEmployeecommand>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name cannot be NULL");
            RuleFor(x => x.EmployeeCode).NotEmpty().WithMessage("Employee Code cannot be NULL");
            RuleFor(x => x.RFId).NotEmpty().WithMessage("RFId cannot be NULL");

        }
    }
}
