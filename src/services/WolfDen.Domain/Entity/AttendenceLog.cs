using WolfDen.Domain.Enums;

namespace WolfDen.Domain.Entity
{
    public class AttendenceLog
    {
        public int Id { get;}
        public int EmployeeId { get;private set; }
        public DateOnly Date { get; private set; }
        public DateTime Time { get;private set; }
        public int DeviceId { get;private set; }
        public DirectionType Direction { get;private set; }

        public int DailyAttendenceId { get;private set; }
        public virtual DailyAttendence DailyAttendence { get;private set; }
    
        public virtual Device Device { get;private set; }

        private AttendenceLog() { }
        public AttendenceLog(int employeeId,DateOnly date,DateTime time,int deviceId,DirectionType direction)
        {
            EmployeeId = employeeId;
            Date=date;
            Time=time;
            DeviceId=deviceId;
            Direction=direction;    
           
        }

    }
}
