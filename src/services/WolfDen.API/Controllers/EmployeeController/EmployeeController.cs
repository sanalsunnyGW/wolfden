using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.Requests.Commands.EmployeeCommands;

namespace WolfDen.API.Controllers.EmployeeController
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<int>AddEmployee([FromBody]AddEmployeeCommand command)
        {
            return await _mediator.Send(command);
        }
        [HttpPost("EmployeeUpdateEmployee")]
        public async Task<bool> EmployeeUpdateEmployee([FromBody]EmployeeUpdateEmployeeCommand command)
        {
            return await _mediator.Send(command);
        }
        [HttpPost("AdminUpdateEmployee")]
        public async Task<bool> AdminUpdateEmployee([FromBody]AdminUpdateEmployeeCommand command)
        { 
            return await _mediator.Send(command);
        }

    }
}
