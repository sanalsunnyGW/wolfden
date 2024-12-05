using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.Requests.Commands.Employees.AddEmployee;
using WolfDen.Application.Requests.Commands.Employees.AdminUpdateEmployee;
using WolfDen.Application.Requests.Commands.Employees.EmployeeUpdateEmployee;
using WolfDen.Application.Requests.Commands.Employees.SuperAdminUpdateEmployee;
using WolfDen.Application.Requests.Commands.Employees.SyncEmployee;
using WolfDen.Application.Requests.Queries.Employees.EmployeeDirectory;
using WolfDen.Application.Requests.Queries.Employees.EmployeeLogin;
using WolfDen.Application.Requests.Queries.Employees.EmployeePasswordCheck;
using WolfDen.Application.Requests.Queries.Employees.GetAllEmployeesName;
using WolfDen.Application.Requests.Queries.Employees.GetEmployeeHierarchy;
using WolfDen.Application.Requests.Queries.Employees.GetEmployeeIdSignUp;
using WolfDen.Application.Requests.Queries.Employees.GetEmployeeTeam;
using WolfDen.Application.Requests.Queries.Employees.ViewEmployee;
using Microsoft.AspNetCore.Authorization;
using WolfDen.Infrastructure.Data;
using WolfDen.Application.Requests.Commands.Employees.SuperAdminUpdateEmployee;
using WolfDen.Application.Requests.Queries.Employees.GetAllEmployeesName;
using WolfDen.Application.Requests.Commands.Employees.SyncEmployee;
using WolfDen.Application.Requests.Queries.Employees.EmployeePasswordCheck;
using WolfDen.Application.Requests.Commands.Employees.ResetPassword;

namespace WolfDen.API.Controllers.Employee
{
    [Authorize]
    [Route("api/employee")]
    [ApiController]
    public class Employee(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;


        [HttpPatch("employee-sync")]
        public async Task<bool> SyncEmployee()
        {
            return await _mediator.Send(new SyncEmployeeCommand());
        }



        [HttpPost]
        public async Task<int> AddEmployee([FromBody] AddEmployeecommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
        [AllowAnonymous]
        [HttpPut("employee-update-employee")]
        public async Task<bool> EmployeeUpdateEmployee([FromBody] EmployeeUpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("admin")]
        public async Task<bool> AdminUpdateEmployee([FromBody] AdminUpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("role")]
        public async Task<bool> SuperAdminUpdateEmployee([FromBody] SuperAdminUpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
        [HttpGet("hierarchy")]
        public async Task<EmployeeHierarchyDto> GetEmployeeHierarchy()
        {
            return await _mediator.Send(new GetEmployeeHierarchyQuery());

        }
        [HttpGet("sign-up")]
        public async Task<EmployeeSignUpDto> GetEmployeeSignUp([FromQuery] GetEmployeeIDSignUpQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }
        [AllowAnonymous]
        [HttpGet("login")]
        public async Task<LoginResponseDTO> EmployeeLogin([FromQuery] EmployeeLoginQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }
        [HttpGet("team")]
        public async Task<List<EmployeeHierarchyDto>> GetMyTeam([FromQuery] GetEmployeeTeamQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }

        [HttpGet("by-Id")]
        public async Task<EmployeeDTO> GetEmployee([FromQuery] GetEmployeeQuery query, CancellationToken cancellationToken)
        {

            return await _mediator.Send(query, cancellationToken);

        }

        [HttpGet("all")]
        public async Task<ActionResult<PaginationResponse>> GetAllEmployees([FromQuery] GetAllEmployeeQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);

        }

        [HttpGet("get-all-by-name")]
        public async Task<ActionResult<List<EmployeeNameDTO>>> GetAllEmployees([FromQuery] GetAllEmployeesByNameQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);


        }
        [HttpGet("check-password")]
        public async Task<ActionResult<bool>> CheckPassword([FromQuery] CheckEmployeePasswordQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);

        }
        [AllowAnonymous]
        [HttpPatch("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
