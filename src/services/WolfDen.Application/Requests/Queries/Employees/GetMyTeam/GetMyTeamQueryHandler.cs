using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.Requests.Services;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Employees.GetMyTeam
{
    public class GetMyTeamQueryHandler(WolfDenContext context) : IRequestHandler<GetMyTeamQuery, List<EmployeeHierarchyDto>>
    {
        private readonly WolfDenContext _context = context;
        public async Task<List<EmployeeHierarchyDto>> Handle(GetMyTeamQuery request, CancellationToken cancellationToken)
        {

            GetEmployeeService service = new(_context);
            List<EmployeeHierarchyDto> teamList = new();
            Employee employee = await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (employee is null)
            {
                return teamList;
            }
            if (employee.IsActive == false)
            {
                teamList.Add(await service.GetEmployee(employee, false, cancellationToken));
                return teamList;
            }

            List<Employee> myTeam = await _context.Employees.Where(x => x.ManagerId == request.Id && x.IsActive == true).Include(e => e.Designation).Include(e => e.Department).ToListAsync();
            foreach (Employee subordinate in myTeam)
            {
                teamList.Add(await service.GetEmployee(subordinate, false, cancellationToken));
            }
            return teamList;
        }
    }
}
