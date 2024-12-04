namespace WolfDen.Application.DTOs.Attendence
{
    public class MonthlyReportDTO
    {
        public int Present { get; set; }
        public int Absent { get; set; }
        public int IncompleteShift { get; set; }
        public int Holiday  { get; set; }
        public string IncompleteShiftDays { get; set; }
        public string RestrictedHolidays { get; set; }
        public string NormalHolidays { get; set; }
        public string WFHDays { get; set; }
        public string LeaveDays { get; set; }
        public int Leave { get; set; }
        public string AbsentDays { get; set; }
        public int WFH{ get; set; }
        public int HalfDays { get; set; }
        public string HalfDayLeaves { get; set; }
    }
}
