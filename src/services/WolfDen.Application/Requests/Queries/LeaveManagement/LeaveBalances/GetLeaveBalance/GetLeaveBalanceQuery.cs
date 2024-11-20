using MediatR;
using WolfDen.Application.DTOs.LeaveManagement;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveBalances.GetLeaveBalance
{
    public class GetLeaveBalanceQuery : IRequest<List<LeaveBalanceDto>>
    {
        public int RequestId { get; set; }
    }
}
