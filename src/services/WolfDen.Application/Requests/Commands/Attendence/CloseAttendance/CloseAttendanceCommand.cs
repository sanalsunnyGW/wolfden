using MediatR;

namespace WolfDen.Application.Requests.Commands.Attendence.CloseAttendance
{
    public class CloseAttendanceCommand : IRequest<int>
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsClosed { get; set; }
    }
}
