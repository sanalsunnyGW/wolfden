using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Application.Requests.Commands.LeaveManagement.LeaveSettings.UpdateLeaveSetting;
using WolfDen.Application.Requests.Queries.LeaveManagement.LeaveSettings.GetLeaveSettings;

namespace WolfDen.API.Controllers.LeaveManagement
{
    [Route("api/leave-setting")]
    [ApiController]
    public class LeaveSettingController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<LeaveSettingDto> GetLeaveSetting(CancellationToken cancellationToken)
        {
            GetLeaveSettingQuery getLeaveSettingQuery = new GetLeaveSettingQuery();
            return await _mediator.Send(getLeaveSettingQuery);  
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut]

        public async Task<bool> UpdateLeaveSetting([FromBody] UpdateLeaveSettingCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command);
        }
    }
}
