using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveSettings.GetLeaveSettings
{
    public class GetLeaveSettingQueryHandler : IRequestHandler<GetLeaveSettingQuery, LeaveSettingDto>
    {
        private readonly WolfDenContext _context;

        public GetLeaveSettingQueryHandler(WolfDenContext context)
        {
            _context = context;
        }
        public async Task<LeaveSettingDto> Handle(GetLeaveSettingQuery request, CancellationToken cancellationToken)
        {
            LeaveSetting leaveSetting = await _context.LeaveSettings.FirstOrDefaultAsync(cancellationToken);
            if (leaveSetting is null) 
            {
                throw new Exception("Leave Settings Do Not Exist");
            }
        
            LeaveSettingDto leaveSettingDto = new LeaveSettingDto
            {
                MinDaysForLeaveCreditJoining = leaveSetting.MinDaysForLeaveCreditJoining,
                MaxNegativeBalanceLimit = leaveSetting.MaxNegativeBalanceLimit,
            };
            return await Task.FromResult(leaveSettingDto);  
        }
    }
}
