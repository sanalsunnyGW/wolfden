namespace WolfDen.Domain.Entity
{
    public class AttendenceClose
    {
        public int Id { get;}
        public DateOnly AttendanceClosedDate { get; private set; }
        public bool IsClosed { get;private set; }

        private AttendenceClose()
        {
            
        }
        public AttendenceClose(DateOnly attendanceClosedDate, bool isClosed)
        {
           AttendanceClosedDate = attendanceClosedDate;
            IsClosed = isClosed;
        }
    }
}
