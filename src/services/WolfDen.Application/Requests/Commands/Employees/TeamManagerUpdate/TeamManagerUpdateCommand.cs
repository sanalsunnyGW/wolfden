using MediatR;

namespace WolfDen.Application.Requests.Commands.Employees.TeamManagerUpdate
{
    public class TeamManagerUpdateCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int ManagerId { get; set; }
    }
}
