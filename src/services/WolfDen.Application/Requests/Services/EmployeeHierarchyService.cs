using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Services
{
    public class EmployeeHierarchyService(WolfDenContext context)
    {
        private readonly WolfDenContext _context=context;
        public async Task<List<EmployeeHierarchyDto>> GetSubordinates(int managerId)
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
