using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.Validators;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Employees
{
    public class AdminUpdateEmployeeCommandHandler : IRequestHandler<AdminUpdateEmployee, bool>
    {
        private readonly WolfDenContext _context;
        private readonly AdminUpdateEmployeeValidator _validator;

      

        public AdminUpdateEmployeeCommandHandler(WolfDenContext context, AdminUpdateEmployeeValidator validator)
        {
            _context = context;
            _validator = validator;

        }

        public async Task<bool> Handle(AdminUpdateEmployee request, CancellationToken cancellationToken)
        {
            var result=_validator.Validate(request);

            if (!result.IsValid)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Validation failed: {errors}");
            }

            var employee = await _context.Employees.FindAsync(request.Id);
            if (employee == null)
            {
                return false;
            }
            employee.AdminUpdateEmployee(request.DesignationId, request.DepartmentId, request.ManagerId, request.IsActive);
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
