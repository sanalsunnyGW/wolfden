using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.DailyStatus
{
    public class DailyStatusQueryHandler(WolfDenContext context) : IRequestHandler<DailyStatusQuery, List<DailyStatusDTO>>
    {
        private readonly WolfDenContext _context = context;
        public async Task<List<DailyStatusDTO>> Handle(DailyStatusQuery request, CancellationToken cancellationToken)
        {
            int minWorkDuration = 360;

            DateOnly monthStart = new DateOnly(request.Year, request.Month, 1);
            DateOnly monthEnd = monthStart.AddMonths(1).AddDays(-1);

            List<DailyStatusDTO> dailyStatuses = new List<DailyStatusDTO>();

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

            List<LeaveRequest> leave = leaveRequests
               .Where(x => x.HalfDay == true)
               .ToList();

            Dictionary<DateOnly, LeaveRequest> leaveDictionary = leave.ToDictionary(x => x.FromDate);

            AttendanceStatus statusId = AttendanceStatus.Absent;
            for (var currentDate = monthStart; currentDate <= monthEnd; currentDate = currentDate.AddDays(1))
            {
                if (currentDate > today)
                {
                    break;
                }

                LeaveRequest? halfDay = leaveDictionary.GetValueOrDefault(currentDate);

                if (halfDay is not null)
                {
                    minWorkDuration = minWorkDuration / 2;
                }

                DailyAttendence attendanceRecord = attendanceRecords.FirstOrDefault(x => x.Date == currentDate);
                if (attendanceRecord is not null)
                {

                    if (attendanceRecord.InsideDuration >= minWorkDuration)
                    {
                        if (halfDay is not null)
                        {
                            statusId = AttendanceStatus.HalfDay;
                        }
                        else
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

                        else if (holiday.Type is AttendanceStatus.RestrictedHoliday)
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
                dailyStatuses.Add(new DailyStatusDTO
                {
                    Date = currentDate,
                    AttendanceStatusId = statusId
                });
            }
            return dailyStatuses;
        }
    }
}
