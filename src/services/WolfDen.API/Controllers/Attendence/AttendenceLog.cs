using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using WolfDen.Application.Requests.Commands.Attendence.StatusUpdation;
using WolfDen.Application.Requests.Queries.Attendence.DailyStatus;


namespace WolfDen.API.Controllers.Attendence
{
    [Route("api/attendance")]
    [ApiController]
    public class AttendenceLog : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly PdfService _pdfService;
      
        public AttendenceLog(IMediator mediator,PdfService pdfService)
        {
            _mediator = mediator;
            _pdfService = pdfService;  
        }

        [HttpPost("{employeeId}/status")]
        public async Task<int> UpdateStatus(int employeeId,[FromQuery] DateOnly date,StatusUpdateCommand statusUpdateCommand)
        {
            statusUpdateCommand.EmployeeId = employeeId;
            statusUpdateCommand.Date = date;
            return await _mediator.Send(statusUpdateCommand); 
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetAttendenceLog(int employeeId,[FromQuery] DateOnly date)
        {
            DailyDetails status = new DailyDetails();
            status.EmployeeId = employeeId;
            status.Date = date;
            var statusRecord = await _mediator.Send(status);
            if (statusRecord is null)
                return NotFound("No Attendence Log found");
            return Ok(statusRecord);
        }

        [HttpGet("{employeeId}/downloadPdf")]
        public async Task<IResult> GeneratePdf(int employeeId,[FromQuery] DateOnly date)
        {
            DailyDetails status = new DailyDetails();
            status.EmployeeId = employeeId;
            status.Date = date;
            var attendenceList = await _mediator.Send(status);
            var document = _pdfService.CreateDocument(attendenceList);
            var pdf = document.GeneratePdf();
            return Results.File(pdf, "application/pdf", "DailyReport.pdf");
        }






    }
}
