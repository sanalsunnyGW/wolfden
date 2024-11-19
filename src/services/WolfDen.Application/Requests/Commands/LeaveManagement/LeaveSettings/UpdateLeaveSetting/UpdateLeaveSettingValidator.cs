using FluentValidation;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveSettings.UpdateLeaveSetting
{
    public class UpdateLeaveSettingValidator:AbstractValidator<UpdateLeaveSettingCommand>
    {
        public UpdateLeaveSettingValidator()
        {
            RuleFor(x => x.MinDaysForLeaveCreditJoining).NotEmpty().WithMessage("Minimum Days For Leave Credit On Joining Is Required");
            RuleFor(x => x.MaxNegativeBalanceLimit).NotEmpty().WithMessage("Maximum Negative Balance Limit of giving leave is Required");  
        }

    }
}
