namespace WolfDen.Application.DTOs.Attendence
{
    public class AllEmployeesMonthlyReportDTO
    {
        public int EmployeeId {  get; set; }
        public string EmployeeName { get; set; }
        public int NoOfAbsentDays { get; set; }
        public string AbsentDays { get; set; }
        public int NofIncompleteShiftDays { get; set; }
        public string IncompleteShiftDays { get; set; }
    }
}
