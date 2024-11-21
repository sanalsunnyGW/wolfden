using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfDen.Application.Requests.Queries.Employees.EmployeeLogin
{
    public class EmployeeLoginQueryValidator: AbstractValidator<EmployeeLoginQuery>
    {
        public EmployeeLoginQueryValidator()
        {
            RuleFor(x=>x.Password).NotEmpty();
            RuleFor(x=>x.Email).NotEmpty();
        }
    }
}
