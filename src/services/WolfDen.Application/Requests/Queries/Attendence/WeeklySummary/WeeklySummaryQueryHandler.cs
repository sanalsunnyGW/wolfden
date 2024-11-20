using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.WeeklySummary
{
    public class WeeklySummaryQueryHandler : IRequestHandler<WeeklySummaryQuery, List<WeeklySummaryDTO>>
    {
        private readonly WolfDenContext _context;

        public WeeklySummaryQueryHandler(WolfDenContext context)
        {
            _context = context;
        }

        public async Task<List<WeeklySummaryDTO>> Handle(WeeklySummaryQuery request, CancellationToken cancellationToken)
        {
            int minWorkDuration = 360;

            List<WeeklySummaryDTO> weeklySummary = new List<WeeklySummaryDTO>();

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);


            List<DailyAttendence> attendanceRecords = await _context.DailyAttendence
                .Where(x => x.EmployeeId == request.EmployeeId && x.Date >= request.WeekStart && x.Date <= request.WeekEnd)
                .ToListAsync(cancellationToken);

            List<Holiday> holidays = await _context.Holiday
                .Where(x => x.Date >= request.WeekStart && x.Date <= request.WeekEnd)
                .ToListAsync(cancellationToken);

            List<LeaveRequest> leaveRequests = await _context.LeaveRequests
                .Where(x => x.EmployeeId == request.EmployeeId &&
                            x.FromDate <= request.WeekEnd && x.ToDate >= request.WeekStart &&
                            x.LeaveRequestStatusId == LeaveRequestStatus.Approved)
                .ToListAsync(cancellationToken);

            List<LeaveType> leaveTypes = await _context.LeaveType.ToListAsync(cancellationToken);

            for (var currentDate = request.WeekStart; currentDate <= request.WeekEnd; currentDate = currentDate.AddDays(1))
            {
                if (currentDate > today)
                {
                    continue;
                }

                AttendanceStatus statusId = AttendanceStatus.Absent;

                DailyAttendence attendanceRecord = attendanceRecords.FirstOrDefault(x => x.Date == currentDate);
                if (attendanceRecord != null)
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
                    if (holiday != null)
                    {

                        if (holiday.Type is AttendanceStatus.NormalHoliday)
                        {
                            statusId = AttendanceStatus.NormalHoliday;
                        }

                        else if (holiday.Type == AttendanceStatus.RestrictedHoliday)
                        {
                            LeaveRequest leaveRequestForHoliday = leaveRequests.FirstOrDefault(x => x.FromDate <= currentDate && x.ToDate >= currentDate);

                            if (leaveRequestForHoliday != null)
                            {
                                LeaveType leaveType = leaveTypes.FirstOrDefault(x => x.Id == leaveRequestForHoliday.TypeId);

                                if (leaveType != null && leaveType.LeaveCategoryId is LeaveCategory.RestrictedHoliday)
                                {
                                    statusId = AttendanceStatus.RestrictedHoliday;
                                }
                            }
                        }
                    }

                    else
                    {
                        LeaveRequest leaveRequest = leaveRequests.FirstOrDefault(x => x.FromDate <= currentDate && x.ToDate >= currentDate);
                        if (leaveRequest != null)
                        {
                            LeaveType leaveType = leaveTypes.FirstOrDefault(x => x.Id == leaveRequest.TypeId);
                            if (leaveType != null && leaveType.LeaveCategoryId is LeaveCategory.WorkFromHome)
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
