using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.Requests.Queries.Attendence.CheckAttendanceClose;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.CheckAttendanceClosed
{
    public class CheckAttendanceClosedQueryHandler : IRequestHandler<CheckAttendanceClosedQuery,bool>
    {
        private readonly WolfDenContext _context;
        public CheckAttendanceClosedQueryHandler(WolfDenContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(CheckAttendanceClosedQuery request, CancellationToken cancellationToken)
        {
            AttendenceClose? attendenceClose=await _context.AttendenceClose.Where(x=>x.AttendanceClosedDate.Month==request.Month && x.AttendanceClosedDate.Year==request.Year && x.IsClosed==true).FirstOrDefaultAsync(cancellationToken);
            if (attendenceClose != null)
            {
                return true;
            }
            return false;
        }
    }
}
