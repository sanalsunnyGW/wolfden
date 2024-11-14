using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.Requests.Commands.Employees;

namespace WolfDen.Application.Validators
{
    public class EmployeeUpdateEmployeeValidator : AbstractValidator<EmployeeUpdateEmployee>
    {
        public EmployeeUpdateEmployeeValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name cannot be NULL");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email cannot be NULL").EmailAddress().WithMessage("A valid email address is required");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("PhoneNumber cannot be NULL");

        }
    }
}
