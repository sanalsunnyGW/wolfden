namespace WolfDen.Domain.Entity
{
    public class LeaveBalance
    {
        public int Id { get; private set; }
        public int EmployeeId { get;private set; }
        public virtual  Employee Employee { get; private set; }
        public int TypeId { get; private set; }
        public virtual LeaveType LeaveType { get; private set; }
        public int Balance { get; set; }
        private LeaveBalance() { }

        public LeaveBalance( int employeeId, int typeId, int balance)
        {
            EmployeeId = employeeId;
            TypeId = typeId;
            Balance = balance;
        }
    }
}
