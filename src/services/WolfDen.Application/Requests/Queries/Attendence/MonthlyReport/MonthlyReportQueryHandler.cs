using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.MonthlyAttendanceReport
{
    public class MonthlyReportQueryHandler : IRequestHandler<MonthlyReportQuery,MonthlyReportDTO>
    {
        private readonly WolfDenContext _context;
        public MonthlyReportQueryHandler(WolfDenContext context)
        {
            _context = context;
        }
        public async Task<MonthlyReportDTO> Handle(MonthlyReportQuery request, CancellationToken cancellationToken)
        {
            int minWorkDuration = 360;

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
                IncompleteShiftDays = new List<DateOnly>(),
                RestrictedHolidays= new List<DateOnly>(),
                NormalHolidays= new List<DateOnly>(),
                WFHDays= new List<DateOnly>(),
                LeaveDays= new List<DateOnly>(),
                AbsentDays= new List<DateOnly>()
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
                            x.LeaveRequestStatusId == LeaveRequestStatus.Approved)
                .ToListAsync(cancellationToken);

            List<LeaveType> leaveTypes = await _context.LeaveType.ToListAsync(cancellationToken);

            for (var currentDate = monthStart; currentDate <= monthEnd; currentDate = currentDate.AddDays(1))
            {
                DailyAttendence attendanceRecord = attendanceRecords.FirstOrDefault(x => x.Date == currentDate);
                if (attendanceRecord is not null)
                {
                    if (attendanceRecord.InsideDuration >= minWorkDuration)
                    {
                        summaryDto.Present++;
                    }
                    else
                    {
                        summaryDto.IncompleteShift++;
                        summaryDto.IncompleteShiftDays.Add(currentDate);
                    }
                }
                else
                {
                    var holiday = holidays.FirstOrDefault(x => x.Date == currentDate);
                    if (holiday is not null)
                    {
                        if (holiday.Type == AttendanceStatus.NormalHoliday)
                        {
                            summaryDto.Holiday++;
                            summaryDto.NormalHolidays.Add(currentDate);

                        }
                        else if (holiday.Type == AttendanceStatus.RestrictedHoliday)
                        {
                            LeaveRequest leaveRequestForHoliday = leaveRequests.FirstOrDefault(x => x.FromDate <= currentDate && x.ToDate >= currentDate);

                            if (leaveRequestForHoliday is not null)
                            {
                                var leaveType = leaveTypes.FirstOrDefault(x => x.Id == leaveRequestForHoliday.TypeId);

                                if (leaveType is not null && leaveType.LeaveCategoryId is LeaveCategory.RestrictedHoliday)
                                {
                                    summaryDto.Holiday++;
                                    summaryDto.RestrictedHolidays.Add(currentDate);
                                }
                            }
                        }
                    }
                    else
                    {
                        LeaveRequest leaveRequest = leaveRequests.FirstOrDefault(x => x.FromDate <= currentDate && x.ToDate >= currentDate);
                        if (leaveRequest is not null)
                        {
                            LeaveType leaveType = leaveTypes.FirstOrDefault(x => x.Id == leaveRequest.TypeId);
                            if (leaveType is not null && leaveType.LeaveCategoryId is LeaveCategory.WorkFromHome)
                            {
                                summaryDto.WFH++;
                                summaryDto.WFHDays.Add(currentDate);
                            }
                            else
                            {
                                summaryDto.Leave++;
                                summaryDto.LeaveDays.Add(currentDate);
                            }
                        }
                        else
                        {
                            summaryDto.Absent++;
                            summaryDto.AbsentDays.Add(currentDate);
                        }
                    }
                }
            }
            return summaryDto;
        }
    }
}
                

