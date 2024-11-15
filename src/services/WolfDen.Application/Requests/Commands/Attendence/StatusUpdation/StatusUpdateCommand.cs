using MediatR;

namespace WolfDen.Application.Requests.Commands.Attendence.StatusUpdation
{
    public class StatusUpdateCommand : IRequest<int>
    {
        public int EmployeeId { get; set; }
        public DateOnly Date { get; set; }
    }
}
