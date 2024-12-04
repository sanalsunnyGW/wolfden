using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.Requests.Commands.Departments;
using WolfDen.Application.Requests.Queries.Department.GetAllDepartment;

namespace WolfDen.API.Controllers.Department
{
    [Route("api/department")]
    [ApiController]
    public class Department(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator=mediator;

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<int> AddDepartment([FromBody] AddDepartmentCommand command,CancellationToken cancellationToken)
        {
            return await _mediator.Send(command,cancellationToken);
        }

        [HttpGet]
        public async Task<ActionResult<List<DepartmentDTO>>> GetAllDepartment([FromQuery] GetAllDepartmentQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }
    }
}
