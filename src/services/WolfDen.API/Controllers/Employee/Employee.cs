using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs;
using WolfDen.Application.Requests.Commands.Employees.AddEmployee;
using WolfDen.Application.Requests.Commands.Employees.AdminUpdateEmployee;
using WolfDen.Application.Requests.Commands.Employees.EmployeeUpdateEmployee;
using WolfDen.Application.Requests.Queries.Employee.GetEmployeeHierarchy;
using WolfDen.Application.Requests.Queries.Employees.EmployeeDirectory;
using WolfDen.Application.Requests.Queries.Employees.ViewEmployee;

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

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<EmployeeDTO>> Get(int employeeId)
        {
            return await _mediator.Send(new GetEmployeeQuery(employeeId));
          
        }
        [HttpGet("all")]
        public async Task<ActionResult<List<EmployeeDirectoryDTO>>> GetAllEmployees([FromQuery] int? departmentId, [FromQuery] string? employeeName)
        {
            return await _mediator.Send(new GetAllEmployeeQuery(departmentId, employeeName));

        }

    }
}
