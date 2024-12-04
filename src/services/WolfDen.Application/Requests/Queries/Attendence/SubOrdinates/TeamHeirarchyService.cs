using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Services
{
    public class TeamHeirarchyService(WolfDenContext context)
    {
        private readonly WolfDenContext _context = context;
        public async Task<List<SubOrdinateDTO>> GetSubordinates(int managerId)
        {
            List<SubOrdinateDTO> result = new List<SubOrdinateDTO>();
            List<Employee> employees = await _context.Employees
                .Where(x => x.ManagerId == managerId && x.IsActive == true)
                .Include(x => x.Department)
                .Include(x => x.Designation)
                .ToListAsync();
            foreach (Employee employee in employees)
            {
                SubOrdinateDTO employeeDto = new SubOrdinateDTO();
                employeeDto.Id = employee.Id;
                employeeDto.EmployeeCode = employee.EmployeeCode;
                employeeDto.Name = employee.FirstName + " " + employee.LastName;
                employeeDto.Email = employee.Email;
                employeeDto.Photo = employee.Photo;
                employeeDto.Department = employee.Department?.Name;
                employeeDto.Designation = employee.Designation?.Name;
                employeeDto.Manager = employee.Manager?.FirstName + " " + employee.Manager?.LastName;
                employeeDto.SubOrdinates = await GetSubordinates(employee.Id);
                result.Add(employeeDto);
            }
            return result;
        }
    }
}