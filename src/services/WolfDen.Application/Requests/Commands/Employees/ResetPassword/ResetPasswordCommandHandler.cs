using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Employees.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly WolfDenContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public ResetPasswordCommandHandler(WolfDenContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0 || string.IsNullOrWhiteSpace(request.password))
            {
                throw new ValidationException("Invalid employee ID or password.");
            }

            var employee = await _context.Employees.FindAsync(new object[] { request.Id }, cancellationToken);
            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == employee.Id, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found for the associated employee.");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, request.password);

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return true;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw new Exception("An error occurred while resetting the password.");
            }
        }
    }
}
