using MediatR;
using WolfDen.Application.DTOs.Attendence;

namespace WolfDen.Application.Requests.Queries.Attendence.CheckAttendanceClose
{
    public class CheckAttendanceClosedQuery:IRequest<CheckAttendanceClosedDTO>
    {
        public string AttendanceClose { get; set; }
        
    }
}
