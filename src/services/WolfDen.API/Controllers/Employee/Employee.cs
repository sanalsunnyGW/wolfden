using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.Requests.Commands.Employees;

namespace WolfDen.API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class Employee : ControllerBase
    {
        private readonly IMediator _mediator;

        public Employee(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<int> AddEmployee([FromBody] AddEmployee command)
        {
            return await _mediator.Send(command);
        }
        [HttpPost("EmployeeUpdateEmployee")]
        public async Task<bool> EmployeeUpdateEmployee([FromBody] EmployeeUpdateEmployee command)
        {
            return await _mediator.Send(command);
        }
        [HttpPost("AdminUpdateEmployee")]
        public async Task<bool> AdminUpdateEmployee([FromBody] AdminUpdateEmployee command)
        {
            return await _mediator.Send(command);
        }

    }
}
