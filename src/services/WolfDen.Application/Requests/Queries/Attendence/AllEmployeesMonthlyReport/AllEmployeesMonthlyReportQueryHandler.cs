﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.AllEmployeesMonthlyReport
{
    public class AllEmployeesMonthlyReportQueryHandler : IRequestHandler<AllEmployeesMonthlyReportQuery, MonthlyReportAndPageCountDTO>
    {
        private readonly WolfDenContext _context;
        public AllEmployeesMonthlyReportQueryHandler(WolfDenContext context)
        {
            _context = context;
        }
        public async Task<MonthlyReportAndPageCountDTO> Handle(AllEmployeesMonthlyReportQuery request, CancellationToken cancellationToken)
        {
            MonthlyReportAndPageCountDTO monthlyReportAndPageCountDTO = new MonthlyReportAndPageCountDTO();
            List<AllEmployeesMonthlyReportDTO> allEmployeesReport = new List<AllEmployeesMonthlyReportDTO>();

            DateTime startDate = DateTime.Parse(request.PreviousClosedDate);
            DateTime endDate = DateTime.Parse(request.ClosedDate);
            DateOnly rangeStart = DateOnly.FromDateTime(startDate);
            DateOnly rangeEnd = DateOnly.FromDateTime(endDate);
            List<LOP> LOPReport = await _context.LOP.
                Where(x => rangeStart <= x.AttendanceClosedDate && x.AttendanceClosedDate <= rangeEnd)
                .Include(x => x.Employee).ToListAsync(cancellationToken);
            foreach (LOP lop in LOPReport)
            {
                AllEmployeesMonthlyReportDTO report = new AllEmployeesMonthlyReportDTO();
                report.EmployeeId = lop.EmployeeId;
                report.EmployeeName = lop.Employee.FirstName + " " + lop.Employee.LastName;
                report.IncompleteShiftDays = lop.IncompleteShiftDays;
                report.NofIncompleteShiftDays = lop.NoOfIncompleteShiftDays;
                report.AbsentDays = lop.LOPDays;
                report.NoOfAbsentDays = lop.LOPDaysCount;
                report.HalfDays = lop.HalfDays;
                report.HalfDayLeaves = lop.HalfDayLeaves;
                allEmployeesReport.Add(report);
            }
            int totalPage = allEmployeesReport.Count();
            List<AllEmployeesMonthlyReportDTO> allEmployeesReports = allEmployeesReport.
                Skip((request.PageNumber) * request.PageSize).
                Take(request.PageSize).ToList();
            monthlyReportAndPageCountDTO.AllEmployeesMonthlyReports = allEmployeesReports;
            monthlyReportAndPageCountDTO.PageCount = totalPage;
            return monthlyReportAndPageCountDTO;
        }
    }
}