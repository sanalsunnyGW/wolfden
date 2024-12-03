using MediatR;
using Microsoft.AspNetCore.Identity;
using WolfDen.Application.Requests.Queries.Employees.EmployeePasswordCheck;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Employees.CheckEmployeePassword
{
    public class CheckEmployeePasswordQueryHandler(UserManager<Domain.Entity.User> userManager, WolfDenContext context) : IRequestHandler<CheckEmployeePasswordQuery, bool>
    {
        private readonly UserManager<Domain.Entity.User> _userManager = userManager;
        private readonly WolfDenContext _context = context;

        public async Task<bool> Handle(CheckEmployeePasswordQuery request, CancellationToken cancellationToken)
        {
            Employee employee = await _context.Employees.FindAsync(request.Id);
            User user = await _userManager.FindByIdAsync(employee.UserId);
            bool checkPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (checkPassword)
            {
                return true;
            }
            return false;
        }
    }
}
