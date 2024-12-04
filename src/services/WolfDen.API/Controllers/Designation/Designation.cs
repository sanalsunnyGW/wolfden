using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.Requests.Commands.Designations;
using WolfDen.Application.Requests.Queries.Designation;

namespace WolfDen.API.Controllers.Designation
{
    [Route("api/designation")]
    [ApiController]
    public class Designation(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator= mediator;

        //[Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<int> AddDesignation([FromBody] AddDesignationCommand command,CancellationToken cancellationToken)
        {
            return await _mediator.Send(command,cancellationToken);
        }

        [HttpGet]
        public async Task<ActionResult<List<DesignationDTO>>> GetAllDepartment([FromQuery] GetAllDesignationQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }
    }
}
