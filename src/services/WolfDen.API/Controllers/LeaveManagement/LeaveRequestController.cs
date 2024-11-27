using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Application.Requests.Commands.LeaveManagement.AddLeaveRequestForEmployeeByAdmin;
using WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.AddLeaveRequest;
using WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.ApproveOrRejectLeaveRequest;
using WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.EditLeaveRequest;
using WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.RevokeLeaveRequest;
using WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetLeaveRequestHistory;
using WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetSubordinateLeave;
using WolfDen.Domain.Enums;

namespace WolfDen.API.Controllers.LeaveManagement
{
    [Route("api/leave-request")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveRequestController(IMediator mediator)
        {
            _mediator=mediator;
        }

        [HttpGet("{id}")]
        public async Task<List<LeaveRequestDto>> GetLeaveRequestHistory(int id, CancellationToken cancellationToken)
        {
            GetLeaveRequestHistoryQuery query= new GetLeaveRequestHistoryQuery();
            query.RequestId=id;
            return await _mediator.Send(query, cancellationToken); 
        }

        [HttpPost]
        public async Task<bool> ApplyLeaveRequest( [FromBody] AddLeaveRequestCommand command,CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [HttpPatch("revove-leave")]
        public async Task<bool> RevokeLeave([FromBody] RevokeLeaveRequestCommand command,CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [HttpGet("subordinate-leave-requets{id}/{status}")]
        public async Task<List<SubordinateLeaveDto>> GetSubordinatesLeaveRequest(int id, LeaveRequestStatus status,CancellationToken cancellationToken)
        {
            GetSubordinateLeaveQuery query= new GetSubordinateLeaveQuery();
            query.Id =id;
            query.StatusId = status;

            return await _mediator.Send(query,cancellationToken);
        }

        [HttpPatch("subordinate-leave-requets/{id}")]
        public async Task<bool> ApproveOrRejectLeave(int id , [FromBody] ApproveOrRejectLeaveRequestCommand command,CancellationToken cancellationToken)
        {
            command.SuperiorId = id;   
            return await _mediator.Send(command,cancellationToken);
        }

        [HttpPatch("edit-leave/{id}")]
        public async Task<bool> EditLeave(int id, [FromBody] EditLeaveRequestCommand command,CancellationToken cancellationToken)
        {
            command.EmpId = id;
            return await _mediator.Send(command,cancellationToken) ;
        }

        [HttpPost("leave-for-employee-by-admin")]

        public async Task<bool> AddLeaveForSubordinates([FromBody] AddLeaveRequestForEmployeeByAdmin command,CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
    }
}
