using MediatR;
using WolfDen.Application.DTOs.Employees;

namespace WolfDen.Application.Requests.Queries.Employees.GetEmployeeTeam
{
    public class GetEmployeeTeamQuery : IRequest<List<EmployeeHierarchyDto>>
    {
        public int Id { get; set; }
    }
}
