using FluentValidation;

namespace WolfDen.Application.Requests.Queries.Employees.EmployeeLogin
{
    public class EmployeeLoginQueryValidator : AbstractValidator<EmployeeLoginQuery>
    {
        public EmployeeLoginQueryValidator()
        {
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
        }
    }
}
