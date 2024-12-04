using System.ComponentModel;
using WolfDen.Domain.Enums;

namespace WolfDen.Domain.Entity
{
    public class Holiday
    {
        public int Id { get; }
        public DateOnly Date { get;private set; }
        public AttendanceStatus Type { get;private set; }

        public string Description {  get; private set; }
        private Holiday()
        {
            
        }
        public Holiday(DateOnly date, AttendanceStatus type)
        {
            Date = date;
            Type = type;
        }

        public Holiday(DateOnly date, AttendanceStatus type, string description)
        {
            Date = date;
            Type = type;
            Description = description;
        }
    }
}
