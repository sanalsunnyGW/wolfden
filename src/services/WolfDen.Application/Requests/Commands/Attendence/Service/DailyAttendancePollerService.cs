using System.Threading;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WolfDen.Application.Requests.Commands.Attendence.Email;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Attendence.Service
{
    public class DailyAttendancePollerService
    {
        private readonly ILogger<DailyAttendancePollerService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public DailyAttendancePollerService(IMediator mediator, ILogger<DailyAttendancePollerService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task SendEmail()
        {
            int min = 360;
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                WolfDenContext _context = scope.ServiceProvider.GetRequiredService<WolfDenContext>();

                _logger.LogInformation("Background service is running at: {time}", DateTimeOffset.Now);

                DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
                List<DailyAttendence> newEntries = await _context.DailyAttendence.Include(x => x.Employee)
                .Where(a => a.Date == today.AddDays(-1) && a.EmailSent == false && a.EmployeeId==42).ToListAsync();

                List<LeaveRequest>? leave = await _context.LeaveRequests
                          .Where(x => x.LeaveRequestStatusId == LeaveRequestStatus.Approved && x.HalfDay == true && x.FromDate == today.AddDays(-1)).ToListAsync();

                foreach (DailyAttendence newEntry in newEntries)
                {
                    IMediator _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    LeaveRequest? halfDay = leave.Find(x => x.EmployeeId == newEntry.EmployeeId);

                    if (halfDay is not null)
                    {
                        min = min / 2;
                    }

                    SendEmailCommand sendEmailCommand = new SendEmailCommand
                    {
                        EmployeeId = newEntry.EmployeeId,
                        Name = newEntry.Employee.FirstName + " " + newEntry.Employee.LastName,
                        Email = newEntry.Employee.Email,
                        Duration = newEntry.InsideDuration,
                        ArrivalTime = newEntry.ArrivalTime,
                        DepartureTime = newEntry.DepartureTime,
                        Date=newEntry.Date,
                        Message = newEntry.InsideDuration < min
                            ? $"{newEntry.Employee.FirstName}'s shift on {newEntry.Date} is marked as incomplete due to insufficient hours; please review and address the issue."
                            : $"Great job {newEntry.Employee.FirstName}! Your extra hours on {newEntry.Date} are appreciated",
                        Subject = newEntry.InsideDuration < min ? "Incomplete Shift" : "Attendance Summary",
                        Status = newEntry.InsideDuration < min ? "Incomplete Shift" : "Shift Complete",
                    };
                    await _mediator.Send(sendEmailCommand);
                }
            }
        }
    }
}