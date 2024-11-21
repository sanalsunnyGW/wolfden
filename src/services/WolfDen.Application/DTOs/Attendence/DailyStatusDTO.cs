using WolfDen.Domain.Enums;

namespace WolfDen.Application.DTOs.Attendence
{
    public class DailyStatusDTO
    {
        public DateOnly Date { get; set; }
        public AttendanceStatus AttendanceStatusId { get; set; }
    }
}
