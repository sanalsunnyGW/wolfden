using MediatR;
using WolfDen.Application.Requests.DTOs.Attendence;

namespace WolfDen.Application.Requests.Queries.Attendence.DailyAttendanceReport
{
    public class DailyDetailsPdf : IRequest<DailyAttendanceDTO>
    {
        public int EmployeeId { get; set; }
        public DateOnly Date { get; set; }
    }
}
