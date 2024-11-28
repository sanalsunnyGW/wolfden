using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Employees.GetAllEmployeesName
{
    public class GetAllEmployeesNameQueryHandler : IRequestHandler<GetAllEmployeesNameQuery, List<EmployeeNameDTO>>
    {
        private readonly WolfDenContext _context;

        public GetAllEmployeesNameQueryHandler(WolfDenContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeNameDTO>> Handle(GetAllEmployeesNameQuery query, CancellationToken cancellationToken)
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

            var employees = await employeesQuery
                .Select(e => new EmployeeNameDTO
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName
                })
                .ToListAsync(cancellationToken);

            return employees;
        }
    }
}
