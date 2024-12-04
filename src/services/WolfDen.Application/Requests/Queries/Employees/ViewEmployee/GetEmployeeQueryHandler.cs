using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Employees.ViewEmployee
{
    public class GetEmployeeQueryHandler(WolfDenContext context) : IRequestHandler<GetEmployeeQuery, EmployeeDTO>
    {
        private readonly WolfDenContext _context;

        public async Task<EmployeeDTO> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        {
            Employee employee = await _context.Employees
                .Include(e => e.Designation)
                .Include(e => e.Department)
                .Include(e => e.Manager)
                .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);

            if (employee == null)
            {
                throw new KeyNotFoundException();
            }
            EmployeeDTO employeeDTO = new()
            {
                Id = employee.Id,
                EmployeeCode = employee.EmployeeCode,
                RFId = employee.RFId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                DateofBirth = employee.DateofBirth,
                JoiningDate = employee.JoiningDate,
                Gender = employee.Gender,
                DesignationId = employee.DesignationId,
                DesignationName = employee.Designation != null ? employee.Designation.Name : null,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department != null ? employee.Department.Name : null,
                ManagerId = employee.ManagerId,
                ManagerName = employee.Manager != null
                              ? $"{employee.Manager.FirstName}{(string.IsNullOrWhiteSpace(employee.Manager.LastName) ? "" : " " + employee.Manager.LastName)}"
                              : null,
                IsActive = employee.IsActive,
                Address = employee.Address,
                State = employee.State,
                Country = employee.Country,
                EmploymentType = employee.EmploymentType,
                Photo = employee.Photo
            };


            return employeeDTO;
        }
    }
}
