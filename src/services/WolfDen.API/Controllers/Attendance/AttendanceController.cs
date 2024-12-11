﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Application.Requests.Commands.Attendence.CloseAttendance;
using WolfDen.Application.Requests.Commands.Attendence.Service;
using WolfDen.Application.Requests.DTOs.Attendence;
using WolfDen.Application.Requests.Queries.Attendence.AllEmployeesMonthlyReport;
using WolfDen.Application.Requests.Queries.Attendence.AttendanceHistory;
using WolfDen.Application.Requests.Queries.Attendence.AttendanceSummary;
using WolfDen.Application.Requests.Queries.Attendence.CheckAttendanceClose;
using WolfDen.Application.Requests.Queries.Attendence.DailyDetails;
using WolfDen.Application.Requests.Queries.Attendence.DailyStatus;
using WolfDen.Application.Requests.Queries.Attendence.getRange;
using WolfDen.Application.Requests.Queries.Attendence.MonthlyAttendanceReport;
using WolfDen.Application.Requests.Queries.Attendence.MonthlyReport;
using WolfDen.Application.Requests.Queries.Attendence.SubOrdinates;
using WolfDen.Application.Requests.Queries.Attendence.WeeklySummary;


namespace WolfDen.API.Controllers.Attendence
{
    [Route("api/attendance")]
    [ApiController]
    public class Attendance : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly PdfService _pdfService;
        private readonly AttendanceClosedReportPdfService _attendanceReport;
        private readonly MonthlyPdf _monthlyPdf;
        public Attendance(IMediator mediator,PdfService pdfService, MonthlyPdf monthlyPdf,AttendanceClosedReportPdfService attendanceClosedReport)
        {
            _mediator = mediator;
            _pdfService = pdfService;
            _monthlyPdf = monthlyPdf;
            _attendanceReport = attendanceClosedReport;
        }

        [HttpGet("daily-attendance")]
        public async Task<IActionResult> GetAttendenceLog([FromQuery] DailyDetailsQuery DailyDetails, CancellationToken cancellationToken)
        {
            DailyAttendanceDTO attendance = await _mediator.Send(DailyDetails, cancellationToken);
            return Ok(attendance);
        }

        [HttpGet("get-range")]
        public async Task<IActionResult> GetRange([FromQuery]getRangeQuery getRange, CancellationToken cancellationToken)
        {
            RangeDTO range = await _mediator.Send(getRange, cancellationToken);
            return Ok(range);
        }

        [HttpGet("daily-attendance-pdf")]
        public async Task<IResult> GeneratePdf([FromQuery]DailyDetailsQuery DailyDetails, CancellationToken cancellationToken)
        {
            DailyAttendanceDTO attendenceList = await _mediator.Send(DailyDetails, cancellationToken);
            IDocument document = _pdfService.CreateDocument(attendenceList);
            byte[] pdf = document.GeneratePdf();
            return Results.File(pdf, "application/pdf", "DailyReport.pdf");
        }

        [HttpGet("employee/monthly")]
        public async Task<AttendanceSummaryDTO> GetMonthlyAttendance([FromQuery] AttendanceSummaryQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }

        [HttpGet("employee/daily")]
        public async Task<List<DailyStatusDTO>> GetDailyStatus([FromQuery] DailyStatusQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }

        [HttpGet("employee/weekly")]
        public async Task<List<WeeklySummaryDTO>> GetWeeklySummary([FromQuery] WeeklySummaryQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }
        
        [HttpGet("monthly-report")]
        public async Task<IActionResult> GenerateMonthlyReport([FromQuery] MonthlyReportQuery MonthlyReportQuery, CancellationToken cancellationToken)
        {
            MonthlyReportDTO monthlyReport = await _mediator.Send(MonthlyReportQuery, cancellationToken);
            return Ok(monthlyReport);
        }

        [HttpPost("close-attendance")]
        public async Task<IActionResult> CloseAttendance([FromQuery] CloseAttendanceCommand closeAttendanceCommand, CancellationToken cancellationToken)
        {
            int closeAttendance = await _mediator.Send(closeAttendanceCommand, cancellationToken);
            return Ok(closeAttendance);
        }

        [HttpGet("monthly-pdf")]
        public async Task<IResult> GenerateMonthlyPdf([FromQuery] MonthlyReportQuery monthlyPdf, CancellationToken cancellationToken)
        {
            MonthlyReportDTO attendence = await _mediator.Send(monthlyPdf, cancellationToken);
            IDocument document = _monthlyPdf.CreateMonthlyReportDocument(attendence);
            byte[] pdf = document.GeneratePdf();
            return Results.File(pdf, "application/pdf", "MonthlyReport.pdf");
        }

        [HttpGet("closed-attendance-pdf")]
        public async Task<IResult> GenerateAttendanceReportPdf([FromQuery] AllEmployeesMonthlyReportQuery report, CancellationToken cancellationToken)
        {
            MonthlyReportAndPageCountDTO reportAndPageCount = await _mediator.Send(report, cancellationToken);
            List<AllEmployeesMonthlyReportDTO> closedReport = reportAndPageCount.AllEmployeesMonthlyReports;
            IDocument document = _attendanceReport.CreateDocument(closedReport);
            byte[] pdf = document.GeneratePdf();
            return Results.File(pdf, "application/pdf", "ClosedReport.pdf");
        }

        [HttpGet("all-employees-monthly-report")]
        public async Task<IActionResult> AllEmployeeMonthlyReport([FromQuery] AllEmployeesMonthlyReportQuery employeesMonthlyReportQuery, CancellationToken cancellationToken)
        {
            MonthlyReportAndPageCountDTO monthlyReport = await _mediator.Send(employeesMonthlyReportQuery, cancellationToken);
            return Ok(monthlyReport);
        }

        [HttpGet("check-attendance-close")]
        public async Task<IActionResult> CheckAttendanceClose([FromQuery] CheckAttendanceClosedQuery checkAttendanceClosedQuery, CancellationToken cancellationToken)
        {
            CheckAttendanceClosedDTO isClosed = await _mediator.Send(checkAttendanceClosedQuery, cancellationToken);
            return Ok(isClosed);
        }

        [HttpGet("subordinates")]
        public async Task<IActionResult> getSubOrdinates([FromQuery] SubOrdinatesQuery subOrdinatesQuery, CancellationToken cancellationToken)
        {
            SubOrdinateDTO subOrdinates = await _mediator.Send(subOrdinatesQuery, cancellationToken);
            return Ok(subOrdinates);
        }

        [HttpGet("employee/history")]
        public async Task<AttendanceHistoryDTO> GetHistory([FromQuery] AttendanceHistoryQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }

    }


}
       
 
