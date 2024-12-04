namespace WolfDen.Application.DTOs.Attendence
{
    public class ManagerWeeklyAttendanceDTO
    {
        public string EmployeeName { get; set; }
        public List<WeeklySummaryDTO> weeklySummary { get; set; }
    }
}
