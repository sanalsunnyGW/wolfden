using MediatR;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.Requests.Commands.LeaveManagement.LeaveTypes.AddLeaveType;

namespace WolfDen.API.Controllers.LeaveManagement
{
    [Route("api/leave-type")]
    [ApiController]
    public class LeaveTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveTypeController(IMediator mediator)
        {
            _mediator = mediator;

        }
        [HttpPost]
        public async Task<bool> AddLeaveType([FromBody] AddLeaveTypeCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
    }
}
