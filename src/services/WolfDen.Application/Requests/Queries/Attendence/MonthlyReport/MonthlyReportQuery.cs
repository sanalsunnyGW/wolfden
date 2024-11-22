using MediatR;
using WolfDen.Application.DTOs.Attendence;

namespace WolfDen.Application.Requests.Queries.Attendence.MonthlyAttendanceReport
{
    public class MonthlyReportQuery : IRequest<MonthlyReportDTO>
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int EmployeeId { get; set; }
    }
}
