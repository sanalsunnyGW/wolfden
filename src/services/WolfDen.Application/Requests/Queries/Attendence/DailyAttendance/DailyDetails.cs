using MediatR;
using WolfDen.Application.Requests.DTOs.Attendence;

namespace WolfDen.Application.Requests.Queries.Attendence.DailyStatus
{
    public class DailyDetails : IRequest<DailyAttendanceDTO>
    {
        public int EmployeeId { get; set; }
        public DateOnly Date { get; set; }
    }
}