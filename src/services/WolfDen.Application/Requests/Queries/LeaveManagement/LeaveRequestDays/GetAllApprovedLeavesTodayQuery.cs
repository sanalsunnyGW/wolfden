using MediatR;
using WolfDen.Application.DTOs.LeaveManagement;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequestDays
{
    public class GetAllApprovedLeavesTodayQuery : IRequest<List<PresentDayApprovedLeavesDto>>
    {
    }
}
