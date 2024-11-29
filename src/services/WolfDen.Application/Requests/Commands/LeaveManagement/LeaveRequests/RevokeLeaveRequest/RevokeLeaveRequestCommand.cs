using MediatR;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.RevokeLeaveRequest
{
    public class RevokeLeaveRequestCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; } 
    }
}
