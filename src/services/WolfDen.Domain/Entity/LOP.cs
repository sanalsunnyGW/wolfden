namespace WolfDen.Domain.Entity
{
    public class LOP
    {
        public int Id { get;  }
        public DateOnly AttendanceClosedDate { get;private set; }
        public int EmployeeId { get; private set; }
        public int LOPDaysCount { get; private set; }
        public int NoOfIncompleteShiftDays {  get; private set; }
        public string LOPDays { get; private set; }
        public string IncompleteShiftDays { get; private set; }
        private LOP()
        {
            
        }
        public LOP(DateOnly attendanceClosedDate, int employeeId, int lOPdaysCount, int noOfIncompleteShiftDays,string lopDays, string incompleteShiftDays)
        {
            AttendanceClosedDate = attendanceClosedDate;
            LOPDaysCount = lOPdaysCount;
            EmployeeId = employeeId;
            LOPDays = lopDays;
            IncompleteShiftDays = incompleteShiftDays;
            NoOfIncompleteShiftDays = noOfIncompleteShiftDays;
        }
    }
}
