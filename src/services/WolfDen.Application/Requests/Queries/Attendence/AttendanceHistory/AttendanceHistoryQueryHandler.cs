using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.AttendanceHistory
{
    public class AttendanceHistoryQueryHandler(WolfDenContext context): IRequestHandler<AttendanceHistoryQuery, AttendanceHistoryDTO>
    {
        private readonly WolfDenContext _context=context;
        public async Task<AttendanceHistoryDTO> Handle(AttendanceHistoryQuery request, CancellationToken cancellationToken)
        {
            DateOnly monthStart = new DateOnly(request.Year, request.Month, 1);
            DateOnly monthEnd = monthStart.AddMonths(1).AddDays(-1);

            int minWorkDuration = 360;

            List<WeeklySummaryDTO> attendanceHistory = new List<WeeklySummaryDTO>();
            List<WeeklySummaryDTO> filteredAttendance = new List<WeeklySummaryDTO>();

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

                AttendanceStatus statusId = AttendanceStatus.Absent;

                DailyAttendence? attendanceRecord = attendanceRecords.FirstOrDefault(x => x.Date == currentDate);
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
                    Holiday? holiday = holidays.FirstOrDefault(x => x.Date == currentDate);
                    if (holiday is not null)
                    {

                        if (holiday.Type is AttendanceStatus.NormalHoliday)
                        {
                            statusId = AttendanceStatus.NormalHoliday;
                        }
                        else if (holiday.Type == AttendanceStatus.RestrictedHoliday)
                        {
                            LeaveRequest? leaveRequestForHoliday = leaveRequests.FirstOrDefault(x => x.FromDate <= currentDate && x.ToDate >= currentDate);

                            if (leaveRequestForHoliday is not null)
                            {
                                LeaveType? leaveType = leaveTypes.FirstOrDefault(x => x.Id == leaveRequestForHoliday.TypeId);

                                if (leaveType is not null && leaveType.LeaveCategoryId is LeaveCategory.RestrictedHoliday)
                                {
                                    statusId = AttendanceStatus.RestrictedHoliday;
                                }
                            }
                        }
                    }
                    else
                    {
                        LeaveRequest? leaveRequest = leaveRequests.FirstOrDefault(x => x.FromDate <= currentDate && x.ToDate >= currentDate);
                        if (leaveRequest is not null)
                        {
                            LeaveType? leaveType = leaveTypes.FirstOrDefault(x => x.Id == leaveRequest.TypeId);
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
                attendanceHistory.Add(new WeeklySummaryDTO
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

            if (request.AttendanceStatusId.HasValue)
            {
                filteredAttendance = attendanceHistory.Where(x => x.AttendanceStatusId == request.AttendanceStatusId).ToList();
            }
            else
            {
                filteredAttendance = attendanceHistory;
            }

            int totalCount = filteredAttendance.Count();
            int totalPageCount = (int)Math.Ceiling((decimal)totalCount / request.PageSize);
            
            List<WeeklySummaryDTO> displayAttendance = filteredAttendance.Skip(request.PageNumber * request.PageSize).Take(request.PageSize).ToList();

            return new AttendanceHistoryDTO
            {
                AttendanceHistory = displayAttendance,
                TotalPages = totalCount
            };
        }
    }
}
