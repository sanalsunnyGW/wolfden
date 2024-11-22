namespace WolfDen.Application.DTOs.Attendence
{
    public class MonthlyReportDTO
    {
        public int Present { get; set; }
        public int Absent { get; set; }
        public int IncompleteShift { get; set; }
        public int Holiday  { get; set; }
        public String IncompleteShiftDays { get; set; }
        public String RestrictedHolidays { get; set; }
        public String NormalHolidays { get; set; }
        public String WFHDays { get; set; }
        public String LeaveDays { get; set; }
        public int Leave { get; set; }
        public String AbsentDays { get; set; }
        public int WFH{ get; set; }
    }
}
