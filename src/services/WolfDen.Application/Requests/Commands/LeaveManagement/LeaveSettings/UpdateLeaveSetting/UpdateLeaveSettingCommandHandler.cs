using System.ComponentModel.DataAnnotations;
using System.Linq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveSettings.UpdateLeaveSetting
{
    public class UpdateLeaveSettingCommandHandler : IRequestHandler<UpdateLeaveSettingCommand, bool>
    {
        private readonly WolfDenContext _context;
        private readonly UpdateLeaveSettingValidator _validator;

        public UpdateLeaveSettingCommandHandler(WolfDenContext context,UpdateLeaveSettingValidator validator)
        {
            _context = context;
            _validator = validator;
        }
        public async Task<bool> Handle(UpdateLeaveSettingCommand request, CancellationToken cancellationToken)
        {

            var result = await _validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Validation failed: {errors}");
            }

            LeaveSetting leaveSetting = await _context.LeaveSettings.FirstOrDefaultAsync(cancellationToken);
            if (leaveSetting == null)
            {
                throw new KeyNotFoundException("LeaveSetting record not found.");
            }
            leaveSetting.UpdateLeaveSetting(request.MinDaysForLeaveCreditJoining, request.MaxNegativeBalanceLimit);
            _context.LeaveSettings.Update(leaveSetting);
           int Saveresult =  await _context.SaveChangesAsync(cancellationToken);
            return Saveresult>0;
        }
    }
}
