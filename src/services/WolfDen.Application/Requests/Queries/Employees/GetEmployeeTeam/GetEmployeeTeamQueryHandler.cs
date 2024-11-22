using LanguageExt;
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
            var employee = await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (employee.IsActive == false)
            {
                result.Id = employee.Id;
                result.EmployeeCode = employee.EmployeeCode;
                result.FirstName = employee.FirstName;
                result.LastName = employee.LastName;
                result.Email = employee.Email;
                result.DateofBirth = employee.DateofBirth;
                result.DepartmentId = employee.DepartmentId;
                result.DepartmentName = employee.Department != null ? employee.Department.Name : null;
                result.DesignationId = employee.DesignationId;
                result.DesignationName = employee.Designation != null ? employee.Designation.Name : null;
                result.ManagerId = employee.ManagerId;
                result.ManagerName = employee.Manager != null
                   ? $"{employee.Manager.FirstName}{(string.IsNullOrWhiteSpace(employee.Manager.LastName) ? "" : " " + employee.Manager.LastName)}"
                   : null;
                result.PhoneNumber = employee.PhoneNumber;
                result.IsActive = employee.IsActive;
                result.Address = employee.Address;
                result.Country = employee.Country;
                result.State = employee.State;
                result.EmploymentType = employee.EmploymentType;
                result.Photo = employee.Photo;
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
                        DepartmentName = teamMate.Department != null ? teamMate.Department.Name : null,
                        DesignationId = teamMate.DesignationId,
                        DesignationName = teamMate.Designation != null ? teamMate.Designation.Name : null,
                        ManagerId = teamMate.ManagerId,
                        ManagerName = teamMate.Manager != null
                        ? $"{teamMate.Manager.FirstName}{(string.IsNullOrWhiteSpace(teamMate.Manager.LastName) ? "" : " " + teamMate.Manager.LastName)}" : null,
                        IsActive = teamMate.IsActive,
                        Address = teamMate.Address,
                        Country = teamMate.Country,
                        State = teamMate.State,
                        EmploymentType = teamMate.EmploymentType,
                        Photo = teamMate.Photo,

                    };
                    teamList.Add(employeeDto);
                }
                return teamList;
            }
            if (request.GetFullHierarchy)
            {
                result.Id = employee.Id;
                result.EmployeeCode = employee.EmployeeCode;
                result.FirstName = employee.FirstName;
                result.LastName = employee.LastName;
                result.Email = employee.Email;
                result.DateofBirth = employee.DateofBirth;
                result.DepartmentId = employee.DepartmentId;
                result.DepartmentName = employee.Department != null ? employee.Department.Name : null;
                result.DesignationId = employee.DesignationId;
                result.DesignationName = employee.Designation != null ? employee.Designation.Name : null;
                result.ManagerId = employee.ManagerId;
                result.ManagerName = employee.Manager != null
                ? $"{employee.Manager.FirstName}{(string.IsNullOrWhiteSpace(employee.Manager.LastName) ? "" : " " + employee.Manager.LastName)}" : null;
                result.PhoneNumber = employee.PhoneNumber;
                result.IsActive = employee.IsActive;
                result.Address = employee.Address;
                result.Country = employee.Country;
                result.State = employee.State;
                result.EmploymentType = employee.EmploymentType;
                result.Photo = employee.Photo;
                result.Subordinates = await service.GetSubordinates(employee.Id, cancellationToken);
                teamList.Add(result);
                return teamList;
            }
            var subordinates = _context.Employees.Where(x => x.ManagerId == employee.Id && x.IsActive == true);
            foreach (var subordinate in subordinates)
            {
                EmployeeHierarchyDto employeeDto = new()
                {
                    Id = subordinate.Id,
                    EmployeeCode = subordinate.EmployeeCode,
                    FirstName = subordinate.FirstName,
                    LastName = subordinate.LastName,
                    Email = subordinate.Email,
                    PhoneNumber = subordinate.PhoneNumber,
                    DateofBirth = subordinate.DateofBirth,
                    DepartmentId = subordinate.DepartmentId,
                    DepartmentName = subordinate.Department != null ? subordinate.Department.Name : null,
                    DesignationId = subordinate.DesignationId,
                    DesignationName = subordinate.Designation != null ? subordinate.Designation.Name : null,
                    ManagerId = subordinate.ManagerId,
                    ManagerName = subordinate.Manager != null
                   ? $"{subordinate.Manager.FirstName}{(string.IsNullOrWhiteSpace(subordinate.Manager.LastName) ? "" : " " + subordinate.Manager.LastName)}"
                   : null,
                    IsActive = subordinate.IsActive,
                    Address = subordinate.Address,
                    Country = subordinate.Country,
                    State = subordinate.State,
                    EmploymentType = subordinate.EmploymentType,
                    Photo = subordinate.Photo
                };
                teamList.Add(employeeDto);
            }
            return teamList;
        }
    }
}
