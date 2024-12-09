using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.Requests.Queries.Employees.GetAllEmployeesName;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Employees.GetAllEmployeesByNameWithPagination
{
    public class GetAllEmployeesByNamePaginatedQueryHandler : IRequestHandler<GetAllEmployeesByNamePaginatedQuery, GetAllEmployeePaginationResponseDTO>
    {
        private readonly WolfDenContext _context;
        private readonly UserManager<Domain.Entity.User> _userManager;

        public GetAllEmployeesByNamePaginatedQueryHandler(WolfDenContext context, UserManager<Domain.Entity.User> usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }

        public async Task<GetAllEmployeePaginationResponseDTO> Handle(GetAllEmployeesByNamePaginatedQuery query, CancellationToken cancellationToken)
        {
            int pageNumber = query.PageNumber > 0 ? query.PageNumber : 0;
            var baseQuery = _context.Employees.AsNoTracking().AsQueryable(); ;

            if (!string.IsNullOrWhiteSpace(query.FirstName))
            {
                baseQuery = baseQuery.Where(e => e.FirstName.Contains(query.FirstName));
            }

            if (!string.IsNullOrWhiteSpace(query.LastName))
            {
                baseQuery = baseQuery.Where(e => e.LastName.Contains(query.LastName));
            }
            int totalCount = await baseQuery.CountAsync(cancellationToken);
            List<Employee> employees = await baseQuery
                          .Skip(query.PageSize * pageNumber)
                          .Take(query.PageSize)
                          .ToListAsync(cancellationToken);
            List<EmployeeNameDTO> employeeNameDTOs = new();

            foreach (var employee in employees)
            {
                User user = await _userManager.FindByIdAsync(employee.UserId);
                var userRole = await _userManager.GetRolesAsync(user);
                EmployeeNameDTO employeeNameDTO = new EmployeeNameDTO()
                {
                    Id = employee.Id,
                    EmployeeCode = employee.EmployeeCode,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Role = string.Join(", ", userRole)
                };
                employeeNameDTOs.Add(employeeNameDTO);

            }
            var empList = employeeNameDTOs.ToList();
            var emp = new GetAllEmployeePaginationResponseDTO
            {
                EmployeeNames = empList,
                TotalRecords = totalCount,
            };
            return emp;
        }
    }
}
