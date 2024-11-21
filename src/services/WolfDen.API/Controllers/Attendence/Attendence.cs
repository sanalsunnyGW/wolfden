using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Application.Requests.DTOs.Attendence;
using WolfDen.Application.Requests.Queries.Attendence.DailyAttendanceReport;
using WolfDen.Application.Requests.Queries.Attendence.DailyStatus;
using WolfDen.Application.Requests.Queries.Attendence.MonthlyAttendanceReport;

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
        public async Task<IActionResult> GetAttendenceLog([FromQuery]DailyDetails DailyDetails, CancellationToken cancellationToken)
        {
            DailyAttendanceDTO attendance = await _mediator.Send(DailyDetails, cancellationToken);
            return Ok(attendance);
        }

        [HttpGet("daily-attendance-pdf")]
        public async Task<IResult> GeneratePdf([FromQuery]DailyDetailsPdf DailyDetailspdf, CancellationToken cancellationToken)
        {
            DailyAttendanceDTO attendenceList = await _mediator.Send(DailyDetailspdf, cancellationToken);
            var document = _pdfService.CreateDocument(attendenceList);
            var pdf = document.GeneratePdf();
            return Results.File(pdf, "application/pdf", "DailyReport.pdf");
        }

        [HttpGet("monthly-report")]
        public async Task<IActionResult> GenerateMonthlyReport([FromQuery]MonthlyReportQuery MonthlyReportQuery, CancellationToken cancellationToken)
        {
            MonthlyReportDTO monthlyReport= await _mediator.Send(MonthlyReportQuery, cancellationToken);
            return Ok(monthlyReport);
        }

    }
}
