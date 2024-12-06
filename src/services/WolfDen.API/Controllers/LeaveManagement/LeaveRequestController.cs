using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Application.Requests.Commands.LeaveManagement.AddLeaveRequestForEmployeeByAdmin;
using WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.AddLeaveRequest;
using WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.ApproveOrRejectLeaveRequest;
using WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.EditLeaveRequest;
using WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.RevokeLeaveRequest;
using WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetLeaveRequestHistory;
using WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetApprovedNextWeekLeaves;
using WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetSubordinateLeave;
using WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetLeaveRequestById;

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

        [HttpGet("next-week/approved")]
        public async Task<List<LeaveRequestDto>> GetNextWeekApprovedLeaves([FromQuery] GetNextWeekApprovedLeaveQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }
        [HttpPost]
        public async Task<ResponseDto> ApplyLeaveRequest( [FromBody] AddLeaveRequestCommand command,CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [HttpPatch("revoke-leave/{id}")]
        public async Task<bool> RevokeLeave(int id ,[FromBody] RevokeLeaveRequestCommand command,CancellationToken cancellationToken)
        {
            command.EmployeeId = id;
            return await _mediator.Send(command, cancellationToken);
        }

        [HttpGet("subordinate-leave-requests")]
        public async Task<SubordinateLeaveRequestPaginationDto> GetSubordinatesLeaveRequest( [FromQuery] GetSubordinateLeaveQuery query, CancellationToken cancellationToken)
        {

            return await _mediator.Send(query,cancellationToken);
        }

        [HttpPatch("subordinate-leave-requests/{id}")]
        public async Task<bool> ApproveOrRejectLeave(int id , [FromBody] ApproveOrRejectLeaveRequestCommand command,CancellationToken cancellationToken)
        {
            command.SuperiorId = id;   
            return await _mediator.Send(command,cancellationToken);
        }
        

        [HttpPut("edit-leave/{id}")]
        public async Task<ResponseDto> EditLeave(int id ,[FromBody] EditLeaveRequestCommand command,CancellationToken cancellationToken)
        {
            command.EmpId = id;
            return await _mediator.Send(command,cancellationToken) ;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("leave-for-employee-by-admin")]

        public async Task<ResponseDto> AddLeaveForSubordinates([FromBody] AddLeaveRequestForEmployeeByAdmin command,CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [HttpGet("{leaveRequestId}")]
        public async Task<EditLeaveRequestDto> GetLeaveRequestById(int leaveRequestId, CancellationToken cancellationToken)
        {
            GetLeaveRequestByIdQuery query = new GetLeaveRequestByIdQuery
            {
                leaveRequestId = leaveRequestId
            };

            return await _mediator.Send(query,cancellationToken);
        }
    }
}
