namespace WolfDen.Domain.Entity
{
    public class LeaveRequestDay
    {
        public int Id { get;private set; }
        public int LeaveRequestId { get;private set; }
        public DateOnly LeaveDate { get; private set; }
        public virtual LeaveRequest LeaveRequest { get; private set; }
        private LeaveRequestDay() {  }
        public LeaveRequestDay (int leaveRequestId,DateOnly leaveDate)
        {
            LeaveRequestId = leaveRequestId;
            LeaveDate = leaveDate;
        }
    }
}
