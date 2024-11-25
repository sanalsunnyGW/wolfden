using MediatR;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Domain.Enums;

namespace WolfDen.Application.Requests.Queries.Attendence.AttendanceHistory
{
    public class AttendanceHistoryQuery:IRequest<AttendanceHistoryDTO>
    {
        public int EmployeeId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public AttendanceStatus? AttendanceStatusId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
}
