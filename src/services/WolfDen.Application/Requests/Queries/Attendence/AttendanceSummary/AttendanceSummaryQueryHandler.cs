//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using WolfDen.Application.Requests.DTOs.Attendence;
//using WolfDen.Infrastructure.Data;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

//namespace WolfDen.Application.Requests.Queries.Attendence.AttendanceSummary
//{
//    public class AttendanceSummaryQueryHandler : IRequestHandler<AttendanceSummaryQuery, AttendanceSummaryDTO>
//    {


//        private readonly WolfDenContext _context;

//        public AttendanceSummaryQueryHandler(WolfDenContext context)
//        {
//            _context = context;
//        }

//        public async Task<AttendanceSummaryDTO> Handle(AttendanceSummaryQuery request, CancellationToken cancellationToken)
//        {
//            if (request.Year <=0 || request.Month <=0 || request.Month>12)
//            {
//                throw new ArgumentException("invalid year and month");
//            }

//            DateOnly monthStart = new DateOnly(request.Year, request.Month, 1);
//            DateOnly monthEnd = monthStart.AddMonths(1).AddDays(-1);

//            var attendanceData = await _context.Status
//                .Where(s => s.EmployeeId == request.EmployeeId && s.Date >= monthStart && s.Date <= monthEnd)
//                .Include(s=>s.StatusType)
//                .GroupBy(s => s.StatusType.StatusName)
//                .Select(g => new
//                {
//                    Status = g.Key,
//                    Count = g.Count()
//                })
//                .ToListAsync(cancellationToken);


//            AttendanceSummaryDTO summaryDto = new AttendanceSummaryDTO
//            {
//                Present = attendanceData.FirstOrDefault(x => x.Status == "Present")?.Count ?? 0,
//                Absent = attendanceData.FirstOrDefault(x=>x.Status == "Absent")?.Count ?? 0,
//                Late = attendanceData.FirstOrDefault(x => x.Status == "late")?.Count ?? 0,
//                WFH = attendanceData.FirstOrDefault(x => x.Status == "wfh")?.Count ?? 0

//            };

//            return summaryDto;
//        }
//    }
//}
