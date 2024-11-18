using MediatR;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.AddLeaveRequest
{
    public class AddLeaveRequestCommand : IRequest<string>
    {
        int Id { get; set; }    
        int TypeId { get; set; }    
        bool HalfDay { get; set; }
        DateOnly FromDate { get; set; }
        DateOnly ToDate { get; set; }   
        string Description { get; set; }

    }
}
