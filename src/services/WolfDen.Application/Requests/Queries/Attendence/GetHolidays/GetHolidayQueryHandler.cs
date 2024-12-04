using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.GetHolidays
{
    public class GetHolidayQueryHandler : IRequestHandler<GetHolidayQuery, List<HolidayDTO>>
    {
        private readonly WolfDenContext _context;
        public GetHolidayQueryHandler(WolfDenContext context)
        {
            _context = context;
        }
        public async Task<List<HolidayDTO>> Handle(GetHolidayQuery request, CancellationToken cancellationToken)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);

            var holidays = await _context.Holiday
                .Where(x => x.Date >= today) 
                .Select(x => new HolidayDTO
                {
                    Date = x.Date,
                    Type = x.Type,
                    Description = x.Description,
                    
                })
                .ToListAsync(cancellationToken);

            return holidays;
        }
    }
}
