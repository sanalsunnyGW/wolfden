using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.Requests.Commands.Employees;

namespace WolfDen.Application.Validators
{
    public class CreateEmployeeValidator:AbstractValidator<AddEmployee>
    {
        public CreateEmployeeValidator()
        {

            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name cannot be NULL");
            RuleFor(x => x.EmployeeCode).NotEmpty().WithMessage("Employee Code cannot be NULL");
            RuleFor(x => x.RFId).NotEmpty().WithMessage("RFId cannot be NULL");
        }
    }
}
