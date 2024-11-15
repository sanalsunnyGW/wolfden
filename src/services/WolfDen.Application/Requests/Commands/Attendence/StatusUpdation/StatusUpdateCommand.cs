using MediatR;
using WolfDen.Application.Requests.DTOs.Attendence;

namespace WolfDen.Application.Requests.Commands.Attendence.StatusUpdation
{
    public class StatusUpdateCommand : IRequest<List<AttendenceLogDTO>>
    {
        public int EmployeeId { get; set; }
        public DateOnly Date { get; set; }

    }
}
