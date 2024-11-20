using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.DTOs;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Employees.ViewEmployee
{
    public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, EmployeeDTO>
    {
        private readonly WolfDenContext _context;

        public GetEmployeeQueryHandler(WolfDenContext context)
        {
            _context=context;
        }
        public async Task<EmployeeDTO> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees
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
                DesignationName= employee.Designation != null ? employee.Designation.Name : null,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department != null ? employee.Department.Name : null,
                ManagerId = employee.ManagerId,
                ManagerName = employee.Manager != null
                              ? $"{employee.Manager.FirstName}{(string.IsNullOrWhiteSpace(employee.Manager.LastName) ? "" : " " + employee.Manager.LastName)}"
                              : null,
                IsActive = employee.IsActive
            };

            return employeeDTO;
        }
    }
}
