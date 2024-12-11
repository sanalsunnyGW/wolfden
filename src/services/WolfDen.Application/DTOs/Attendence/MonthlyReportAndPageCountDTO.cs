namespace WolfDen.Application.DTOs.Attendence
{
    public class MonthlyReportAndPageCountDTO
    {
        public List<AllEmployeesMonthlyReportDTO> AllEmployeesMonthlyReports { get; set; }
        public int? PageCount { get; set; }

    }
}
