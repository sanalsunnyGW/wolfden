using MediatR;
using WolfDen.Domain.Enums;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.ApproveOrRejectLeaveRequest
{
    public class ApproveOrRejectLeaveRequestCommand : IRequest<bool>
    {
        public int SuperiorId { get; set; }
        public int LeaveRequestId { get; set; }
        public LeaveRequestStatus statusId { get; set; }

    }
}
