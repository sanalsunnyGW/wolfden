using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.Requests.Commands.Employees;

namespace WolfDen.Application.Validators
{
    public class AdminUpdateEmployeeValidator : AbstractValidator<AdminUpdateEmployee>
    {
        public AdminUpdateEmployeeValidator()
        {
            RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("Department Id cannot be NULL");
            RuleFor(x => x.DesignationId).NotEmpty().WithMessage("Designation Id cannot be NULL");
            RuleFor(x => x.IsActive).NotEmpty().WithMessage("Employee Status should not be NULL");
        }

    }
}
