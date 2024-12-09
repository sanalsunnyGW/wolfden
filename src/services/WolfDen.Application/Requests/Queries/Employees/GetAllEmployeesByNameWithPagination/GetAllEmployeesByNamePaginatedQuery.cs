using MediatR;
using WolfDen.Application.DTOs.Employees;

namespace WolfDen.Application.Requests.Queries.Employees.GetAllEmployeesByNameWithPagination
{
    public class GetAllEmployeesByNamePaginatedQuery : IRequest<GetAllEmployeePaginationResponseDTO>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public GetAllEmployeesByNamePaginatedQuery()
        {
            
        }
    }
}
