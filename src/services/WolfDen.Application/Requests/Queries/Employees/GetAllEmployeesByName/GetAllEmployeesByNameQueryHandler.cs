using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Employees.GetAllEmployeesName
{
    public class GetAllEmployeesByNameQueryHandler(WolfDenContext context, UserManager<Domain.Entity.User> usermanager) : IRequestHandler<GetAllEmployeesByNameQuery, List<EmployeeNameDTO>>
    {
        private readonly WolfDenContext _context = context;
        private readonly UserManager<Domain.Entity.User> _userManager = usermanager;

        public async Task<List<EmployeeNameDTO>> Handle(GetAllEmployeesByNameQuery query, CancellationToken cancellationToken)
        {
            var employeesQuery = _context.Employees.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.FirstName))
            {
                employeesQuery = employeesQuery.Where(e => e.FirstName.Contains(query.FirstName));
            }

            if (!string.IsNullOrWhiteSpace(query.LastName))
            {
                employeesQuery = employeesQuery.Where(e => e.LastName.Contains(query.LastName));
            }
            List<Employee> employees = await employeesQuery.ToListAsync(cancellationToken);
            List<EmployeeNameDTO> employeeNameDTOs = new List<EmployeeNameDTO>();

            foreach (var employee in employees)
            {
                var user = await _userManager.FindByIdAsync(employee.UserId);
                var userRole = await _userManager.GetRolesAsync(user);
                EmployeeNameDTO employeeNameDTO = new EmployeeNameDTO()
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Role = string.Join(", ", userRole)
                };
                employeeNameDTOs.Add(employeeNameDTO);

            }
            return employeeNameDTOs;
        }
    }
}
