using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.Requests.DTOs.Attendence;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.DailyAttendanceReport
{
    public class DailyDetailsPdfQueryHandler : IRequestHandler<DailyDetailsPdf, DailyAttendanceDTO>
    {
        private readonly WolfDenContext _context;
        public DailyDetailsPdfQueryHandler(WolfDenContext context)
        {
            _context = context;
        }
        public async Task<DailyAttendanceDTO> Handle(DailyDetailsPdf request, CancellationToken cancellationToken)
        {
            int minWorkDuration = 360;
            var attendence = await _context.DailyAttendence.Where(x => x.EmployeeId == request.EmployeeId && x.Date == request.Date).Select(x => new DailyAttendanceDTO
            {
                ArrivalTime = x.ArrivalTime,
                DepartureTime = x.DepartureTime,
                InsideHours = x.InsideDuration,
                OutsideHours = x.OutsideDuration,
                MissedPunch = x.MissedPunch,
            }).FirstOrDefaultAsync(cancellationToken);

            if(attendence is null)
            {
                var notPresentDay = new DailyAttendanceDTO();
                Holiday holiday = await _context.Holiday.Where(x => x.Date == request.Date).FirstOrDefaultAsync(cancellationToken);
                if(holiday is not null)
                {
                    if (holiday.Type == AttendanceStatus.NormalHoliday)
                    {
                        notPresentDay.AttendanceStatusId = AttendanceStatus.NormalHoliday;
                    }
                    else
                    {
                        LeaveRequest leave = await _context.LeaveRequests.Where(x => x.EmployeeId == request.EmployeeId && x.FromDate <= request.Date && request.Date<=x.ToDate && x.LeaveRequestStatusId == LeaveRequestStatus.Approved).Include(x => x.LeaveType).FirstOrDefaultAsync(cancellationToken);
                        if (leave is null)
                        {
                            notPresentDay.AttendanceStatusId = AttendanceStatus.Absent;
                        }
                        else
                        {
                            notPresentDay.AttendanceStatusId = AttendanceStatus.RestrictedHoliday;
                        }    
                    }
                }
                else
                {
                    LeaveRequest leave = await _context.LeaveRequests.Where(x => x.EmployeeId == request.EmployeeId && x.FromDate <= request.Date && request.Date <= x.ToDate && x.LeaveRequestStatusId == LeaveRequestStatus.Approved).Include(x => x.LeaveType).FirstOrDefaultAsync(cancellationToken);
                    if (leave is null)
                    {
=                        notPresentDay.AttendanceStatusId = AttendanceStatus.Absent;
                    }
                    else
                    {
                        LeaveType leaveType = await _context.LeaveType.FirstOrDefaultAsync(x => x.Id == leave.TypeId);
                        if (leaveType.LeaveCategoryId == LeaveCategory.WorkFromHome)
                        {
                            notPresentDay.AttendanceStatusId = AttendanceStatus.WFH;
                        }      
                        else
                        {
                            notPresentDay.AttendanceStatusId = AttendanceStatus.Absent;
                        }
                    }
                }  
                return notPresentDay;
            }
            else
            {
                if (attendence.InsideHours >= minWorkDuration)
                {
                    attendence.AttendanceStatusId = AttendanceStatus.Present;
                }
                else
                {
                    attendence.AttendanceStatusId = AttendanceStatus.IncompleteShift;
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
            return attendence;
        }
    }
}
