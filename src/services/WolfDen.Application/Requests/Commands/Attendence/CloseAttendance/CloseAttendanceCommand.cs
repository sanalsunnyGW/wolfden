using MediatR;

namespace WolfDen.Application.Requests.Commands.Attendence.CloseAttendance
{
    public class CloseAttendanceCommand : IRequest<int>
    {
        public bool IsClosed { get; set; }
    }
}
