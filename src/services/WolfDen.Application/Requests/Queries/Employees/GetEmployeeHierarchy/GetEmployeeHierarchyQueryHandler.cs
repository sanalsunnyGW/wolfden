using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.Requests.Services;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Employees.GetEmployeeHierarchy
{
    public class GetEmployeeHierarchyQueryHandler(WolfDenContext context) : IRequestHandler<GetEmployeeHierarchyQuery, EmployeeHierarchyDto>
    {
        private readonly WolfDenContext _context = context;

        public async Task<EmployeeHierarchyDto> Handle(GetEmployeeHierarchyQuery request, CancellationToken cancellationToken)
        {

            var employee = await _context.Employees
            .Where(e => e.ManagerId == null && e.IsActive == true)
            .FirstOrDefaultAsync(e => _context.Employees.Any(sub => sub.ManagerId == e.Id), cancellationToken);
            if (employee == null)
            {
                throw new InvalidOperationException("No team head found.");
            }
            EmployeeHierarchyService service = new(_context);
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
                IsActive = employee.IsActive,
                Address = employee.Address,
                Country = employee.Country,
                State = employee.State,
                EmploymentType = employee.EmploymentType,
                Photo = employee.Photo,
                Subordinates = await service.GetSubordinates(employee.Id, cancellationToken)
            };
            return result;

        }

    }

}


