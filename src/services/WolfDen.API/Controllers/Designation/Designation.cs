using MediatR;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.Requests.Commands.Designations;

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
    }
}
