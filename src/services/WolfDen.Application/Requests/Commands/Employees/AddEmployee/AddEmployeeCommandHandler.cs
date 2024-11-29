using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Employees.AddEmployee
{
    public class AddEmployeeCommandHandler(WolfDenContext context, CreateEmployeeValidator validator, UserManager<User> userManager) : IRequestHandler<AddEmployeecommand, int>
    {
        private readonly WolfDenContext _context = context;
        private readonly CreateEmployeeValidator _validator = validator;
        private readonly UserManager<User> _userManager = userManager;


        public async Task<int> Handle(AddEmployeecommand request, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Validation failed: {errors}");
            }
            User user = new User(request.RFId);
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var creationResult = await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, "Employee");
                if (!creationResult.Succeeded)
                {
                    throw new Exception("User creation failed: " + string.Join(", ", creationResult.Errors.Select(e => e.Description)));
                }

                Employee employee = new Employee(request.EmployeeCode, request.RFId, user.Id);
                _context.Employees.Add(employee);
                await transaction.CommitAsync(cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return employee.Id;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw new Exception("Transaction Failed ");
            }
        }
    }
}
