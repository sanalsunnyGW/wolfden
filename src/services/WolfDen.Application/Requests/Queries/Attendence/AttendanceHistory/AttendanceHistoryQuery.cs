using MediatR;
using WolfDen.Application.DTOs.Attendence;

namespace WolfDen.Application.Requests.Queries.Attendence.AttendanceHistory
{
    public class AttendanceHistoryQuery:IRequest<List<WeeklySummaryDTO>>
    {
        public int EmployeeId { get; set; }
        public int Year { get; set; }
        public int CurrentPage { get; set; }

    }
}
