using MediatR;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs;
using WolfDen.Application.Requests.Commands.Employees.AddEmployee;
using WolfDen.Application.Requests.Commands.Employees.AdminUpdateEmployee;
using WolfDen.Application.Requests.Commands.Employees.EmployeeUpdateEmployee;
using WolfDen.Application.Requests.Queries.Employee.GetEmployeeHierarchy;

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
        public async Task<int> AddEmployee([FromBody] AddEmployeecommand command)
        {
            return await _mediator.Send(command);
        }
        [HttpPut("EmployeeUpdateEmployee")]
        public async Task<bool> EmployeeUpdateEmployee([FromBody] EmployeeUpdateEmployeeCommand command)
        {
            return await _mediator.Send(command);
        }
        [HttpPut("AdminUpdateEmployee")]
        public async Task<bool> AdminUpdateEmployee([FromBody] AdminUpdateEmployeeCommand command)
        {
            return await _mediator.Send(command);
        }
        [HttpGet("Employee Hierarchy")]
        public async Task<EmployeeHierarchyDto> GetEmployeeHierarchy(int id)
        {
            GetEmployeeHierarchyQuery query = new GetEmployeeHierarchyQuery();
            query.Id = id;
            return await _mediator.Send(query);

        }


    }
}
