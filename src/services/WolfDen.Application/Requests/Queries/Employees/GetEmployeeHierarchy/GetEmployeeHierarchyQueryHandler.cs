using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Employees.GetEmployeeHierarchy
{
    public class GetEmployeeHierarchyQueryHandler(WolfDenContext context) : IRequestHandler<GetEmployeeHierarchyQuery, EmployeeHierarchyDto>
    {
        private readonly WolfDenContext _context=context;

        public async Task<EmployeeHierarchyDto> Handle(GetEmployeeHierarchyQuery request, CancellationToken cancellationToken)
        {

            var employee = await _context.Employees.FindAsync(request.Id, cancellationToken);
            EmployeeHierarchyDto result = new()
            {

                Id = employee.Id,
                EmployeeCode = employee.EmployeeCode,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                DateofBirth = employee.DateofBirth,
                DepartmentId = employee.DepartmentId,
                DesignationId = employee.DesignationId,
                ManagerId = employee.ManagerId,
                PhoneNumber = employee.PhoneNumber,
                Subordinates = await GetSubordinates(employee.Id)
            };


            return result;

        }
        private async Task<List<EmployeeHierarchyDto>> GetSubordinates(int managerId)
        {
            List<EmployeeHierarchyDto> result = new();
            var employees = await _context.Employees.Where(x => x.ManagerId == managerId).ToListAsync();
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
                    DesignationId = employee.DesignationId,
                    ManagerId = employee.ManagerId,
                    Subordinates = await GetSubordinates(employee.Id)
                };
                result.Add(employeeDto);

            }
            return result;


        }
    }

}


