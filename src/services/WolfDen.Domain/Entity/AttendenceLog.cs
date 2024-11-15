using WolfDen.Domain.Enums;

namespace WolfDen.Domain.Entity
{
    public class AttendenceLog
    {
        public int Id { get;}
        public int EmployeeId { get;private set; }
        public DateOnly PunchDate { get; private set; }
        public DateTime PunchTime { get;private set; }
        public int DeviceId { get; set; }
        public DirectionType Direction { get;set; }
        public virtual Device Device { get;set; }

        private AttendenceLog() { }
        public AttendenceLog(int employeeId,DateOnly date,DateTime time,int deviceId,DirectionType direction)
        {
            EmployeeId = employeeId;
            PunchDate=date;
            PunchTime=time;
            DeviceId=deviceId;
            Direction=direction;    
           
        }

    }
}
