using MediatR;
using WolfDen.Application.DTOs;

namespace WolfDen.Application.Requests.Queries.Employees.GetEmployeeHierarchy
{
    public class GetEmployeeHierarchyQuery : IRequest<EmployeeHierarchyDto>
    {
        public int Id { get; set; }
    }
}
