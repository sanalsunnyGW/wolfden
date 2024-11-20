using LanguageExt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetLeaveRequestHistory
{
    public class GetLeaveRequestHistoryQueryHandler(WolfDenContext context) : IRequestHandler<GetLeaveRequestHistoryQuery, List<LeaveRequestDto>>
    {
        private readonly WolfDenContext _context = context;

        public async Task<List<LeaveRequestDto>> Handle(GetLeaveRequestHistoryQuery request, CancellationToken cancellationToken)
        {
                List<LeaveRequestDto> leaveRequestList = await _context.LeaveRequests
                .Where(x => x.EmployeeId.Equals(request.RequestId))
                .Include(x => x.LeaveType)
                .Join(
                    _context.Employees,
                    leaveRequest => leaveRequest.ProcessedBy,
                    employee => employee.Id,
                    (leaveRequest, employee) => new LeaveRequestDto
                    {
                        FromDate = leaveRequest.FromDate,
                        ToDate = leaveRequest.ToDate,
                        ApplyDate = leaveRequest.ApplyDate,
                        TypeName = leaveRequest.LeaveType.TypeName,
                        HalfDay = leaveRequest.HalfDay,
                        Description = leaveRequest.Description,
                        ProcessedBy = employee.FirstName,
                        LeaveRequestStatusId = leaveRequest.LeaveRequestStatusId
                    })
                .ToListAsync(cancellationToken);

            return leaveRequestList;
        }
    }
}
