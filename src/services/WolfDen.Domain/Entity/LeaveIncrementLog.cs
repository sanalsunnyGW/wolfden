namespace WolfDen.Domain.Entity
{
    public class LeaveIncrementLog
    {
        public int Id {  get;private set; }
        public int LeaveBalanceId { get; private set; }
        public virtual LeaveBalance LeaveBalance { get;private set; }
        public DateOnly LogDate { get; private set; }
        public int CurrentBalance { get; private set; }
        public int IncrementValue { get; private set; }
        private LeaveIncrementLog() {  }

        public LeaveIncrementLog(int leaveBalanceId, DateOnly logDate, int currentBalance, int incrementValue)
        {
            LeaveBalanceId = leaveBalanceId;
            LogDate = logDate;
            CurrentBalance = currentBalance;
            IncrementValue = incrementValue;
        }
    }
}
