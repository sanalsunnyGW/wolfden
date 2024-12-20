﻿using FluentValidation;
using WolfDen.Application.Requests.Commands.Employees.EmployeeUpdateEmployee;

namespace WolfDen.Application.Requests.Commands.Employees.ResetPassword
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator() {
            RuleFor(x => x.Id).NotEmpty().WithMessage("ID canont be empty");

            RuleFor(x => x.password).NotEmpty().WithMessage("Password canont be empty");

        }
    }
}
