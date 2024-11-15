using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.Requests.Services;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Employees.GetEmployeeHierarchy
{
    public class GetEmployeeHierarchyQueryHandler(WolfDenContext context) : IRequestHandler<GetEmployeeHierarchyQuery, EmployeeHierarchyDto>
    {
        private readonly WolfDenContext _context=context;

        public async Task<EmployeeHierarchyDto> Handle(GetEmployeeHierarchyQuery request, CancellationToken cancellationToken)
        {

            var employee = await _context.Employees.FindAsync(request.Id, cancellationToken);
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
                Subordinates = await service.GetSubordinates(employee.Id)
            }; 


            return result;

        }
       
    }

}


