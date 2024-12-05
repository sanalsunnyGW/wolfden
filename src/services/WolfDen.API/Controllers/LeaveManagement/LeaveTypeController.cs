using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Application.Requests.Commands.LeaveManagement.LeaveTypes.AddLeaveType;
using WolfDen.Application.Requests.Commands.LeaveManagement.LeaveTypes.UpdateLeaveType;
using WolfDen.Application.Requests.Queries.LeaveManagement.LeaveTypes;
using WolfDen.Application.Requests.Queries.LeaveManagement.LeaveTypes.NewFolder;

namespace WolfDen.API.Controllers.LeaveManagement
{
    [Route("api/leave-type")]
    [ApiController]
    public class LeaveTypeController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<bool> AddleaveType([FromBody] AddLeaveTypeCommand command,CancellationToken cancellationToken)
        {
            return await _mediator.Send(command,cancellationToken);
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut]
        public async Task<bool> UpdateLeaveType(UpdateLeaveTypeCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [HttpGet]

        public async Task<List<LeaveTypeDto>> GetLeaveTypeIdAndName([FromQuery]GetAllLeaveTypeIdAndNameQuery query,CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }

        [HttpGet("leave-type-details")]
        public async Task<LeaveTypeDto> GetLeaveDetails([FromQuery] GetLeaveTypeQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }
    }
}
