using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.Requests.Services;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Employees.GetEmployeeTeam
{
    public class GetEmployeeTeamQueryHandler(WolfDenContext context) : IRequestHandler<GetEmployeeTeamQuery, List<EmployeeHierarchyDto>>
    {
        private readonly WolfDenContext _context = context;
        public async Task<List<EmployeeHierarchyDto>> Handle(GetEmployeeTeamQuery request, CancellationToken cancellationToken)
        {
            EmployeeHierarchyService service = new(_context);
            List<EmployeeHierarchyDto> teamList = new();
            EmployeeHierarchyDto result = new();
            var employee = await _context.Employees.FindAsync(request.Id, cancellationToken);
            if (employee.IsActive == false)
            {
                result.Id = employee.Id;
                result.EmployeeCode = employee.EmployeeCode;
                result.FirstName = employee.FirstName;
                result.LastName = employee.LastName;
                result.Email = employee.Email;
                result.DateofBirth = employee.DateofBirth;
                result.DepartmentId = employee.DepartmentId;
                result.DesignationId = employee.DesignationId;
                result.ManagerId = employee.ManagerId;
                result.PhoneNumber = employee.PhoneNumber;
                result.IsActive = employee.IsActive;
                result.Subordinates = await service.GetSubordinates(employee.Id);
                teamList.Add(result);
                return teamList;
            }
            var myTeam = await _context.Employees.Where(x => x.ManagerId == request.Id && x.IsActive == true).ToListAsync();
            if (myTeam.Count == 0)
            {
                var teamMates = _context.Employees.Where(x => x.ManagerId == employee.ManagerId && x.IsActive == true);

                foreach (var teamMate in teamMates)
                {
                    EmployeeHierarchyDto employeeDto = new()
                    {
                        Id = teamMate.Id,
                        EmployeeCode = teamMate.EmployeeCode,
                        FirstName = teamMate.FirstName,
                        LastName = teamMate.LastName,
                        Email = teamMate.Email,
                        PhoneNumber = teamMate.PhoneNumber,
                        DateofBirth = teamMate.DateofBirth,
                        DepartmentId = teamMate.DepartmentId,
                        DesignationId = teamMate.DesignationId,
                        ManagerId = teamMate.ManagerId,
                        IsActive = teamMate.IsActive,

                    };
                    teamList.Add(employeeDto);
                }
                return teamList;
            }

            result.Id = employee.Id;
            result.EmployeeCode = employee.EmployeeCode;
            result.FirstName = employee.FirstName;
            result.LastName = employee.LastName;
            result.Email = employee.Email;
            result.DateofBirth = employee.DateofBirth;
            result.DepartmentId = employee.DepartmentId;
            result.DesignationId = employee.DesignationId;
            result.ManagerId = employee.ManagerId;
            result.PhoneNumber = employee.PhoneNumber;
            result.IsActive = employee.IsActive;
            result.Subordinates = await service.GetSubordinates(employee.Id);
            teamList.Add(result);
            return teamList;
        }


    }
}
