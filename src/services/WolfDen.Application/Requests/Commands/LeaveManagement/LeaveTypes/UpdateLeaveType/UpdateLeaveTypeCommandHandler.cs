using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveTypes.UpdateLeaveType
{
    public class UpdateLeaveTypeCommandHandler(WolfDenContext context) : IRequestHandler<UpdateLeaveTypeCommand, bool>
    {
        private readonly WolfDenContext _context = context;

        public async Task<bool> Handle(UpdateLeaveTypeCommand command, CancellationToken cancellationToken)
        {
            LeaveType leaveType = await _context.LeaveType.Where(x => x.Id.Equals(command.Id)).FirstOrDefaultAsync();
            leaveType.updateLeaveType(command.MaxDays, command.IsHalfDayAllowed, command.IncrementCount, command.IncrementGapId, command.CarryForward, command.CarryForwardLimit, command.DaysCheck, command.DaysCheckMore, command.DaysCheckEqualOrLess, command.DutyDaysRequired, command.Sandwich);
            _context.LeaveType.Update(leaveType);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
