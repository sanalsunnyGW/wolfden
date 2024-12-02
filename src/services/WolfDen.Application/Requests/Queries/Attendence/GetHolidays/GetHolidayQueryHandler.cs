using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.GetHolidays
{
    public class GetHolidayQueryHandler(WolfDenContext context) : IRequestHandler<GetHolidayQuery, List<HolidayDTO>>
    {
        private readonly WolfDenContext _context=context;
        public async Task<List<HolidayDTO>> Handle(GetHolidayQuery request, CancellationToken cancellationToken)
        {
            DateOnly yearStart = new DateOnly(request.Year, 1, 1);
            DateOnly yearEnd = new DateOnly(request.Year+1, 1, 1);

            List<HolidayDTO> holidayList = new List<HolidayDTO>();

            var holidays = await _context.Holiday
                .Where(x=>x.Date >= yearStart && x.Date < yearEnd)
                .ToListAsync(cancellationToken);

            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);

            for (var currentDate = yearStart; currentDate < yearEnd; currentDate.AddDays(1))
            {
                if (currentDate < today)
                    continue;

                holidayList = holidays
                    .Where(x => x.Date >= today && x.Date < yearEnd)
                    .Select(x => new HolidayDTO
                    {
                        Date = x.Date,
                        Type = x.Type
                    }).ToList();
            }

            return holidayList;
        }
    }
}
