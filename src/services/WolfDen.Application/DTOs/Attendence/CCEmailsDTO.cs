namespace WolfDen.Application.Requests.DTOs.Attendence
{
    public class CCEmailsDTO
    {
        public string Name { get; set; }=string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? ManagerId { get; set; } = null;
    }
}
