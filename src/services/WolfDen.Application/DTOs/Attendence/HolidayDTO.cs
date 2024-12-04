using WolfDen.Domain.Enums;

namespace WolfDen.Application.DTOs.Attendence
{
    public class HolidayDTO
    {
        public DateOnly Date { get; set; }
        public AttendanceStatus Type { get; set; }

        public string Description {  get; set; }
    }
}
