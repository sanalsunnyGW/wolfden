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
                IncompleteShiftDays = "",
                RestrictedHolidays = "",
                NormalHolidays ="",
                WFHDays = "",
                LeaveDays = "",
                AbsentDays = ""
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

            LOP? lop =await _context.LOP
                .Where(x=>x.EmployeeId==request.EmployeeId && x.AttendanceClosedDate.Month==request.Month)
                .FirstOrDefaultAsync(cancellationToken);
            if (lop is not null)
            {
                summaryDto.IncompleteShiftDays = lop.IncompleteShiftDays;
                summaryDto.IncompleteShift = lop.NoOfIncompleteShiftDays;
                summaryDto.Absent = lop.LOPDaysCount;
                summaryDto.AbsentDays = lop.LOPDays;
            }
            List<LeaveType> leaveTypes = await _context.LeaveType.ToListAsync(cancellationToken);

            for (var currentDate = monthStart; currentDate <= monthEnd; currentDate = currentDate.AddDays(1))
            {
                DailyAttendence? attendanceRecord = attendanceRecords.FirstOrDefault(x => x.Date == currentDate);
                if (attendanceRecord is not null)
                {
                    if (attendanceRecord.InsideDuration >= minWorkDuration)
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
                            summaryDto.NormalHolidays += currentDate.ToString("yyyy-MM-dd") + ",";
                        }
                        else if (holiday.Type is AttendanceStatus.RestrictedHoliday)
                        {
                            LeaveRequest? leaveRequestForHoliday = leaveRequests
                                .FirstOrDefault(x => x.FromDate <= currentDate && x.ToDate >= currentDate);

                            if (leaveRequestForHoliday is not null)
                            {
                                LeaveType? leaveType = leaveTypes.FirstOrDefault(x => x.Id == leaveRequestForHoliday.TypeId);

                                if (leaveType is not null && leaveType.LeaveCategoryId is LeaveCategory.RestrictedHoliday)
                                {
                                    summaryDto.Holiday++;
                                    summaryDto.RestrictedHolidays += currentDate.ToString("yyyy-MM-dd") + ",";
                                }
                            }
                        }
                    }
                    else
                    {
                        LeaveRequest? leaveRequest = leaveRequests
                            .FirstOrDefault(x => x.FromDate <= currentDate && x.ToDate >= currentDate);
                        if (leaveRequest is not null)
                        {
                            LeaveType? leaveType = leaveTypes.FirstOrDefault(x => x.Id == leaveRequest.TypeId);
                            if (leaveType is not null && leaveType.LeaveCategoryId is LeaveCategory.WorkFromHome)
                            {
                                summaryDto.WFH++;
                                summaryDto.WFHDays += currentDate.ToString("yyyy-MM-dd") + ",";
                            }
                            else
                            {
                                summaryDto.Leave++;
                                summaryDto.LeaveDays += currentDate.ToString("yyyy-MM-dd") + ",";
                            }
                        }
                        
                    }
                }
            }
            return summaryDto;
        }
    }
}
                

