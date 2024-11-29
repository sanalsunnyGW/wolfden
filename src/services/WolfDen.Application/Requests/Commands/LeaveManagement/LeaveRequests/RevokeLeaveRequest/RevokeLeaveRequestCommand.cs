using MediatR;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.RevokeLeaveRequest
{
    public class RevokeLeaveRequestCommand : IRequest<bool>
    {
        public int LeaveRequestId { get; set; }
        public int EmployeeId { get; set; } 
    }
}
