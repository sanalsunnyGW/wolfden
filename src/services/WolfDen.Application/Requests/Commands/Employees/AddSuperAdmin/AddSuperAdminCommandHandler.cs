using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Employees.AddSuperAdmin
{
    public class AddSuperAdminCommandHandler(WolfDenContext context, CreateSuperAdminValidator validator, UserManager<Domain.Entity.User> userManager, IPasswordHasher<User> passwordHasher) : IRequestHandler<AddSuperAdminCommand, int>
    {
        private readonly WolfDenContext _context = context;
        private readonly CreateSuperAdminValidator _validator = validator;
        private readonly UserManager<Domain.Entity.User> _userManager = userManager;
        private readonly IPasswordHasher<User> _passwordHasher=passwordHasher;

        public async Task<int> Handle(AddSuperAdminCommand request, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Validation failed: {errors}");
            }
            Domain.Entity.User user = new Domain.Entity.User(request.RFId);
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
           
                var creationResult = await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, "SuperAdmin");
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
            
        }
    }

