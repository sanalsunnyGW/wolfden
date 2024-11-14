using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.Requests.Commands.Departments;

namespace WolfDen.API.Controllers.Department
{
    [Route("api/[controller]")]
    [ApiController]
    public class Department : ControllerBase
    {
        private readonly IMediator _mediator;

        public Department(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<int> AddDepartment([FromBody] AddDepartment command)
        {
            return await _mediator.Send(command);
        }
    }
}
