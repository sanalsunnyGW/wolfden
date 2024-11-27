namespace WolfDen.Domain.Entity
{
    public class Notification
    {
        public int Id { get; }
        public int  EmployeeId { get;private set; }
        public string Message { get;private set; }
        public DateTime CreatedAt { get;private set; }
        public bool IsRead { get;private set; }
        private Notification()
        {
            
        }
        public Notification(int employeeId, string message)
        {
            EmployeeId = employeeId;
            Message = message;
            CreatedAt = DateTime.Now;
            IsRead = false;
           
        }
    }
}
