using MediatR;
using Microsoft.AspNetCore.Mvc;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Application.Requests.Queries.Attendence.AttendanceSummary;
using WolfDen.Application.Requests.Queries.Attendence.DailyStatus;
using WolfDen.Application.Requests.Queries.Attendence.WeeklySummary;


namespace WolfDen.API.Controllers.Attendence
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendenceLog : ControllerBase
    {
        private readonly IMediator _mediator;


        public AttendenceLog(IMediator mediator)
        {
            _mediator = mediator;
            
        }

        [HttpGet("employee/monthly")]
        public async Task<AttendanceSummaryDTO> GetMonthlyAttendance([FromQuery] AttendanceSummaryQuery query,CancellationToken cancellationToken)
        { 
            return await _mediator.Send(query,cancellationToken);
        }

        [HttpGet("employee/daily-status")]
        public async Task<List<DailyStatusDTO>> GetDailyStatus([FromQuery] DailyStatusQuery query,CancellationToken cancellationToken)
        { 
            return await _mediator.Send(query,cancellationToken);
        }

        [HttpGet("employee/weekly")]
        public async Task<List<WeeklySummaryDTO>> GetWeeklySummary([FromQuery] WeeklySummaryQuery query,CancellationToken cancellationToken)
        {
            return await _mediator.Send(query,cancellationToken);
        }

    }
}
