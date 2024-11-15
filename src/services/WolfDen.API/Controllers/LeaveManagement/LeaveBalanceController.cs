using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.Requests.DTOs.LeaveManagement;
using WolfDen.Application.Requests.Queries.LeaveManagementQuery.LeaveBalanceQuery;

namespace WolfDen.API.Controllers.LeaveManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveBalanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveBalanceController(  IMediator mediator)
        {
            _mediator= mediator;
        }

        [HttpGet]
        public async Task<List<object>> GetLeaveBalances(int RequestId)
        {
            GetLeaveBalanceQuery query= new GetLeaveBalanceQuery();
            query.RequestId = RequestId;    
            return await _mediator.Send(query);
        }
    }
}
