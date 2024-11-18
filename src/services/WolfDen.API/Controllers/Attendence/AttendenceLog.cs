using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using WolfDen.Application.Requests.DTOs.Attendence;
using WolfDen.Application.Requests.Queries.Attendence.AttendanceSummary;
using WolfDen.Application.Requests.Queries.Attendence.AttendenceLog;
using WolfDen.Application.Requests.Queries.Attendence.DailyStatus;
using WolfDen.Application.Requests.Queries.Attendence.WeeklyAttendence;
using WolfDen.Application.Requests.Queries.Attendence.WeeklySummary;


namespace WolfDen.API.Controllers.Attendence
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendenceLog : ControllerBase
    {
        private readonly IMediator _mediator;
       // private readonly PdfService _pdfService;

        public AttendenceLog(IMediator mediator)
        {
            _mediator = mediator;
           // _pdfService = pdfService;
        }

        [HttpGet("employee/{employeeId}/monthly")]
        public async Task<AttendanceSummaryDTO> GetMonthlyAttendance(int employeeId, [FromQuery] int year, [FromQuery] int month)
        {

            AttendanceSummaryQuery query = new AttendanceSummaryQuery();
            query.EmployeeId = employeeId;
            query.Year = year;
            query.Month = month;
            return await _mediator.Send(query);
        }

        [HttpGet("employee/{employeeId}/dailystatus")]
        public async Task<List<DailyStatusDTO>> GetDailyStatus(int employeeId, [FromQuery] int year, [FromQuery] int month)
        {

            DailyStatusQuery query = new DailyStatusQuery();
            query.EmployeeId = employeeId;
            query.Year = year;
            query.Month = month;
            return await _mediator.Send(query);
        }

        [HttpGet("employee/{employeeId}/weekly")]
        public async Task<List<WeeklySummaryDTO>> GetWeeklySummary(int employeeId, [FromQuery] DateOnly weekstart, [FromQuery] DateOnly weekend)
        {

            WeeklySummaryQuery query = new WeeklySummaryQuery();
            query.EmployeeId = employeeId;
            query.WeekStart = weekstart;
            query.WeekEnd = weekend;
            return await _mediator.Send(query);
        }




        //[HttpGet("{employeeId}/{date}")]
        //public async Task<IActionResult> GetAttendenceLog(int employeeId, DateOnly date)
        //{
        //    AttendenceLogQuery AttendenceLog = new AttendenceLogQuery();
        //    AttendenceLog.EmployeeId = employeeId;
        //    AttendenceLog.Date = date;
        //    var AttendenceList = await _mediator.Send(AttendenceLog);

        //    if (AttendenceList == null)
        //        return NotFound("No Attendence Log found");
        //    return Ok(AttendenceList);
        //}

        //[HttpGet("{employeeId}/{date}/downloadPdf")]
        //public async Task<IResult> GeneratePdf(int employeeId, DateOnly date)
        //{

        //    AttendenceLogQuery AttendenceLog = new AttendenceLogQuery();
        //    AttendenceLog.EmployeeId = employeeId;
        //    AttendenceLog.Date = date;
        //    var AttendenceList = await _mediator.Send(AttendenceLog);
        //    var document = _pdfService.CreateDocument(AttendenceList);


        //    var pdf = document.GeneratePdf();
        //    return Results.File(pdf, "application/pdf", "customer-report.pdf");
        //}


        //[HttpGet("{employeeId}/{week}"]
        //public async Task<IActionResult> GetWeeklyAttendence(int employeeId, string week)
        //{
        //    AttendenceLogQuery AttendenceLog = new AttendenceLogQuery();
        //    AttendenceLog.EmployeeId = employeeId;
        //    AttendenceLog.Date = date;
        //    var AttendenceList = await _mediator.Send(AttendenceLog);

        //    if (AttendenceList == null)
        //        return NotFound("No Attendence Log found");
        //    return Ok(AttendenceList);
        //}



    }
}
