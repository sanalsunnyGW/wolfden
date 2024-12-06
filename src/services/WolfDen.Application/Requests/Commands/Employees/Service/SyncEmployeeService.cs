using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Employees.Service
{
    public class SyncEmployeeService
    {
        private readonly WolfDenContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMediator _mediator;


        public SyncEmployeeService(WolfDenContext context, UserManager<User> userManager, IServiceScopeFactory serviceScopeFactory, IMediator mediator)
        {
            _context = context;

            _userManager = userManager;

        }
        public async Task SyncEmployeeUser()
        {

            List<Employee> employees = _context.Employees.Where(x => x.UserId == null).ToList();
            if (employees.Any())
            {
                foreach (Employee employee in employees)
                {
                    User user = new User(employee.RFId);
                    user.SetEmail(employee.Email);
                    await _userManager.CreateAsync(user);
                    employee.UpdateUserId(user.Id);
                    await _userManager.AddToRoleAsync(user, "Employee");
                    _context.Employees.Update(employee);
                }
                await _context.SaveChangesAsync();
            }


        }

    }

}