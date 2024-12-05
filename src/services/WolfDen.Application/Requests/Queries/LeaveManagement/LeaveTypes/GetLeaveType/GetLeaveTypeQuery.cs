using MediatR;
using WolfDen.Application.DTOs.LeaveManagement;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveTypes.NewFolder
{
    public class GetLeaveTypeQuery:IRequest<LeaveTypeDto>
    {
        public int RequestId { get; set; } 
    }
}
