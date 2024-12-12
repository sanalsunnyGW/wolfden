using MediatR;
using WolfDen.Application.DTOs.Employees;

namespace WolfDen.Application.Requests.Queries.Employees.GetMyTeam
{
    public class GetMyTeamQuery : IRequest<List<EmployeeHierarchyDto>>
    {
        public int Id { get; set; }

    }
}
