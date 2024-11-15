namespace WolfDen.Domain.Entity
{
    public class Holiday
    {
        public int Id { get;private set; }
        public DateOnly Date { get;private set; }
        public string Type { get;private set; }
        private Holiday()
        {
            
        }
        public Holiday(DateOnly date, string type)
        {
            Date = date;
            Type = type;
        }
    }
}
