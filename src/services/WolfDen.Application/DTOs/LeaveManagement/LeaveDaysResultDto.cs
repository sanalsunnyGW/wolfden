namespace WolfDen.Application.DTOs.LeaveManagement
{
    public class LeaveDaysResultDto
    {
        public decimal DaysCount { get; set; }
        public List<DateOnly> ValidDate {  get; set; }  = new List<DateOnly>();

    }
}
