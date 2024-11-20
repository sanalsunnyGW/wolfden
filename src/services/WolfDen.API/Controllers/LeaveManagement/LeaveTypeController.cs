using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Application.Requests.Commands.LeaveManagement.LeaveTypes.AddLeaveType;
using WolfDen.Application.Requests.Commands.LeaveManagement.LeaveTypes.UpdateLeaveType;
using WolfDen.Application.Requests.Queries.LeaveManagement.LeaveTypes.WolfDen.Application.Requests.Queries.LeaveManagement.LeaveTypes;

namespace WolfDen.API.Controllers.LeaveManagement
{
    [Route("api/leave-type")]
    [ApiController]
    public class LeaveTypeController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<bool> AddleaveType([FromBody] AddLeaveTypeCommand command,CancellationToken cancellationToken)
        {
            return await _mediator.Send(command,cancellationToken);
        }

        [HttpPut]
        public async Task<bool> UpdateLeaveType(UpdateLeaveTypeCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [HttpGet]

        public async Task<List<LeaveTypeDto>> GetLeaveTypeIdAndName(CancellationToken cancellationToken)
        {
            GetAllLeaveTypeIdAndNameQuery getAllLeaveTypeIdAndNameQuery = new GetAllLeaveTypeIdAndNameQuery();
            return await _mediator.Send(getAllLeaveTypeIdAndNameQuery, cancellationToken);
        }
    }
}
