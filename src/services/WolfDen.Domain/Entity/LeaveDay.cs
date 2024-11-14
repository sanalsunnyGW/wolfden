namespace WolfDen.Domain.Entity
{
    public class LeaveDay
    {
        public int Id { get;private set; }
        public int LeaveRequestId { get;private set; }

        public LeaveRequest LeaveRequest { get; private set; }
        public DateOnly LeaveDate { get; private set; }
        private LeaveDay() {  }
    }
}
