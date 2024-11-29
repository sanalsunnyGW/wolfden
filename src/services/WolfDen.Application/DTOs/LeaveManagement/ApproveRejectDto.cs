namespace WolfDen.Application.DTOs.LeaveManagement
{
    public class ApproveRejectDto
    {
        public int EmployeeId { get; set; }
        public int TypeId { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }   
        public DateOnly ApplyDate { get; set; } 
        public bool? Sandwich { get; set; }
    }
}
