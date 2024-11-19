using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Application.Requests.DTOs.Attendence;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Attendence.DailyStatus
{
    public class DailyStatusQueryHandler : IRequestHandler<DailyStatusQuery, List<DailyStatusDTO>>
    {
        private readonly WolfDenContext _context;

        public DailyStatusQueryHandler(WolfDenContext context)
        {
            _context = context;
        }


        public async Task<List<DailyStatusDTO>> Handle(DailyStatusQuery request, CancellationToken cancellationToken)
        {
            if (request.Year <= 0 || request.Month <= 0 || request.Month > 12)
            {
                throw new ArgumentException("invalid year and month");
            }

            DateOnly monthStart = new DateOnly(request.Year, request.Month, 1);
            DateOnly monthEnd = monthStart.AddMonths(1).AddDays(-1);

            List<DailyStatusDTO> dailyStatuses = new List<DailyStatusDTO>();

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            for (var currentDate = monthStart; currentDate <= monthEnd; currentDate = currentDate.AddDays(1))
            {
                if(currentDate>today)
                {
                    continue;
                }
                
                AttendanceStatus statusId = AttendanceStatus.Absent;


                var attendanceRecord = await _context.DailyAttendence
                    .Where(x => x.EmployeeId == request.EmployeeId && x.Date == currentDate)
                    .FirstOrDefaultAsync(cancellationToken);

                if (attendanceRecord != null)
                {
                    
                    if (attendanceRecord.InsideDuration >= 360)
                    {
                        
                        statusId = AttendanceStatus.Present;
                    }
                    else
                    {
                        
                        statusId = AttendanceStatus.IncompleteShift; 
                    }
                }
                else
                {

                    var holiday = await _context.Holiday
                        .Where(x => x.Date == currentDate)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (holiday != null)
                    {
                        
                        statusId = holiday.Type == AttendanceStatus.NormalHoliday
                            ?AttendanceStatus.NormalHoliday
                            : AttendanceStatus.RestrictedHoliday;
                    }
                    else
                    {
                        
                        statusId = AttendanceStatus.Absent;
                    }
                }

                
                dailyStatuses.Add(new DailyStatusDTO
                {
                    Date = currentDate,
                    AttendanceStatusId = statusId
                });
            }

            return dailyStatuses;


        }
    }
}
