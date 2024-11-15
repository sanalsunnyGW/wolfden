using MediatR;
using WolfDen.Application.DTOs.Employees;

namespace WolfDen.Application.Requests.Queries.Employees.GetEmployeeHierarchy
{
    public class GetEmployeeHierarchyQuery : IRequest<EmployeeHierarchyDto>
    {
        public int Id { get; set; }
    }
}
