namespace WolfDen.Domain.Entity
{
    public class LOP
    {
        public int Id { get;  }
        public int Month { get;private set; }
        public int Year { get; private set; }
        public int EmployeeId { get; private set; }
        public int LOPDays { get; private set; }
        private LOP()
        {
            
        }
        public LOP(int month,int year,int lOPdays, int employeeId)
        {
            Month = month;
            Year = year;
            LOPDays = lOPdays;
            EmployeeId = employeeId;   
        }
    }
}
