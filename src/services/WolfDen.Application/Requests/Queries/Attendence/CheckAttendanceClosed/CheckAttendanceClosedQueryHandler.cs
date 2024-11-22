using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Application.Requests.Queries.Attendence.CheckAttendanceClose;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.CheckAttendanceClosed
{
    public class CheckAttendanceClosedQueryHandler : IRequestHandler<CheckAttendanceClosedQuery,CheckAttendanceClosedDTO>
    {
        private readonly WolfDenContext _context;
        public CheckAttendanceClosedQueryHandler(WolfDenContext context)
        {
            _context = context;
        }
        public async Task<CheckAttendanceClosedDTO> Handle(CheckAttendanceClosedQuery request, CancellationToken cancellationToken)
        {
            AttendenceClose? attendenceClose=await _context.AttendenceClose
                .Where(x=>x.AttendanceClosedDate.Month==request.Month && x.AttendanceClosedDate.Year==request.Year && x.IsClosed==true)
                .FirstOrDefaultAsync(cancellationToken);
            CheckAttendanceClosedDTO checkAttendanceClosedDTO = new CheckAttendanceClosedDTO();
            if (attendenceClose != null)
            {
                checkAttendanceClosedDTO.status = true;
                return checkAttendanceClosedDTO;
            }
            checkAttendanceClosedDTO.status = false;
            return checkAttendanceClosedDTO;
        }
    }
}
