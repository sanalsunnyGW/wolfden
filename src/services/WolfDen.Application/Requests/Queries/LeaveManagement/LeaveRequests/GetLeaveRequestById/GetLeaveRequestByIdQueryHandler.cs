using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetLeaveRequestById
{
    public class GetLeaveRequestByIdQueryHandler(WolfDenContext context) : IRequestHandler<GetLeaveRequestByIdQuery, EditLeaveRequestDto>
    {
        private readonly WolfDenContext _context = context;

        public async Task<EditLeaveRequestDto> Handle(GetLeaveRequestByIdQuery request, CancellationToken cancellationToken)
        {
            EditLeaveRequestDto editLeaveRequestDto = await _context.LeaveRequests.Where(x => x.Id.Equals(request.leaveRequestId))
                .Select(x => new EditLeaveRequestDto
                {
                    LeaveRequestId = x.Id,
                    TypeId = x.TypeId,
                    HalfDay = x.HalfDay,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    Description = x.Description,

                }).FirstOrDefaultAsync(cancellationToken);
            if (editLeaveRequestDto == null) 
            {
                throw new Exception("No Such Leave Request");
            }
            return editLeaveRequestDto;

        }
    }
}
