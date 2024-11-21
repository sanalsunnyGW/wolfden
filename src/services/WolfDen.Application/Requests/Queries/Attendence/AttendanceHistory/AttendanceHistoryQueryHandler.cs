using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.AttendanceHistory
{
    public class AttendanceHistoryQueryHandler(WolfDenContext context): IRequestHandler<AttendanceHistoryQuery, List<AttendanceHistoryDTO>>
    {
        private readonly WolfDenContext _context=context;
        public async Task<List<AttendanceHistoryDTO>> Handle(AttendanceHistoryQuery request, CancellationToken cancellationToken)
        {
            DateOnly yearStart = new DateOnly(request.Year,1,1);
            DateOnly yearEnd = new DateOnly(request.Year+1,1,0);

            var dailyAttendance =await _context.DailyAttendence
                .Where(x=>x.EmployeeId == request.EmployeeId && x.Date >= yearStart && x.Date <=yearEnd)
                .Select(x=> new AttendanceHistoryDTO
                {
                    ArrivalTime = x.ArrivalTime,
                    DepartureTime = x.DepartureTime,
                    InsideHours = x.InsideDuration,
                    OutsideHours = x.OutsideDuration,
                    MissedPunch = x.MissedPunch,

                })
                .ToListAsync();

            int totalCount=dailyAttendance.Count;
            int perPageCount = 15;

            var records = new List<AttendanceHistoryDTO>
            {
                Date
            };
            
            var displayAttendance = dailyAttendance.Skip(request.CurrentPage * perPageCount).Take(perPageCount).ToList();

            return records;
        }
    }
}
