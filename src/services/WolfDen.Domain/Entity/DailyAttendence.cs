using WolfDen.Domain.Enums;

namespace WolfDen.Domain.Entity
{
    public class DailyAttendence
    {
        public int Id { get; }
        public int EmployeeId { get; private set; }
        public DateOnly Date {  get;private set; }
        public DateTimeOffset ArrivalTime { get; private set; }
        public DateTimeOffset? DepartureTime { get; private set; }
        public int? InsideDuration { get;private set; }
        public int? OutsideDuration { get;private set; }
        public int? PantryDuration { get;private set; }
        public string? MissedPunch {  get;private set; }
        public int? AttendanceStatusId { get; private set; }
        private DailyAttendence()
        {
            
        }
        public DailyAttendence(int employeeId, DateOnly date, DateTimeOffset arrivalTime, DateTimeOffset departureTime, int insideDuration, int outsideDuration, string missedPunch,int attendanceStatusId)
        {
            EmployeeId = employeeId;
            Date = date;
            ArrivalTime = arrivalTime;
            DepartureTime = departureTime;
            InsideDuration = insideDuration;
            OutsideDuration = outsideDuration;
            MissedPunch = missedPunch;
            AttendanceStatusId = attendanceStatusId;
        }
    }
}
