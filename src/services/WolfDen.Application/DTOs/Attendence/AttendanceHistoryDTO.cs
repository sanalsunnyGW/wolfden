namespace WolfDen.Application.DTOs.Attendence
{
    public class AttendanceHistoryDTO
    {
        public List<WeeklySummaryDTO>? AttendanceHistory { get; set; }
        public int TotalPages { get; set; }
    }
}
