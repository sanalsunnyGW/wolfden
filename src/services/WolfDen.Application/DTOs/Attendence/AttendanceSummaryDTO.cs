namespace WolfDen.Application.DTOs.Attendence
{
    public class AttendanceSummaryDTO
    {
        public int Present { get; set; }
        public int Absent { get; set; }
        public int IncompleteShift { get; set; }
        public int RestrictedHoliday { get; set; }
        public int NormalHoliday { get; set; }
        public int WFH { get; set; }
        public int Leave { get; set; }
    }

}
