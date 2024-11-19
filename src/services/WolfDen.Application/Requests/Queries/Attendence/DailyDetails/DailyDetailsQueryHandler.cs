using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.Requests.DTOs.Attendence;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.DailyDetails
{
    public class DailyDetailsQueryHandler : IRequestHandler<DailyDetails, DailyAttendanceDTO>
    {
        private readonly WolfDenContext _context;
        public DailyDetailsQueryHandler(WolfDenContext context)
        {
            _context = context;
        }
        public async Task<DailyAttendanceDTO> Handle(DailyDetails request, CancellationToken cancellationToken)
        {
            var attendence = await _context.DailyAttendence.Where(x => x.EmployeeId == request.EmployeeId && x.Date == request.Date).Select(x => new DailyAttendanceDTO
            {
                ArrivalTime = x.ArrivalTime,
                DepartureTime = x.DepartureTime,
                InsideHours = x.InsideDuration,
                OutsideHours = x.OutsideDuration,
                MissedPunch = x.MissedPunch,
            }).FirstOrDefaultAsync(cancellationToken);

            if (attendence is null)
            {
                Holiday holiday = await _context.Holiday.Where(x => x.Date == request.Date).FirstOrDefaultAsync(cancellationToken);

                if (holiday.Type == AttendanceStatus.NormalHoliday)
                {
                    AttendanceStatus attendanceStatusId = AttendanceStatus.NormalHoliday;
                    attendence.AttendanceStatusId = attendanceStatusId;
                }
                else
                {
                    LeaveRequest leave = await _context.LeaveRequests.Where(x => x.EmployeeId == request.EmployeeId && x.FromDate == request.Date && x.LeaveRequestStatusId == LeaveRequestStatus.Approved).Include(x => x.LeaveType).FirstOrDefaultAsync(cancellationToken);
                    if (leave is null)
                    {
                        AttendanceStatus attendanceStatusId = AttendanceStatus.Absent;
                        attendence.AttendanceStatusId = attendanceStatusId;
                    }
                    else
                    {
                        LeaveType leaveType = await _context.LeaveType.FirstOrDefaultAsync(x => x.Id == leave.TypeId);
                        if (leaveType.LeaveCategoryId == LeaveCategory.WorkFromHome)
                        {
                            AttendanceStatus attendanceStatusId = AttendanceStatus.WFH;
                            attendence.AttendanceStatusId = attendanceStatusId;
                        }
                        else
                        {
                            AttendanceStatus attendanceStatusId = AttendanceStatus.RestrictedHoliday;
                            attendence.AttendanceStatusId = attendanceStatusId;
                        }
                    }
                }
            }
            else
            {
                await _context.AddAsync(attendence.AttendanceStatusId);
                if (attendence.InsideHours >= 360)
                {
                    AttendanceStatus attendanceStatusId = AttendanceStatus.Present;
                    attendence.AttendanceStatusId = attendanceStatusId;
                }
                else
                {
                    AttendanceStatus attendanceStatusId = AttendanceStatus.IncompleteShift;
                    attendence.AttendanceStatusId = attendanceStatusId;
                }
            }
            var attendenceRecords = await _context.AttendenceLog.Where(x => x.EmployeeId == request.EmployeeId && x.PunchDate == request.Date).Include(x => x.Device)
             .Select(x => new AttendenceLogDTO
             {
                 Time = x.PunchTime,
                 DeviceName = x.Device.Name,
                 Direction = x.Direction
             }).ToListAsync(cancellationToken);
            attendence.DailyLog = attendenceRecords;
            await _context.SaveChangesAsync(cancellationToken);
            return attendence;
        }
    }
}
