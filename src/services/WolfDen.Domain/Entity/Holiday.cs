using WolfDen.Domain.Enums;

namespace WolfDen.Domain.Entity
{
    public class Holiday
    {
        public int Id { get; }
        public DateOnly Date { get;private set; }
        public AttendanceStatus Type { get;private set; }
        private Holiday()
        {
            
        }
        public Holiday(DateOnly date, AttendanceStatus type)
        {
            Date = date;
            Type = type;
        }
    }
}
