namespace WolfDen.Application.DTOs.LeaveManagement
{
     public class SubordinateLeaveRequestPaginationDto
    {
       public List<SubordinateLeaveDto> SubordinateLeaveDtosList { get; set; }
       public int TotalDataCount { get; set; }
    }
}
