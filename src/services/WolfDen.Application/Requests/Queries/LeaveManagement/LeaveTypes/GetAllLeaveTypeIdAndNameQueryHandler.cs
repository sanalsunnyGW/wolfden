using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveTypes
{
    public class GetAllLeaveTypeIdAndNameQueryHandler(WolfDenContext context) : IRequestHandler<GetAllLeaveTypeIdAndNameQuery, List<LeaveTypeDto>>
    {
        private readonly WolfDenContext _context = context;

        public async Task<List<LeaveTypeDto>> Handle(GetAllLeaveTypeIdAndNameQuery request, CancellationToken cancellationToken)
        {
            List<LeaveTypeDto> leaveTypeDtoList = await _context.LeaveType
                .Where(leaveType => leaveType.LeaveCategoryId != LeaveCategory.EmergencyLeave)
                .Select(leaveType => new LeaveTypeDto
                {
                    Id = leaveType.Id,
                    Name = leaveType.TypeName
                })
                .ToListAsync(cancellationToken);

            if (leaveTypeDtoList is null)
            {
                throw new KeyNotFoundException("LeaveType records not found.");
            }

            return leaveTypeDtoList;
        }
    }
}
