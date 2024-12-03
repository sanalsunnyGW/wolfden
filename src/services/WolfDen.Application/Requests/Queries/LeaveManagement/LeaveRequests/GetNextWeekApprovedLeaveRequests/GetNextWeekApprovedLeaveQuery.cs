using MediatR;
using WolfDen.Application.DTOs.LeaveManagement;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetApprovedNextWeekLeaves
{
    public class GetNextWeekApprovedLeaveQuery : IRequest<List<LeaveRequestDto>>
        {
            public int? EmployeeId { get; set; }
        }
    
}
