using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.LeaveManagement;
//using WolfDen.Application.Requests.Commands.LeaveManagement.LeaveBalances.InitializeLeaveBalance;
using WolfDen.Application.Requests.Commands.LeaveManagement.LeaveBalances.UpdateLeaveBalance;
using WolfDen.Application.Requests.Queries.LeaveManagement.LeaveBalances.GetLeaveBalance;

namespace WolfDen.API.Controllers.LeaveManagement
{
    [Route("api/leave-balance")]
    [ApiController]
    public class LeaveBalanceController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<List<LeaveBalanceDto>> GetLeaveBalances([FromQuery] GetLeaveBalanceQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }

       // [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut]
        public async Task<bool> UpdateLeaveBalance(CancellationToken cancellationToken)
        {
            UpdateLeaveBalanceCommand command = new UpdateLeaveBalanceCommand();
            return await _mediator.Send(command, cancellationToken);
        }

        //[HttpPost]
        //public async Task<bool> InitializeLeaveBalance(int requestId, CancellationToken cancellationToken)
        //{
        //    InitializeLeaveBalanceCommand command = new InitializeLeaveBalanceCommand();
        //    command.RequestId = requestId;
        //    return await _mediator.Send(command, cancellationToken);
        //}
    }
}
