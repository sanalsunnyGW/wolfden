namespace WolfDen.Application.DTOs.Attendence
{
    public class RangeDTO
    {
        public List<DateOnly> PreviousAttendanceClosedDate { get; set; }
        public List<DateOnly> AttendanceClosedDate { get; set; }

    }
}
