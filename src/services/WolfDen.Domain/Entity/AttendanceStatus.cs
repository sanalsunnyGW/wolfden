namespace WolfDen.Domain.Entity
{
    public class AttendanceStatus
    {
        public int Id { get; }
        public string StatusName { get;private set; }
        private AttendanceStatus()
        {
            
        }
        public AttendanceStatus(string statusName)
        {
            StatusName = statusName;
        }
    }
}
