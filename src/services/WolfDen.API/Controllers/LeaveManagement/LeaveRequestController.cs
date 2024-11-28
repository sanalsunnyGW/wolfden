using MediatR;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetLeaveRequestHistory;
using WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetApprovedNextWeekLeaves;

namespace WolfDen.API.Controllers.LeaveManagement
{
    [Route("api/leave-request")]
    [ApiController]
    public class LeaveRequestController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<LeaveRequestHistoryResponseDto> GetLeaveRequestHistory([FromQuery] GetLeaveRequestHistoryQuery query, CancellationToken cancellationToken)
        { 
            return await _mediator.Send(query,cancellationToken); 
        }

        [HttpGet("on-leave")]
        public async Task<List<LeaveRequestDto>> GetApprovedLeaves([FromQuery] GetNextWeekApprovedLeaveQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }
    }
}
