using MediatR;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveTypeCommand
{
    public class LeaveTypeAddCommandHandler : IRequestHandler<LeaveTypeAddCommand, int>
    {
        private readonly WolfDenContext _context;

        public LeaveTypeAddCommandHandler(WolfDenContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(LeaveTypeAddCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == 1)
            {
                LeaveType leaveType = new LeaveType(request.TypeName, request.MaxDays, request.HalfDays, request.IncrementCount,
                    request.IncrementGap, request.CarryForward, request.CarryForwardLimit, request.DaysCheck, request.DaysChekcMore,
                    request.DaysCheckEqualOrLess, request.DutyDaysRequired, request.Hidden, request.RestrictionType);

                _context.LeaveType.Add(leaveType);
                return await _context.SaveChangesAsync(cancellationToken);
            }
            return -1;
        }
    }
}
