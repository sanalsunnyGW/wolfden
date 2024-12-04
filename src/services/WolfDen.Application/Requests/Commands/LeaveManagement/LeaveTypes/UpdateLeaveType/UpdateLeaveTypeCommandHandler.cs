using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveTypes.UpdateLeaveType
{
    public class UpdateLeaveTypeCommandHandler(WolfDenContext context,UpdateLeaveTypeValidator validator) : IRequestHandler<UpdateLeaveTypeCommand, bool>
    {
        private readonly WolfDenContext _context = context;
        private readonly UpdateLeaveTypeValidator _validator = validator;
        public async Task<bool> Handle(UpdateLeaveTypeCommand command, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(command, cancellationToken);

            if (!result.IsValid)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Validation failed: {errors}");
            }
            LeaveType leaveType = await _context.LeaveType.Where(x => x.Id.Equals(command.Id)).FirstOrDefaultAsync(cancellationToken);
            leaveType.updateLeaveType(command.MaxDays, command.IsHalfDayAllowed, command.IncrementCount, command.IncrementGapId, command.CarryForward, command.CarryForwardLimit, command.DaysCheck, command.DaysCheckMore, command.DaysCheckEqualOrLess, command.DutyDaysRequired, command.Sandwich);
            _context.LeaveType.Update(leaveType);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
