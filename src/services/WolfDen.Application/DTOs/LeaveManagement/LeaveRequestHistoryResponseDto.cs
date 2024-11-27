namespace WolfDen.Application.DTOs.LeaveManagement
{
    public class LeaveRequestHistoryResponseDto
    {
        public List<LeaveRequestDto> LeaveRequests { get; set; }
        public int TotalPages { get; set; }
    }
}
