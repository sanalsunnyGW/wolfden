using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using WolfDen.Application.Requests.Commands.Attendence.StatusUpdation;
using WolfDen.Application.Requests.Queries.Attendence.DailyStatus;


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

        [HttpPost("{employeeId}/{date}")]
        public async Task<IActionResult> UpdateStatus(int employeeId, DateOnly date)
        {
            StatusUpdateCommand StatusUpdateCommand = new StatusUpdateCommand();
            StatusUpdateCommand.EmployeeId = employeeId;
            StatusUpdateCommand.Date = date;
            var Status = await _mediator.Send(StatusUpdateCommand);
            return Ok(Status);
        }

        [HttpGet("{employeeId}/{date}/attendence-log")]
        public async Task<IActionResult> GetAttendenceLog(int employeeId, DateOnly date)
        {

            DailyDetails Status = new DailyDetails();
            Status.EmployeeId = employeeId;
            Status.Date = date;
            var StatusRecord = await _mediator.Send(Status);
            if (StatusRecord == null)
                return NotFound("No Attendence Log found");
            return Ok(StatusRecord);
        }



        //[HttpGet("{employeeId}/{date}/downloadPdf")]
        //public async Task<IResult> GeneratePdf(int employeeId, DateOnly date)
        //{

        //    DailyDetails Status = new DailyDetails();
        //    Status.EmployeeId = employeeId;
        //    Status.Date = date;
        //    var AttendenceList = await _mediator.Send(Status);
        //    var document = _pdfService.CreateDocument(AttendenceList);


        //    var pdf = document.GeneratePdf();
        //    return Results.File(pdf, "application/pdf", "customer-report.pdf");
        //}






    }
}
