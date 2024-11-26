using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Employees.EmployeeDirectory
{
    public class GetAllEmployeeQueryHandler : IRequestHandler<GetAllEmployeeQuery, PaginationResponse>
    {
        private readonly WolfDenContext _context;

        public GetAllEmployeeQueryHandler(WolfDenContext context)
        {  
            _context = context;
        }
        public async Task<PaginationResponse> Handle(GetAllEmployeeQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber > 0 ? request.PageNumber : 0;
            var baseQuery = _context.Employees
               .Include(e => e.Designation)
               .Include(e => e.Department)
               .AsQueryable();
            if (request.DepartmentID.HasValue)
            {
                baseQuery = baseQuery.Where(e => e.DepartmentId == request.DepartmentID);
            }
            if (!string.IsNullOrWhiteSpace(request.EmployeeName))
            {
                baseQuery = baseQuery.Where(e =>(e.FirstName + " " + e.LastName).Contains(request.EmployeeName));
            }

            int totalCount = await baseQuery.CountAsync(cancellationToken);

            var employees = await baseQuery
               .Skip(request.PageSize * pageNumber)
               .Take(request.PageSize)
               .Select(e => new EmployeeDirectoryDTO
               {
                   EmployeeCode = e.EmployeeCode,
                   FirstName = e.FirstName,
                   LastName = e.LastName,
                   Email = e.Email,
                   PhoneNumber = e.PhoneNumber,
                   DateofBirth = e.DateofBirth,
                   JoiningDate = e.JoiningDate,
                   Gender = e.Gender,
                   DesignationId = e.DesignationId,
                   DesignationName = e.Designation != null ? e.Designation.Name : null,
                   DepartmentId = e.DepartmentId,
                   DepartmentName = e.Department != null ? e.Department.Name : null,
                   ManagerId = e.ManagerId,
                   ManagerName = e.Manager != null
                   ? $"{e.Manager.FirstName}{(string.IsNullOrWhiteSpace(e.Manager.LastName) ? "" : " " + e.Manager.LastName)}"
                   : null,
                   IsActive = e.IsActive
               })
            .ToListAsync(cancellationToken);
            /* int totalpage = employees.Count;*/
            int pageCount = (int)(Math.Ceiling((decimal)totalCount / request.PageSize));
            var emp = employees.ToList();
            var empDirectory = new PaginationResponse
            {
                EmployeeDirectoryDTOs = emp,
                TotalRecords=totalCount,
                TotalPages = pageCount,
            };

            return empDirectory;
        }
    }
}
