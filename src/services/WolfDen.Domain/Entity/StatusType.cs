using System.ComponentModel.DataAnnotations;

namespace WolfDen.Domain.Entity
{
    public class StatusType
    {
        [Key]
        public int Id { get; }
      
        
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
