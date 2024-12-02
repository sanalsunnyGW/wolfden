using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WolfDen.Application.Requests.Commands.Attendence.Email;
using WolfDen.Domain.Entity;
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

                List<DailyAttendence> newEntries = await _context.DailyAttendence
                .Where(a =>a.Date == DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1) && a.EmailSent==false).ToListAsync();

                foreach (DailyAttendence newEntry in newEntries)
                {
                    Employee? employee = await _context.Employees
                        .Where(e => e.Id == newEntry.EmployeeId).FirstOrDefaultAsync();

                    IMediator _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    SendEmailCommand sendEmailCommand = new SendEmailCommand
                    {
                        EmployeeId = newEntry.EmployeeId,
                        Email = employee.Id,
                        Message = newEntry.InsideDuration < min
                            ? $"{employee.FirstName}'s shift on {newEntry.Date} is marked as incomplete due to insufficient hours; please review and address the issue."
                            : $"Great job {employee.FirstName}! Your extra hours on {newEntry.Date} are appreciated",
                        Subject = newEntry.InsideDuration < min ? "Incomplete Shift" : "Overtime Acknowledgement"
                    };

                    await _mediator.Send(sendEmailCommand);
                }
            }
        }
    }
}
