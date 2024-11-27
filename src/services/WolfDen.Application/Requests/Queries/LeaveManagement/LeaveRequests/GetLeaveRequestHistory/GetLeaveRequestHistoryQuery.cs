using MediatR;
using WolfDen.Application.DTOs.LeaveManagement;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetLeaveRequestHistory
{
    public class GetLeaveRequestHistoryQuery : IRequest<List<LeaveRequestDto>>
    {
        public int RequestId { get; set; }
        public int TypeId { get; set; }
     
    }
}
