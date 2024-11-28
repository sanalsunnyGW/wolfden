using MediatR;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.Requests.Commands.Departments;
using WolfDen.Application.Requests.Queries.Employees.GetAllDepartment;


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

        [HttpGet("all")]
        public async Task<ActionResult<List<DepartmentDTO>>> GetAllDepartment([FromQuery] GetAllDepartmentQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }
    }
}
