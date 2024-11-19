using WolfDen.Domain.Enums;

namespace WolfDen.Application.Requests.DTOs.Attendence
{
    public class DailyAttendanceDTO
    {
        public DateTimeOffset ArrivalTime { get; set; }
        public DateTimeOffset DepartureTime { get; set; }
        public int InsideHours { get; set; }
        public int OutsideHours { get;set; }
        public string MissedPunch { get; set; }
        public AttendanceStatus AttendanceStatusId { get; set; }
        public List<AttendenceLogDTO> DailyLog { get; set; } = [];
    }
}
