using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveTypes.NewFolder
{
    public class GetLeaveTypeQueryHandler(WolfDenContext context) : IRequestHandler<GetLeaveTypeQuery, LeaveTypeDto>
    {
        private readonly WolfDenContext _context = context;

        public async Task<LeaveTypeDto> Handle(GetLeaveTypeQuery request, CancellationToken cancellationToken)
        {
            LeaveTypeDto leaveTypeDetails = await _context.LeaveTypes
                .Where(x => x.Id.Equals(request.RequestId))
                .Select(x => new LeaveTypeDto
                {
                    MaxDays = x.MaxDays,
                    IsHalfDayAllowed = x.IsHalfDayAllowed,
                    IncrementCount = x.IncrementCount,
                    IncrementGapId = x.IncrementGapId,
                    CarryForward = x.CarryForward,
                    CarryForwardLimit = x.CarryForwardLimit,
                    DaysCheck = x.DaysCheck,
                    DaysCheckEqualOrLess = x.DaysCheckEqualOrLess,
                    DaysCheckMore = x.DaysCheckMore,
                    DutyDaysRequired = x.DutyDaysRequired,
                    Sandwich = x.Sandwich
                })
                .FirstAsync(cancellationToken);

            return leaveTypeDetails;
        }
    }
}
