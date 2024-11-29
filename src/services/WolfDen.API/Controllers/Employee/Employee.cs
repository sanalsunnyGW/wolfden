using MediatR;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.Requests.Commands.Employees.AddEmployee;
using WolfDen.Application.Requests.Commands.Employees.AdminUpdateEmployee;
using WolfDen.Application.Requests.Commands.Employees.EmployeeUpdateEmployee;
using WolfDen.Application.Requests.Queries.Employees.EmployeeDirectory;
using WolfDen.Application.Requests.Queries.Employees.GetEmployeeHierarchy;
using WolfDen.Application.Requests.Queries.Employees.GetEmployeeIdSignUp;
using WolfDen.Application.Requests.Queries.Employees.GetEmployeeTeam;
using WolfDen.Application.Requests.Queries.Employees.EmployeeLogin;
using WolfDen.Application.Requests.Queries.Employees.ViewEmployee;
using Microsoft.AspNetCore.Authorization;
using WolfDen.Infrastructure.Data;
using WolfDen.Application.Requests.Commands.Employees.SuperAdminUpdateEmployee;
using WolfDen.Application.Requests.Queries.Employees.GetAllEmployeesName;
using WolfDen.Application.Requests.Commands.Employees.SyncEmployee;

namespace WolfDen.API.Controllers.Employee
{

    [Route("api/[controller]")]
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
        /* public async Task<IActionResult> AddEmployee([FromBody] AddEmployeecommand command, CancellationToken cancellationToken)
         {
             try
             {
                 int employeeId = await _mediator.Send(command, cancellationToken);

                 return CreatedAtAction(nameof(AddEmployee), new { id = employeeId }, employeeId);
             }
             catch (ValidationException ex)
             {
                 var validationErrors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

                 var result = new ObjectResult(new
                 {
                     Message = "Validation failed",
                     Errors = validationErrors
                 })
                 {
                     StatusCode = StatusCodes.Status422UnprocessableEntity 
                 };
                 return result;
    }
        }*/



        /*     public async Task<IActionResult> AddEmployee([FromBody] AddEmployeecommand command, CancellationToken cancellationToken)
             {
                 try
                 {
                     int employeeId = await _mediator.Send(command, cancellationToken);

                     return CreatedAtAction(nameof(AddEmployee), new { id = employeeId }, employeeId);
                 }
                 catch (ValidationException ex)
                 {
                     var validationErrors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                     return UnprocessableEntity(new
                     {
                         Message = "Validation failed",
                         Errors = validationErrors
                     });
                 }
             }*/



        [HttpPut("employee-update-employee")]
        public async Task<bool> EmployeeUpdateEmployee([FromBody] EmployeeUpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("admin")]
        public async Task<bool> AdminUpdateEmployee([FromBody] AdminUpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
        //[Authorize(Roles = "SuperAdmin")]
        [HttpPut("super-admin")]
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

    }
}
