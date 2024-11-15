using MediatR;
using System.ComponentModel.DataAnnotations;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Employees.EmployeeUpdateEmployee
{
    public class EmployeeUpdateEmployeeCommandHandler : IRequestHandler<EmployeeUpdateEmployeeCommand, bool>
    {
        private readonly WolfDenContext _context;
        private readonly EmployeeUpdateEmployeeValidator _validator;

        public EmployeeUpdateEmployeeCommandHandler(WolfDenContext context, EmployeeUpdateEmployeeValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<bool> Handle(EmployeeUpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Validation failed: {errors}");
            }
            var employee = await _context.Employees.FindAsync(request.Id, cancellationToken);
            if (employee == null)
            {
                return false;
            }
            employee.EmployeeUpdateEmployee(request.FirstName, request.LastName, request.DateofBirth, request.Email, request.PhoneNumber, request.Gender, request.JoiningDate);
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync(cancellationToken);
            return true;

        }
    }
}
