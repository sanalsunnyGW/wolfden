using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.Requests.DTOs.Attendence;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.DailyStatus
{
    public class DailyDetailsQueryHandler :IRequestHandler<DailyDetails,DailyAttendanceDTO>
    {
        private readonly WolfDenContext _context;
        public DailyDetailsQueryHandler(WolfDenContext context)
        {
            _context = context;
        }
        public async Task<DailyAttendanceDTO> Handle(DailyDetails request, CancellationToken cancellationToken)
        {

            var attendenceRecords = await _context.AttendenceLog.Where(x => x.EmployeeId == request.EmployeeId && x.PunchDate == request.Date).Include(x => x.Device)
              .Select(x => new AttendenceLogDTO
              {
                  Time = x.PunchTime,
                  DeviceName = x.Device.Name,
                  Direction = x.Direction
              }).ToListAsync(cancellationToken);

            var attendence = await _context.DailyAttendence.Where(x => x.EmployeeId == request.EmployeeId && x.Date == request.Date).Select(x => new DailyAttendanceDTO
            {
                ArrivalTime = x.ArrivalTime,
                DepartureTime = x.DepartureTime,
                InsideHours = x.InsideDuration,
                OutsideHours = x.OutsideDuration,
                MissedPunch = x.MissedPunch

            }).FirstOrDefaultAsync(cancellationToken);

            var status = await _context.Status.Include(x => x.StatusType).Where(x => x.EmployeeId == request.EmployeeId && x.Date == request.Date).Select(x => x.StatusType.StatusName).FirstOrDefaultAsync(cancellationToken);
            attendence.Status = status;
            attendence.DailyLog = attendenceRecords;
            return attendence;
        }
    }
}
