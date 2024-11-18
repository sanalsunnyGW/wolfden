namespace WolfDen.Domain.Entity
{
    public class AttendenceClose
    {
        public int Id { get;}
        public int Month { get;private set; }
        public int Year { get;private set; }

        public bool IsClosed { get;private set; }

        private AttendenceClose()
        {
            
        }
        public AttendenceClose(int month,int year,bool isClosed)
        {
            Month = month;
            Year = year;
            IsClosed = isClosed;
        }
    }
}
