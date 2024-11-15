using WolfDen.Domain.Enums;

namespace WolfDen.Application.Requests.DTOs.Attendence
{
    public class AttendenceLogDTO
    {
        public DateTime Time { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public DirectionType Direction { get; set; } 
     
    }
}
