using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Employees.EmployeeUpdateEmployee
{
    public class EmployeeUpdateEmployeeCommandHandler(WolfDenContext context, EmployeeUpdateEmployeeValidator validator, UserManager<User> userManager) : IRequestHandler<EmployeeUpdateEmployeeCommand, bool>
    {
        private readonly WolfDenContext _context = context;
        private readonly EmployeeUpdateEmployeeValidator _validator = validator;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<bool> Handle(EmployeeUpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Validation failed: {errors}");
            }
            Employee employee = await _context.Employees.FindAsync(request.Id, cancellationToken);
            if (employee == null)
            {
                return false;
            }

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                User user = await _userManager.FindByNameAsync(employee.RFId);
                user.SetEmail(request.Email);
                var updateResult = await _userManager.UpdateAsync(user);
                employee.EmployeeUpdateEmployee(request.FirstName, request.LastName, request.DateofBirth, request.Email, request.PhoneNumber, request.Gender, request.Address, request.Country, request.State, request.Photo);
                _context.Employees.Update(employee);
                await transaction.CommitAsync(cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw new Exception("Transaction Failed ");
            }
        }
    }
}
