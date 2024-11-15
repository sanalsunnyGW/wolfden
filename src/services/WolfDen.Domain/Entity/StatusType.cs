namespace WolfDen.Domain.Entity
{
    public class StatusType
    {
        public int Id { get;private set; }
        public string StatusName { get;private set; }
        private StatusType()
        {
            
        }
        public StatusType(string statusName)
        {
            statusName=StatusName;
        }
    }
}
