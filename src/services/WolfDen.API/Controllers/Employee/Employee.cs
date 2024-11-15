using MediatR;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.Requests.Commands.Employees.AddEmployee;
using WolfDen.Application.Requests.Commands.Employees.AdminUpdateEmployee;
using WolfDen.Application.Requests.Commands.Employees.EmployeeUpdateEmployee;
using WolfDen.Application.Requests.Queries.Employees.GetEmployeeHierarchy;
using WolfDen.Application.Requests.Queries.Employees.GetEmployeeIdSignUp;
using WolfDen.Application.Requests.Queries.Employees.GetEmployeeTeam;

namespace WolfDen.API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class Employee(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;


        [HttpPost]
        public async Task<int> AddEmployee([FromBody] AddEmployeecommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
        [HttpPut("EmployeeUpdateEmployee")]
        public async Task<bool> EmployeeUpdateEmployee([FromBody] EmployeeUpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
        [HttpPut("Admin")]
        public async Task<bool> AdminUpdateEmployee([FromBody] AdminUpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
        [HttpGet("Hierarchy")]
        public async Task<EmployeeHierarchyDto> GetEmployeeHierarchy([FromQuery] GetEmployeeHierarchyQuery query, CancellationToken cancellationToken)
        {

            return await _mediator.Send(query, cancellationToken);

        }
        [HttpGet("Sign-Up")]
        public async Task<EmployeeSignUpDto> GetEmployeeSignUp([FromQuery] GetEmployeeIDSignUpQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }
        [HttpGet("Team")]
        public async Task<List<EmployeeHierarchyDto>> GetMyTeam([FromQuery] GetEmployeeTeamQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }
    }
}
