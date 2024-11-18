namespace WolfDen.Domain.Entity
{
    public class LOP
    {
        public int Id { get;  }
        public DateOnly AttendanceClosedDate { get;private set; }
        public int EmployeeId { get; private set; }
        public int LOPDays { get; private set; }
        private LOP()
        {
            
        }
        public LOP(DateOnly attendanceClosedDate, int lOPdays, int employeeId)
        {
            AttendanceClosedDate=attendanceClosedDate;
            LOPDays = lOPdays;
            EmployeeId = employeeId;   
        }
    }
}
