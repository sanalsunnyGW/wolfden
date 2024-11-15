using MediatR;
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

            var employee = await _context.Employees.FindAsync(request.Id, cancellationToken);
            EmployeeHierarchyDto result = new();
            if (employee.IsActive == false)
            {
                result.IsActive = false;
                return result;
            }
            EmployeeHierarchyService service = new(_context);
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
            return result;

        }

    }

}


