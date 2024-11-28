using LanguageExt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.Requests.Services;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Employees.GetEmployeeTeam
{
    public class GetEmployeeTeamQueryHandler(WolfDenContext context) : IRequestHandler<GetEmployeeTeamQuery, List<EmployeeHierarchyDto>>
    {
        private readonly WolfDenContext _context = context;
        public async Task<List<EmployeeHierarchyDto>> Handle(GetEmployeeTeamQuery request, CancellationToken cancellationToken)
        {
            GetEmployeeService service = new(_context);
            List<EmployeeHierarchyDto> teamList = new();
            Employee employee = await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (employee == null)
            {
                return teamList;
            }
            if (employee.IsActive == false)
            {
                teamList.Add(await service.GetEmployee(employee, false, cancellationToken));
                return teamList;
            }
            if (employee.ManagerId == null)
            {
                teamList.Add(await service.GetEmployee(employee, false, cancellationToken));
                return teamList;
            }
            List<Employee> myTeam = await _context.Employees.Where(x => x.ManagerId == request.Id && x.IsActive == true).ToListAsync();
            if (myTeam.Count == 0)
            {
                List<Employee> teamMates = await _context.Employees.Where(x => x.ManagerId == employee.ManagerId && x.IsActive == true).ToListAsync();

                foreach (Employee teamMate in teamMates)
                {
                    teamList.Add(await service.GetEmployee(teamMate, false, cancellationToken));

                }
                return teamList;
            }
            if (request.Hierarchy)
            {
                teamList.Add(await service.GetEmployee(employee, true, cancellationToken));
                return teamList;
            }
            foreach (Employee subordinate in myTeam)
            {
                teamList.Add(await service.GetEmployee(subordinate, false, cancellationToken));
            }
            return teamList;
        }
    }
}
