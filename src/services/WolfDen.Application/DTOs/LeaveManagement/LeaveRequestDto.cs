using WolfDen.Domain.Enums;

namespace WolfDen.Application.DTOs.LeaveManagement
{
    public class LeaveRequestDto
    {
       public int Id { get; set; }  
        public string? Name { get; set; }
        public string TypeName { get; set; }  
        public bool? HalfDay { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public DateOnly ApplyDate { get;set; }
        public LeaveRequestStatus LeaveRequestStatusId { get; set; }
        public string Description { get; set; }
        public string ProcessedBy { get; set; }
    }
}
