namespace WolfDen.Domain.Entity
{
    public class LeaveSetting
    {
        public int Id {  get;private set; }
        public int MinDaysForLeaveCreditJoining { get; private set; }
        public int MaxNegativeBalanceLimit { get;private set; }
        private LeaveSetting() { }
        public LeaveSetting(int minDaysForLeaveCreditJoining,int maxNegativeBalanceLimit)
        {
            MinDaysForLeaveCreditJoining = minDaysForLeaveCreditJoining;
            MaxNegativeBalanceLimit = maxNegativeBalanceLimit;  
        }

        public void UpdateLeaveSetting(int minDaysForLeaveCreditJoining, int maxNegativeBalanceLimit)
        {
            MinDaysForLeaveCreditJoining = minDaysForLeaveCreditJoining;
            MaxNegativeBalanceLimit = maxNegativeBalanceLimit;
        }
    }
}
