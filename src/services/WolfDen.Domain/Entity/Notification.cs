namespace WolfDen.Domain.Entity
{
    public class Notification
    {
        public int Id { get;private set; }
        public int  EmployeeId { get;private set; }
        public string Message { get;private set; }
        public DateTime CreatedAt { get;private set; }
        public bool IsAcknowledged { get;private set; }
        public virtual Employee Employee { get;private set; }
        public Notification()
        {
            
        }
        public Notification(int employeeId, string message, DateTime createdAt, bool isAcknowledged, Employee employee)
        {
            EmployeeId = employeeId;
            Message = message;
            CreatedAt = createdAt;
            IsAcknowledged = isAcknowledged;
            Employee = employee;
        }

    }
}
