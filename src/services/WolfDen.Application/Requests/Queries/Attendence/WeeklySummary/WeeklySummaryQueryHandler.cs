using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.WeeklySummary
{
    public class WeeklySummaryQueryHandler(WolfDenContext context) : IRequestHandler<WeeklySummaryQuery, List<WeeklySummaryDTO>>
    {
        private readonly WolfDenContext _context = context;
        public async Task<List<WeeklySummaryDTO>> Handle(WeeklySummaryQuery request, CancellationToken cancellationToken)
        {
            int minWorkDuration = 360;
            List<WeeklySummaryDTO> weeklySummary = new List<WeeklySummaryDTO>();
            DateTime startDate = DateTime.Parse(request.WeekStart);
            DateTime endDate = DateTime.Parse(request.WeekEnd);
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            DateOnly weekStart = DateOnly.FromDateTime(startDate);
            DateOnly weekEnd = DateOnly.FromDateTime(endDate);
            List<DailyAttendence> attendanceRecords = await _context.DailyAttendence
                .Where(x => x.EmployeeId == request.EmployeeId && x.Date >= weekStart && x.Date <= weekEnd)
                .ToListAsync(cancellationToken);
            List<Holiday> holidays = await _context.Holiday
                .Where(x => x.Date >= weekStart && x.Date <= weekEnd)
                .ToListAsync(cancellationToken);
            List<LeaveRequest> leaveRequests = await _context.LeaveRequests
                .Where(x => x.EmployeeId == request.EmployeeId &&
                            x.FromDate <= weekEnd && x.ToDate >= weekStart &&
                            x.LeaveRequestStatusId == LeaveRequestStatus.Approved)
                .ToListAsync(cancellationToken);
            List<LeaveType> leaveTypes = await _context.LeaveType.ToListAsync(cancellationToken);
            for (DateOnly currentDate = weekStart; currentDate <= weekEnd; currentDate = currentDate.AddDays(1))
            {
                if (currentDate > today)
                {
                    break;
                }
                AttendanceStatus statusId = AttendanceStatus.Absent;
                DailyAttendence attendanceRecord = attendanceRecords.FirstOrDefault(x => x.Date == currentDate);
                if (attendanceRecord is not null)
                {
                    if (attendanceRecord.InsideDuration >= minWorkDuration)
                    {
                        statusId = AttendanceStatus.Present;
                    }
                    else
                    {
                        statusId = AttendanceStatus.IncompleteShift;
                    }
                }
                else
                {
                    Holiday holiday = holidays.FirstOrDefault(x => x.Date == currentDate);
                    if (holiday is not null)
                    {

                        if (holiday.Type is AttendanceStatus.NormalHoliday)
                        {
                            statusId = AttendanceStatus.NormalHoliday;
                        }
                        else if (holiday.Type == AttendanceStatus.RestrictedHoliday)
                        {
                            LeaveRequest leaveRequestForHoliday = leaveRequests.FirstOrDefault(x => x.FromDate <= currentDate && x.ToDate >= currentDate);

                            if (leaveRequestForHoliday is not null)
                            {
                                LeaveType leaveType = leaveTypes.FirstOrDefault(x => x.Id == leaveRequestForHoliday.TypeId);

                                if (leaveType is not null && leaveType.LeaveCategoryId is LeaveCategory.RestrictedHoliday)
                                {
                                    statusId = AttendanceStatus.RestrictedHoliday;
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
                                statusId = AttendanceStatus.WFH;
                            }
                            else
                            {
                                statusId = AttendanceStatus.Leave;
                            }
                        }
                        else
                        {
                            statusId = AttendanceStatus.Absent;
                        }
                    }
                }
                weeklySummary.Add(new WeeklySummaryDTO
                {
                    Date = currentDate,
                    ArrivalTime = attendanceRecord?.ArrivalTime,
                    DepartureTime = attendanceRecord?.DepartureTime,
                    InsideDuration = attendanceRecord?.InsideDuration,
                    OutsideDuration = attendanceRecord?.OutsideDuration,
                    MissedPunch = attendanceRecord?.MissedPunch,
                    AttendanceStatusId = statusId
                });
            }
            return weeklySummary;
        }
    }
}
