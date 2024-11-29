namespace WolfDen.Application.DTOs.LeaveManagement
{
    public class SubordinateLeaveDto
    {
        public int LeaveRequestId { get; set; }
        public string Name { get; set; }
        public int EmployeeCode { get; set; }
        public string TypeName { get; set; }
        public bool? HalfDay { get; set; }
        public DateOnly ApplyDate { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string Description { get; set; }

    }
}
