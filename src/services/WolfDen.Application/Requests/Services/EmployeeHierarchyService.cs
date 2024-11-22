using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Services
{
    public class EmployeeHierarchyService(WolfDenContext context)
    {
        private readonly WolfDenContext _context = context;
        public async Task<List<EmployeeHierarchyDto>> GetSubordinates(int managerId, CancellationToken cancellationToken)
        {
            List<EmployeeHierarchyDto> result = new();
            var employees = await _context.Employees.Where(x => x.ManagerId == managerId && x.IsActive == true).ToListAsync();
            foreach (var employee in employees)
            {
                EmployeeHierarchyDto employeeDto = new()
                {
                    Id = employee.Id,
                    EmployeeCode = employee.EmployeeCode,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    DateofBirth = employee.DateofBirth,
                    DepartmentId = employee.DepartmentId,
                    DepartmentName = employee.Department != null ? employee.Department.Name : null,
                    DesignationId = employee.DesignationId,
                    DesignationName = employee.Designation != null ? employee.Designation.Name : null,
                    ManagerId = employee.ManagerId,
                    ManagerName = employee.Manager != null
                       ? $"{employee.Manager.FirstName}{(string.IsNullOrWhiteSpace(employee.Manager.LastName) ? "" : " " + employee.Manager.LastName)}"
                       : null,
                    IsActive = employee.IsActive,
                    Address = employee.Address,
                    Country = employee.Country,
                    State = employee.State,
                    EmploymentType = employee.EmploymentType,
                    Photo = employee.Photo,
                    Subordinates = await GetSubordinates(employee.Id, cancellationToken)
                };
                result.Add(employeeDto);

            }
            return result;


        }
    }
}
