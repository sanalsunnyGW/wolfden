using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.AttendanceSummary
{
    public class AttendanceSummaryQueryHandler(WolfDenContext context) : IRequestHandler<AttendanceSummaryQuery, AttendanceSummaryDTO>
    {
        private readonly WolfDenContext _context = context;
        public async Task<AttendanceSummaryDTO> Handle(AttendanceSummaryQuery request, CancellationToken cancellationToken)
        {
            int minWorkDuration = 360;

            DateOnly monthStart = new DateOnly(request.Year, request.Month, 1);
            DateOnly monthEnd = monthStart.AddMonths(1).AddDays(-1);

            AttendanceSummaryDTO summaryDto = new AttendanceSummaryDTO
            {
                Present = 0,
                Absent = 0,
                IncompleteShift = 0,
                RestrictedHoliday = 0,
                NormalHoliday = 0,
                WFH = 0,
                Leave = 0
            };

            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);

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

            for (DateOnly currentDate = monthStart; currentDate <= monthEnd; currentDate = currentDate.AddDays(1))
            {
                if (currentDate > today) 
                {
                    break;
                }

                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    continue; 
                }


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
                    }
                }
                else
                {
                    Holiday holiday = holidays.FirstOrDefault(x => x.Date == currentDate);
                    if (holiday is not null)
                    {

                        if (holiday.Type is AttendanceStatus.NormalHoliday)
                        {
                            summaryDto.NormalHoliday++;
                        }

                        else if (holiday.Type is AttendanceStatus.RestrictedHoliday)
                        {
                            LeaveRequest leaveRequestForHoliday = leaveRequests.FirstOrDefault(x => x.FromDate <= currentDate && x.ToDate >= currentDate);

                            if (leaveRequestForHoliday is not null)
                            {
                                var leaveType = leaveTypes.FirstOrDefault(x => x.Id == leaveRequestForHoliday.TypeId);

                                if (leaveType is not null && leaveType.LeaveCategoryId is LeaveCategory.RestrictedHoliday)
                                {
                                    summaryDto.RestrictedHoliday++;
                                }
                            }
                        }
                    }

                    else
                    {
                        LeaveRequest leaveRequest = leaveRequests.FirstOrDefault(x => x.FromDate <= currentDate && x.ToDate >= currentDate);
                        if (leaveRequest is not null)
                        {
                            var leaveType = leaveTypes.FirstOrDefault(x => x.Id == leaveRequest.TypeId);
                            if (leaveType is not null && leaveType.LeaveCategoryId is LeaveCategory.WorkFromHome)
                            {
                                summaryDto.WFH++;
                            }
                            else
                            {
                                summaryDto.Leave++;
                            }
                        }
                        else
                        {
                            summaryDto.Absent++;
                        }
                    }
                }
            }
            return summaryDto;
        }
    }
}
