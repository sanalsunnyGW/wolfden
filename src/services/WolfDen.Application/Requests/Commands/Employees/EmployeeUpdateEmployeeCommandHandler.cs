using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.Validators;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Employees
{
    public class EmployeeUpdateEmployeeCommandHandler : IRequestHandler<EmployeeUpdateEmployee, bool>
    {
        private readonly WolfDenContext _context;
        private readonly EmployeeUpdateEmployeeValidator _validator;

        public EmployeeUpdateEmployeeCommandHandler(WolfDenContext context, EmployeeUpdateEmployeeValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<bool> Handle(EmployeeUpdateEmployee request, CancellationToken cancellationToken)
        {
            var result = _validator.Validate(request);
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
            employee.EmployeeUpdateEmployee(request.FirstName, request.LastName, request.DateofBirth, request.Email, request.PhoneNumber, request.Gender, request.JoiningDate);
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return true;




        }
    }
}
