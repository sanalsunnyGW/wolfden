namespace WolfDen.Domain.Entity
{
    public class AttendenceClose
    {
        public int Id { get;}
        public string Month { get; private set; }
        public int Year { get; private set; }
        public DateOnly AttendanceClosedDate { get; private set; }
        public bool IsClosed { get;private set; }

        private AttendenceClose()
        {
            
        }
        public AttendenceClose(DateOnly attendanceClosedDate, bool isClosed,string month,int year)
        {
           AttendanceClosedDate = attendanceClosedDate;
            IsClosed = isClosed;
            Month=month;
            Year=year;
        }

    }
}
