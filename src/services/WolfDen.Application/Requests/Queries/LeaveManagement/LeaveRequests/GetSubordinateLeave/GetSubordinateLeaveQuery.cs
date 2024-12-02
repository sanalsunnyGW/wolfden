using MediatR;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Domain.Enums;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetSubordinateLeave
{
    public class GetSubordinateLeaveQuery : IRequest<SubordinateLeaveRequestPaginationDto>
    {
        public int Id { get; set; }
        public LeaveRequestStatus StatusId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

    }
}
