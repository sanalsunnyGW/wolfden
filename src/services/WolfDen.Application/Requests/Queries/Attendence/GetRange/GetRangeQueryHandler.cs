using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.GetRange
{
    public class GetRangeQueryHandler : IRequestHandler<GetRangeQuery, RangeDTO>
    {

        private readonly WolfDenContext _context;
        public GetRangeQueryHandler(WolfDenContext context)
        {
            _context = context;
        }
        public async Task<RangeDTO> Handle(GetRangeQuery request, CancellationToken cancellationToken)
        {
            List<DateOnly> previousDates = await _context.AttendenceClose.Select(x => x.PreviousAttendanceClosed).ToListAsync();
            List<DateOnly> closedDates = await _context.AttendenceClose.Select(x => x.AttendanceClosedDate).ToListAsync();
            RangeDTO range = new RangeDTO();
            range.PreviousAttendanceClosedDate = previousDates;
            range.AttendanceClosedDate = closedDates;
            return range;
        }
    }
}