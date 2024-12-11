﻿using MediatR;
using WolfDen.Application.DTOs.Attendence;

namespace WolfDen.Application.Requests.Queries.Attendence.AllEmployeesMonthlyReport
{
    public class AllEmployeesMonthlyReportQuery : IRequest<MonthlyReportAndPageCountDTO>
    {
        public string PreviousClosedDate { get; set; }
        public string ClosedDate { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}