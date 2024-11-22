using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.Requests.Services;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Employees.GetEmployeeHierarchy
{
    public class GetEmployeeHierarchyQueryHandler(WolfDenContext context) : IRequestHandler<GetEmployeeHierarchyQuery, EmployeeHierarchyDto>
    {
        private readonly WolfDenContext _context = context;

        public async Task<EmployeeHierarchyDto> Handle(GetEmployeeHierarchyQuery request, CancellationToken cancellationToken)
        {

            Employee employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.ManagerId == null && e.IsActive == true && _context.Employees.Any(sub => sub.ManagerId == e.Id), cancellationToken);
            if (employee is null)
            {
                throw new InvalidOperationException("No team head found.");
            }
            GetEmployeeService service = new(_context);
            return await service.GetEmployee(employee, cancellationToken);

        }

    }

}


