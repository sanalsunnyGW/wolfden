namespace WolfDen.Application.DTOs.Attendence
{
    public class NotificationDTO
    {
        public int NotificationId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
