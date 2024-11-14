namespace WolfDen.Domain.Entity
{
    public class AttendenceClose
    {
        public int Id { get;private set; }
        public int Month { get;private set; }
        public bool IsClosed { get;private set; }

        public AttendenceClose()
        {
            
        }
        public AttendenceClose(int month,bool isClosed)
        {
            month=Month;
            isClosed=IsClosed;
        }
    }
}
