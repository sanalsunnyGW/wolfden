using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Domain.ConfigurationModel;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.MonthlyAttendanceReport
{
    public class MonthlyReportQueryHandler : IRequestHandler<MonthlyReportQuery,MonthlyReportDTO>
    {
        private readonly WolfDenContext _context;
        private readonly IOptions<OfficeDurationSettings> _officeDurationSettings;

        public MonthlyReportQueryHandler(WolfDenContext context,IOptions<OfficeDurationSettings> officeDurationSettings)
        {
            _context = context;
            _officeDurationSettings = officeDurationSettings;
        }
        public async Task<MonthlyReportDTO> Handle(MonthlyReportQuery request, CancellationToken cancellationToken)
        {
            
            int minWorkDuration = _officeDurationSettings.Value.MinWorkDuration;

            DateOnly monthStart = new DateOnly(request.Year, request.Month, 1);
            DateOnly monthEnd = monthStart.AddMonths(1).AddDays(-1);

            MonthlyReportDTO summaryDto = new MonthlyReportDTO
            {
                Present = 0,
                Absent = 0,
                IncompleteShift = 0,
                Holiday = 0,
                WFH = 0,
                Leave = 0,
                HalfDays = 0,
                IncompleteShiftDays = "",
                RestrictedHolidays = "",
                NormalHolidays ="",
                WFHDays = "",
                LeaveDays = "",
                AbsentDays = "",
                HalfDayLeaves=""
            };

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            List<DailyAttendence> attendanceRecords = await _context.DailyAttendence
                .Where(x => x.EmployeeId == request.EmployeeId && x.Date >= monthStart && x.Date <= monthEnd)
                .ToListAsync(cancellationToken);

            List<Holiday> holidays = await _context.Holiday
                .Where(x => x.Date >= monthStart && x.Date <= monthEnd)
                .ToListAsync(cancellationToken);

            List<LeaveRequest> leaveRequests = await _context.LeaveRequests
                .Where(x => x.EmployeeId == request.EmployeeId &&
                            x.FromDate <= monthEnd && x.ToDate >= monthStart &&
                            x.LeaveRequestStatusId == LeaveRequestStatus.Approved).Include(x=>x.LeaveType)
                .ToListAsync(cancellationToken);

            for (var currentDate = monthStart; currentDate <= monthEnd; currentDate = currentDate.AddDays(1))
            {
                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    continue;
                }
                if (currentDate >= today)
                {
                    break;
                }
                DailyAttendence? attendanceRecord = attendanceRecords.FirstOrDefault(x => x.Date == currentDate);
                if (attendanceRecord is not null)
                {
                    LeaveRequest? leaveRequest = leaveRequests
                              .FirstOrDefault(x => x.FromDate <= currentDate && x.ToDate >= currentDate);
                    if (leaveRequest is not null && leaveRequest.HalfDay is true)
                    {
                        minWorkDuration = minWorkDuration / 2;
                        summaryDto.HalfDays++;
                        summaryDto.HalfDayLeaves += currentDate.ToString("yyyy-MM-dd") + ", ";
                    }
                    if (attendanceRecord.InsideDuration < minWorkDuration)
                    {
                        summaryDto.IncompleteShiftDays += currentDate.ToString("yyyy-MM-dd") + ", ";
                        summaryDto.IncompleteShift++;
                    }
                    else
                    {
                        summaryDto.Present++;
                    }
                }
                else
                {
                    Holiday? holiday = holidays.FirstOrDefault(x => x.Date == currentDate);
                    if (holiday is not null)
                    {
                        if (holiday.Type is AttendanceStatus.NormalHoliday)
                        {
                            summaryDto.Holiday++;
                            summaryDto.NormalHolidays += currentDate.ToString("yyyy-MM-dd") + ", ";
                        }
                        else if (holiday.Type is AttendanceStatus.RestrictedHoliday)
                        {
                            LeaveRequest? leaveRequestForHoliday = leaveRequests
                                .FirstOrDefault(x => x.FromDate <= currentDate && x.ToDate >= currentDate);

                            if (leaveRequestForHoliday is not null && leaveRequestForHoliday.LeaveType.LeaveCategoryId is LeaveCategory.RestrictedHoliday)
                            {
                                summaryDto.Holiday++;
                                summaryDto.RestrictedHolidays += currentDate.ToString("yyyy-MM-dd") + ", ";
                            }
                        }
                    }
                    else
                    {
                        LeaveRequest? leaveRequest = leaveRequests
                            .FirstOrDefault(x => x.FromDate <= currentDate && x.ToDate >= currentDate);
                        if (leaveRequest is not null && leaveRequest.LeaveType.LeaveCategoryId is LeaveCategory.WorkFromHome)
                        {
                            summaryDto.WFH++;
                            summaryDto.WFHDays += currentDate.ToString("yyyy-MM-dd") + ", ";
                        }
                        else if(leaveRequest is not null)
                        {
                            summaryDto.Leave++;
                            summaryDto.LeaveDays += currentDate.ToString("yyyy-MM-dd") + ", ";
                           
                        }
                        else
                        {
                            summaryDto.Absent++;
                            summaryDto.AbsentDays += currentDate.ToString("yyyy-MM-dd") + ", ";
                        }
                    }
                }
            }
            summaryDto.AbsentDays = update(summaryDto.AbsentDays);
            summaryDto.IncompleteShiftDays = update(summaryDto.IncompleteShiftDays);
            summaryDto.LeaveDays = update(summaryDto.LeaveDays);
            summaryDto.WFHDays = update(summaryDto.WFHDays);
            summaryDto.HalfDayLeaves = update(summaryDto.HalfDayLeaves);
            summaryDto.RestrictedHolidays = update(summaryDto.RestrictedHolidays);
            return summaryDto;
        }
        private string update(string days)
        {
            string updatedDays = days.Length > 0 ? days.Substring(0, days.Length - 2)  : " ";
            return updatedDays;
        }
    }
}
                

