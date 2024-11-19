using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.WeeklySummary
{
    public class WeeklySummaryQueryHandler : IRequestHandler<WeeklySummaryQuery, List<WeeklySummaryDTO>>
    {
        private readonly WolfDenContext _context;

        public WeeklySummaryQueryHandler(WolfDenContext context)
        {
            _context = context;
        }

        public async Task<List<WeeklySummaryDTO>> Handle(WeeklySummaryQuery request, CancellationToken cancellationToken)
        {
            var weeklyData = await _context.DailyAttendence
            .Where(s => s.EmployeeId == request.EmployeeId && s.Date >= request.WeekStart && s.Date <= request.WeekEnd)
            .Select(s => new WeeklySummaryDTO
            {
                Date = s.Date,
                ArrivalTime = s.ArrivalTime,
                DepartureTime = s.DepartureTime,
                InsideDuration = s.InsideDuration,
                OutsideDuration = s.OutsideDuration,
                MissedPunch = s.MissedPunch,
                AttendanceStatusId=s.AttendanceStatusId
                
            })
            .ToListAsync(cancellationToken);

            //foreach (var entry in weeklyData)
            //{
            //    var status = await _context.Status
            //        .Include(x => x.StatusType)
            //        .Where(x => x.EmployeeId == request.EmployeeId && x.Date == entry.Date)
            //        .Select(x => x.StatusType.StatusName)
            //        .FirstOrDefaultAsync(cancellationToken);

            //    entry.Status = status;
            //}

            return weeklyData;


        }
    }
}
