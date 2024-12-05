using MediatR;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.Requests.Commands.Employees.AddSuperAdmin;

namespace WolfDen.API.Controllers.Employee
{
    [Route("api/superadmin")]
    [ApiController]
    public class SuperAdmin(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("add-super-admin")]
        public async Task<int> AddSuperAdmin([FromBody] AddSuperAdminCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
    }
}
