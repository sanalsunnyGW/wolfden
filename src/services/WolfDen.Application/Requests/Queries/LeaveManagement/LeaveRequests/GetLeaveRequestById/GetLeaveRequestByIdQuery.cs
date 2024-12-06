using MediatR;
using WolfDen.Application.DTOs.LeaveManagement;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetLeaveRequestById
{
    public class GetLeaveRequestByIdQuery : IRequest<EditLeaveRequestDto>
    {
        public int leaveRequestId { get; set; }
    }
}
