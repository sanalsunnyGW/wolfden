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
            var dailyAttendance =await _context.DailyAttendence
                .Where(x=>x.EmployeeId == request.EmployeeId)
                .ToListAsync();


        }
    }
}
