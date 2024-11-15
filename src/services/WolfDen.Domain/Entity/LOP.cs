namespace WolfDen.Domain.Entity
{
    public class LOP
    {
        public int Id { get; private set; }
        public int Month { get;private set; }
        public int EmployeeId { get; private set; }
        public int LOPDays { get; private set; }
        public virtual Employee Employee { get; private set; }
        private LOP()
        {
            
        }
        public LOP(int month,int LOPdays, int employeeId, int lOPDays, Employee employee)
        {
            Month = month;
            LOPDays = LOPdays;
            EmployeeId = employeeId;
            LOPDays = lOPDays;
            Employee = employee;
        }
    }
}
