

namespace WolfDen.Domain.Entity
{
    public class Status
    {
        public int Id { get;private set; }
        public int EmployeeId { get;private set; }
        public DateOnly Date { get;private set; }
        public int Duration { get;private set; }
        public int StatusId { get;private set; }
        public virtual StatusType StatusType { get; private set; }
        public virtual Employee Employee { get;private set; }
        public Status()
        {
            
        }
        public Status(int employeeId, DateOnly date, int duration,int statusId, StatusType statusType,Employee employee)
        {
            EmployeeId= employeeId;
            Date= date;
            Duration = duration;
            StatusId = statusId;
            StatusType= statusType;
            Employee = employee;

        }
    }
}
