using MediatR;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Application.Requests.Commands.Attendence.AddHoliday;
using WolfDen.Application.Requests.Queries.Attendence.GetHolidays;
using WolfDen.Application.Requests.Queries.Notifications.GetNotification;

namespace WolfDen.API.Controllers.Attendence
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayController : ControllerBase
    {
        private readonly IMediator _mediator;
        public HolidayController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("holiday")]
        public async Task<int> AddHoliday([FromBody] AddHolidayCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [HttpGet("upcoming-holiday")]
        public async Task<List<HolidayDTO>> GetHoliday([FromQuery] GetHolidayQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }
    }
}
