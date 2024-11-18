namespace WolfDen.Application.Requests.DTOs.Attendence
{
    public class DailyAttendanceDTO
    {
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public int InsideHours { get; set; }
        public string Status { get; set; }
        public int OutsideHours { get;set; }
        public string MissedPunch { get; set; }
        public List<AttendenceLogDTO> DailyLog { get; set; }
    }
}
