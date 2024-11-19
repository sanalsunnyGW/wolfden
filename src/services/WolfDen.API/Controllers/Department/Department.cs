using MediatR;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.Requests.Commands.Departments;


namespace WolfDen.API.Controllers.Department
{
    [Route("api/[controller]")]
    [ApiController]
    public class Department(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator=mediator;

        
        [HttpPost]
        public async Task<int> AddDepartment([FromBody] AddDepartmentCommand command,CancellationToken cancellationToken)
        {
            return await _mediator.Send(command,cancellationToken);
        }
    }
}
