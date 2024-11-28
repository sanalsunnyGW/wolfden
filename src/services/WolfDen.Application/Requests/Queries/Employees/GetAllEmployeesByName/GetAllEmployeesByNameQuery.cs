using MediatR;
using WolfDen.Application.DTOs.Employees;

namespace WolfDen.Application.Requests.Queries.Employees.GetAllEmployeesName
{
    public class GetAllEmployeesByNameQuery: IRequest<List<EmployeeNameDTO>>
    {
        public string? FirstName { get;  set; }
        public string? LastName { get;  set; }
        public GetAllEmployeesByNameQuery()
        {
            
        }
    }
}
