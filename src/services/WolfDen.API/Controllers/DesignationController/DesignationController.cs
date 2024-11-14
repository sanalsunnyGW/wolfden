using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.Requests.Commands.DesignationCommands;

namespace WolfDen.API.Controllers.DesignationController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DesignationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<int> AddDesignation([FromBody]AddDesignationCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
