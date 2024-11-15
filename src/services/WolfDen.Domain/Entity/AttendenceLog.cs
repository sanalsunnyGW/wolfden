namespace WolfDen.Domain.Entity
{
    public class AttendenceLog
    {
        public int Id { get;private set; }
        public int EmployeeId { get;private set; }
        public DateOnly Date { get; private set; }
        public DateTime Time { get;private set; }
        public int DeviceId { get;private set; }
        public string Direction { get;private set; }
        public virtual Employee Employee { get;private set; }
        public virtual Device Device { get;private set; }

        private AttendenceLog() { }
        public AttendenceLog(int employeeId,DateOnly date,DateTime time,int deviceId,string direction,Employee employee)
        {
            EmployeeId = employeeId;
            Date=date;
            Time=time;
            DeviceId=deviceId;
            Direction=direction;    
            Employee = employee;
        }

    }
}
