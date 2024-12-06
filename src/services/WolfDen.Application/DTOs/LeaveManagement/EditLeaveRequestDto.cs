namespace WolfDen.Application.DTOs.LeaveManagement
{
    public class EditLeaveRequestDto
    {
        public int LeaveRequestId {  get; set; }
        public int TypeId { get; set; }
        public bool? HalfDay { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string Description { get; set; }


    }
}
