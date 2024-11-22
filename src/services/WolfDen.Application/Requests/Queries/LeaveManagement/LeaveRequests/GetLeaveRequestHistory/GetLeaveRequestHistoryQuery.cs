using MediatR;
using WolfDen.Application.DTOs.LeaveManagement;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetLeaveRequestHistory
{
    public class GetLeaveRequestHistoryQuery : IRequest<LeaveRequestHistoryResponseDto>
    {
        public int RequestId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
     
    }
}
