namespace WolfDen.Domain.Entity
{
    public class DailyAttendence
    {
        public int Id { get; }
        public int EmployeeId { get; private set; }
        public DateOnly Date {  get;private set; }
        public DateTime ArrivalTime { get; private set; }
        public DateTime DepartureTime { get; private set; }
        public int InsideDuration { get;private set; }
        public int OutsideDuration { get;private set; }
        public int PantryDuration { get;private set; }
        public string MissedPunch {  get;private set; }
        public virtual AttendanceStatus AttendanceStatus { get; set; }
        public Status Status { get; private set; }
      
        private DailyAttendence()
        {
            
        }
        public DailyAttendence(int employeeId, DateOnly date, DateTime arrivalTime, DateTime departureTime, int insideDuration, int outsideDuration, string missedPunch)
        {
            EmployeeId = employeeId;
            Date = date;
            ArrivalTime = arrivalTime;
            DepartureTime = departureTime;
            InsideDuration = insideDuration;
            OutsideDuration = outsideDuration;
            MissedPunch = missedPunch;   
        }


    }
}
