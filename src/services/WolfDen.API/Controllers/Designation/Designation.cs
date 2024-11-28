using MediatR;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.Requests.Commands.Designations;
using WolfDen.Application.Requests.Queries.Designation;
using WolfDen.Application.Requests.Queries.Employees.GetAllDepartment;

namespace WolfDen.API.Controllers.Designation
{
    [Route("api/[controller]")]
    [ApiController]
    public class Designation(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator= mediator;

     
        [HttpPost]
        public async Task<int> AddDesignation([FromBody] AddDesignationCommand command,CancellationToken cancellationToken)
        {
            return await _mediator.Send(command,cancellationToken);
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<DesignationDTO>>> GetAllDepartment([FromQuery] GetAllDesignationQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }
    }
}
