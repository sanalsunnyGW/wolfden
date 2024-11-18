namespace WolfDen.Domain.Entity
{
    public class Status
    {
        public int Id { get; }
        public int EmployeeId { get;private set; }
        public DateOnly Date { get;private set; }
        public int StatusTypeId { get;private set; }
        public virtual AttendanceStatus AttendanceStatus { get; private set; }
        private Status()
        {
            
        }
        public Status(int employeeId, DateOnly date,int statusId)
        {
            EmployeeId= employeeId;
            Date= date;
            StatusTypeId = statusId;
        }
    }
}
