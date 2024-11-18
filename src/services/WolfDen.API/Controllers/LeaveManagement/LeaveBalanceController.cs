using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Application.Requests.Queries.LeaveManagement.LeaveBalances.GetLeaveBalance;

namespace WolfDen.API.Controllers.LeaveManagement
{
    [Route("api/leave-balance")]
    [ApiController]
    public class LeaveBalanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveBalanceController(  IMediator mediator)
        {
            _mediator= mediator;
        }

        [HttpGet("{id}")]
        public async Task<List<LeaveBalanceDto>> GetLeaveBalances(int id, CancellationToken cancellationToken)
        {
            GetLeaveBalanceQuery query= new GetLeaveBalanceQuery();
            query.RequestId = id;    
            return await _mediator.Send(query, cancellationToken);
        }
    }
}
