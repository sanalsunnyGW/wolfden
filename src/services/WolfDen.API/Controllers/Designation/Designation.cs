using MediatR;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.Requests.Commands.Designations;

namespace WolfDen.API.Controllers.Designation
{
    [Route("api/[controller]")]
    [ApiController]
    public class Designation : ControllerBase
    {
        private readonly IMediator _mediator;

        public Designation(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<int> AddDesignation([FromBody] AddDesignationCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
