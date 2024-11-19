using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
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
        
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetAttendenceLog(int employeeId, DailyDetails status)
        {
            status.EmployeeId = employeeId;
            var statusRecord = await _mediator.Send(status);
            if (statusRecord is null)
                return NotFound("No Attendence Log found");
            return Ok(statusRecord);
        }

        [HttpGet("{employeeId}/downloadPdf")]
        public async Task<IResult> GeneratePdf(int employeeId, DailyDetails status)
        {
            status.EmployeeId = employeeId;
            var attendenceList = await _mediator.Send(status);
            var document = _pdfService.CreateDocument(attendenceList);
            var pdf = document.GeneratePdf();
            return Results.File(pdf, "application/pdf", "DailyReport.pdf");
        }
    }
}
