namespace WolfDen.Domain.Entity
{
    public class LOP
    {
        public int Id { get;  }
        public int Month { get;private set; }
        public int EmployeeId { get; private set; }
        public int LOPDays { get; private set; }
        private LOP()
        {
            
        }
        public LOP(int month,int LOPdays, int employeeId, int lOPDays)
        {
            Month = month;
            LOPDays = LOPdays;
            EmployeeId = employeeId;
            LOPDays = lOPDays;
           
        }
    }
}
