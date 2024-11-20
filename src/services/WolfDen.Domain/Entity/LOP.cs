namespace WolfDen.Domain.Entity
{
    public class LOP
    {
        public int Id { get;  }
        public DateOnly AttendanceClosedDate { get;private set; }
        public int EmployeeId { get; private set; }
        public int LOPDaysCount { get; private set; }
        public string LOPDays { get; private set; }
        private LOP()
        {
            
        }
        public LOP(DateOnly attendanceClosedDate, int lOPdaysCount, int employeeId,string lopDays)
        {
            AttendanceClosedDate=attendanceClosedDate;
            LOPDaysCount = lOPdaysCount;
            EmployeeId = employeeId;
            LOPDays = lopDays;
        }
    }
}
