using MediatR;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetLeaveRequestHistory;

namespace WolfDen.API.Controllers.LeaveManagement
{
    [Route("api/leave-request")]
    [ApiController]
    public class LeaveRequestController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id}/{pageNumber}/{pageSize}")]
        public async Task<LeaveRequestHistoryResponseDto> GetLeaveRequestHistory(int id,int pageNumber,int pageSize,CancellationToken cancellationToken)
        {
            GetLeaveRequestHistoryQuery query= new GetLeaveRequestHistoryQuery();
            query.RequestId=id;
            query.PageNumber = pageNumber;
            query.PageSize=pageSize;
            return await _mediator.Send(query,cancellationToken); 
        }
    }
}
