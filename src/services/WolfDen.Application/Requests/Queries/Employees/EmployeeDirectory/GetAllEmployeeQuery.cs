using MediatR;
using WolfDen.Application.DTOs.Employees;

namespace WolfDen.Application.Requests.Queries.Employees.EmployeeDirectory
{
    public class GetAllEmployeeQuery : IRequest<PaginationResponse>
    {
        public int? DepartmentID { get; set; }
        public string? EmployeeName {  get; set; }
        public int PageNumber {  get; set; }
        public int PageSize { get; set; }

        public GetAllEmployeeQuery()
        {
            
        }

    }
}
