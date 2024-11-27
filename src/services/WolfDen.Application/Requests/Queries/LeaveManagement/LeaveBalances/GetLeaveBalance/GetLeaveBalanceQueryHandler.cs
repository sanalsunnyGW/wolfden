using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveBalances.GetLeaveBalance
{
    public class GetLeaveBalanceQueryHandler(WolfDenContext context) : IRequestHandler<GetLeaveBalanceQuery, List<LeaveBalanceDto>>
    {
        private readonly WolfDenContext _context = context;
        public async Task<List<LeaveBalanceDto>> Handle(GetLeaveBalanceQuery request, CancellationToken cancellationToken)
            {
                List<LeaveBalanceDto> leaveBalanceDtosList = await _context.LeaveBalances
                        .Where(x => x.EmployeeId.Equals(request.EmployeeId))
                        .Include(x => x.LeaveType)
                        .Select(leave => new LeaveBalanceDto
                        {
                            Name = leave.LeaveType.TypeName,
                            Balance = leave.Balance
                        })
                        .ToListAsync(cancellationToken);

                return leaveBalanceDtosList;
            }
    }
}
