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

                    if (newEntry.InsideDuration < min)
                    {
                        IMediator _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        SendEmailCommand sendEmailCommand = new SendEmailCommand();
                        sendEmailCommand.EmployeeId = newEntry.EmployeeId;
                        sendEmailCommand.Subject = "Incomplete Shift";
                        sendEmailCommand.Email = employee.Email;
                        sendEmailCommand.Message = $"{employee.FirstName}'s shift on {newEntry.Date} is marked as incomplete due to insufficient hours; please review and address the issue.";
                        await _mediator.Send(sendEmailCommand);
                    }
                    else
                    {
                        IMediator _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        SendEmailCommand sendEmailCommand = new SendEmailCommand();
                        sendEmailCommand.EmployeeId = newEntry.EmployeeId;
                        sendEmailCommand.Subject = "Overtime Acknowledgement";
                        sendEmailCommand.Email = employee.Email;
                        sendEmailCommand.Message = $"Great job {employee.FirstName}! Your extra hours on {newEntry.Date} are appreciated";
                        await _mediator.Send(sendEmailCommand);
                    }
                }
            }
        }
    }
}
