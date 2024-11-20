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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WolfDen.Application.Requests.Queries.Attendence.AttendanceSummary
{
    public class AttendanceSummaryQueryHandler : IRequestHandler<AttendanceSummaryQuery, AttendanceSummaryDTO>
    {


        private readonly WolfDenContext _context;

        public AttendanceSummaryQueryHandler(WolfDenContext context)
        {
            _context = context;
        }

        public async Task<AttendanceSummaryDTO> Handle(AttendanceSummaryQuery request, CancellationToken cancellationToken)
        {
            if (request.Year <= 0 || request.Month <= 0 || request.Month > 12)
            {
                throw new ArgumentException("invalid year and month");
            }

            DateOnly monthStart = new DateOnly(request.Year, request.Month, 1);
            DateOnly monthEnd = monthStart.AddMonths(1).AddDays(-1);

            AttendanceSummaryDTO summaryDto = new AttendanceSummaryDTO
            {
                Present = 0,
                Absent = 0,
                IncompleteShift = 0,
                RestrictedHoliday = 0,
                NormalHoliday = 0,
                WFH = 0,
                Leave = 0
            };

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            for (var currentDate = monthStart; currentDate <= monthEnd; currentDate = currentDate.AddDays(1))
            {

                if (currentDate > today)
                {
                    continue;
                }

                var attendanceRecord = await _context.DailyAttendence
                    .Where(x => x.EmployeeId == request.EmployeeId && x.Date == currentDate)
                    .FirstOrDefaultAsync(cancellationToken);

                if (attendanceRecord != null)
                {
                    
                    if (attendanceRecord.InsideDuration >= 360) 
                    {
                        summaryDto.Present++;
                    }
                    else
                    {
                        summaryDto.IncompleteShift++;
                    }
                }
                else
                {

                    var holiday = await _context.Holiday
                        .Where(x => x.Date == currentDate)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (holiday != null)
                    {

                        if (holiday.Type == AttendanceStatus.NormalHoliday)
                        {
                            summaryDto.NormalHoliday++;
                        }
                        else
                        {
                            summaryDto.RestrictedHoliday++;
                        }
                    }
                    else
                    {
                        
                        var leaveRequest = await _context.LeaveRequests
                            .Where(x => x.EmployeeId == request.EmployeeId &&
                                        x.FromDate <= currentDate && x.ToDate >= currentDate &&
                                        x.LeaveRequestStatusId == LeaveRequestStatus.Approved)
                            .FirstOrDefaultAsync(cancellationToken);

                        if (leaveRequest != null)
                        {
                            
                            var leaveType = await _context.LeaveType
                                .Where(x => x.Id == leaveRequest.TypeId)
                                .FirstOrDefaultAsync(cancellationToken);

                            if (leaveType != null && leaveType.LeaveCategoryId == LeaveCategory.WorkFromHome)
                            {
                                summaryDto.WFH++;
                            }
                            else
                            {
                                summaryDto.Leave++;
                            }
                        }
                        else
                        {
                            
                            summaryDto.Absent++;
                        }
                    }
                }
            }

            return summaryDto;
        }
    }
}
