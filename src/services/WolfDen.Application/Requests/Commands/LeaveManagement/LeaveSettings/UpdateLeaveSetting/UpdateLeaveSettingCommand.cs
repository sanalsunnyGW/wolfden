using MediatR;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveSettings.UpdateLeaveSetting
{
    public class UpdateLeaveSettingCommand:IRequest<bool>
    {
        public int MinDaysForLeaveCreditJoining {  get; set; }
        public int MaxNegativeBalanceLimit { get; set; }

    }
}
