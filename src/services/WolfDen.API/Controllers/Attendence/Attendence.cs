using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using WolfDen.Application.Requests.Queries.Attendence.DailyAttendanceReport;
using WolfDen.Application.Requests.Queries.Attendence.DailyStatus;

namespace WolfDen.API.Controllers.Attendence
{
    [Route("api/attendance")]
    [ApiController]
    public class Attendance : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly PdfService _pdfService;
        public Attendance(IMediator mediator,PdfService pdfService)
        {
            _mediator = mediator;
            _pdfService = pdfService;  
        }
        
        [HttpGet("daily-attendance")]
        public async Task<IActionResult> GetAttendenceLog([FromQuery]DailyDetails attendanceRecord,CancellationToken cancellationToken)
        {
            var attendance = await _mediator.Send(attendanceRecord, cancellationToken);
            if (attendance is null)
                return NotFound("No Attendence Log found");
            return Ok(attendance);
        }

        [HttpGet("daily-attendance-pdf")]
        public async Task<IResult> GeneratePdf([FromQuery]DailyDetailsPdf DailyDetailspdf, CancellationToken cancellationToken)
        {
            var attendenceList = await _mediator.Send(DailyDetailspdf, cancellationToken);
            var document = _pdfService.CreateDocument(attendenceList);
            var pdf = document.GeneratePdf();
            return Results.File(pdf, "application/pdf", "DailyReport.pdf");
        }
    }
}
