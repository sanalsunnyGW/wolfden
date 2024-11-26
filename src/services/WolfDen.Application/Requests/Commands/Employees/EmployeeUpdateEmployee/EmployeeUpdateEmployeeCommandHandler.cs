using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Employees.EmployeeUpdateEmployee
{
    public class EmployeeUpdateEmployeeCommandHandler : IRequestHandler<EmployeeUpdateEmployeeCommand, bool>
    {
        private readonly WolfDenContext _context;
        private readonly EmployeeUpdateEmployeeValidator _validator;
        private readonly UserManager<User> _userManager;

        public EmployeeUpdateEmployeeCommandHandler(WolfDenContext context, EmployeeUpdateEmployeeValidator validator, UserManager<User> userManager)
        {
            _context = context;
            _validator = validator;
            _userManager = userManager;
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

            User user = new User(request.Email, request.FirstName);
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var creationResult = await _userManager.CreateAsync(user, request.Password);
                if (!creationResult.Succeeded)
                {
                    throw new Exception("User creation failed: " + string.Join(", ", creationResult.Errors.Select(e => e.Description)));
                }

                employee.EmployeeUpdateEmployee(request.FirstName, request.LastName, request.DateofBirth, request.Email, request.PhoneNumber, request.Gender, request.Address, request.Country, request.State, request.Photo, user.Id);
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return true;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
