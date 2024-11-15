namespace WolfDen.Application.Requests.DTOs.Attendence
{
    public class AttendenceLogDTO
    {
        public DateTime Time { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public string Direction { get; set; } = string.Empty;
    }
}
