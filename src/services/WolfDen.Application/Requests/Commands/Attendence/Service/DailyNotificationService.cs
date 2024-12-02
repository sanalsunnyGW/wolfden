using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.Requests.Commands.Attendence.SendNotification;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Attendence.Service
{
    public class DailyNotificationService(WolfDenContext context,IMediator mediator)
    {
        private readonly WolfDenContext _context =context;
        private readonly IMediator _mediator=mediator;
        public async Task SendNotificationsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var minWorkDuration = 360;

            List<DailyAttendence> attendanceRecords = await _context.DailyAttendence
                .Include(x => x.Employee)
                .Where(x => x.Date == today)
                .ToListAsync();

            foreach (var record in attendanceRecords)
            {
                string employeeMessage = $"Your working duration today is {record.InsideDuration / 60} hours and {record.InsideDuration % 60} minutes.";
                string employeeIncompleteMessage = $"Incomplete Shift !!! Your working duration today is {record.InsideDuration / 60} hours and {record.InsideDuration % 60} minutes.";

                NotificationCommand sendEmployeeNotificationCommand = new NotificationCommand
                {
                    EmployeeId = record.EmployeeId,
                    Message = record.InsideDuration < minWorkDuration
                            ? employeeIncompleteMessage
                            : employeeMessage
                   
                };
                await _mediator.Send(sendEmployeeNotificationCommand);
                
                if (record.InsideDuration < minWorkDuration)
                {
                    Employee? manager = await _context.Employees
                        .FirstOrDefaultAsync(e => e.Id == record.Employee.ManagerId);

                    if (manager != null)
                    {
                        string managerMessage = $"Employee {record.Employee.FirstName} has worked {record.InsideDuration / 60} hours today.";
                        NotificationCommand sendManagerNotificationCommand = new NotificationCommand
                        {
                            EmployeeId = manager.Id,
                            Message = managerMessage
                        };
                        await _mediator.Send(sendManagerNotificationCommand);
                    }
                }
            }
        }
        }
}
