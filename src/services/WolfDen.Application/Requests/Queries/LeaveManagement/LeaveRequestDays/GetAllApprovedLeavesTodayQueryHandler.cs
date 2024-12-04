using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequestDays
{
    public class GetAllApprovedLeavesTodayQueryHandler(WolfDenContext context) : IRequestHandler<GetAllApprovedLeavesTodayQuery, List<PresentDayApprovedLeavesDto>>
    {
        private readonly WolfDenContext _context = context;
        public async Task<List<PresentDayApprovedLeavesDto>> Handle(GetAllApprovedLeavesTodayQuery request, CancellationToken cancellationToken)
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            List<PresentDayApprovedLeavesDto>  presentDayApprovedLeavesDtosList = await _context.LeaveRequestDays
                .Where(x => x.LeaveDate == currentDate && x.LeaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Approved)
                .Select(x => new PresentDayApprovedLeavesDto
                {
                    EmployeeId = x.LeaveRequest.EmployeeId,
                    EmployeeName = x.LeaveRequest.Employee.FirstName +" "+ x.LeaveRequest.Employee.LastName,
                    EmployeeCode = x.LeaveRequest.Employee.EmployeeCode,
                }).ToListAsync(cancellationToken);


             return presentDayApprovedLeavesDtosList;
        }
    }
}
