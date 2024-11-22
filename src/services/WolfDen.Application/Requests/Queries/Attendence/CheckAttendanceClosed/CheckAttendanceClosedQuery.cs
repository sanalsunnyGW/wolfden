using MediatR;

namespace WolfDen.Application.Requests.Queries.Attendence.CheckAttendanceClose
{
    public class CheckAttendanceClosedQuery:IRequest<bool>
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
