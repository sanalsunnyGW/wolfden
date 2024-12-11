using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Application.Requests.Queries.Attendence.CheckAttendanceClose;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.CheckAttendanceClosed
{
    public class CheckAttendanceClosedQueryHandler : IRequestHandler<CheckAttendanceClosedQuery, CheckAttendanceClosedDTO>
    {
        private readonly WolfDenContext _context;
        public CheckAttendanceClosedQueryHandler(WolfDenContext context)
        {
            _context = context;
        }
        public async Task<CheckAttendanceClosedDTO> Handle(CheckAttendanceClosedQuery request, CancellationToken cancellationToken)
        {
            DateOnly previousFirst = await _context.AttendenceClose.MinAsync(x => x.PreviousAttendanceClosed);
            DateOnly LastClosed = await _context.AttendenceClose.MaxAsync(x => x.AttendanceClosedDate);
            DateTime closingDateString = DateTime.Parse(request.AttendanceClose);
            DateOnly closingDate = DateOnly.FromDateTime(closingDateString);
            AttendenceClose? attendenceClose = await _context.AttendenceClose
                .Where(x => closingDate >= previousFirst && closingDate <= LastClosed)
                .FirstOrDefaultAsync(cancellationToken);
            CheckAttendanceClosedDTO checkAttendanceClosedDTO = new CheckAttendanceClosedDTO();
            if (attendenceClose is not null)
            {
                checkAttendanceClosedDTO.status = true;
                return checkAttendanceClosedDTO;
            }
            checkAttendanceClosedDTO.status = false;
            return checkAttendanceClosedDTO;
        }
    }
}