using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Domain.Enums;
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

            List<WeeklySummaryDTO> weeklySummary = new List<WeeklySummaryDTO>();

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            for (var currentDate = request.WeekStart; currentDate <= request.WeekEnd; currentDate = currentDate.AddDays(1))
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

                
                weeklySummary.Add(new WeeklySummaryDTO
                {
                    Date = currentDate,
                    ArrivalTime=attendanceRecord?.ArrivalTime,
                    DepartureTime=attendanceRecord?.DepartureTime,
                    InsideDuration=attendanceRecord?.InsideDuration,
                    OutsideDuration=attendanceRecord?.OutsideDuration,
                    MissedPunch=attendanceRecord?.MissedPunch,
                    AttendanceStatusId = statusId
                });
            }

            return weeklySummary;



        }
    }
}
