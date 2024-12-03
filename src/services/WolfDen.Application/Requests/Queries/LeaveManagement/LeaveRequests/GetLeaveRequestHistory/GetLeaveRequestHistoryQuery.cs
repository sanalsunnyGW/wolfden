using MediatR;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Domain.Enums;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetLeaveRequestHistory
{
    public class GetLeaveRequestHistoryQuery : IRequest<LeaveRequestHistoryResponseDto>
    {
        public int EmployeeId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public LeaveRequestStatus? LeaveStatusId { get; set; }
     
    }
}
