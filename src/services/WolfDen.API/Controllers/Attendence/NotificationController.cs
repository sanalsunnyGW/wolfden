using MediatR;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Application.Requests.Commands.Attendence.SendNotification;
using WolfDen.Application.Requests.Commands.Attendence.UpdateNotification;
using WolfDen.Application.Requests.Commands.Employees.EmployeeUpdateEmployee;
using WolfDen.Application.Requests.Queries.Attendence.DailyDetails;
using WolfDen.Application.Requests.Queries.Notifications.GetNotification;

namespace WolfDen.API.Controllers.Attendence
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public NotificationController(IMediator mediator, PdfService pdfService)
        {
            _mediator = mediator;
        }

        [HttpGet("employee")]
        public async Task<List<NotificationDTO>> GetMonthlyAttendance([FromQuery] GetNotificationQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }

        [HttpPatch("read")]
        public async Task<int> UpdateRead([FromBody] UpdateNotificationReadCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [HttpPost("read")]
        public async Task<int> Add(NotificationCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

    }
}
