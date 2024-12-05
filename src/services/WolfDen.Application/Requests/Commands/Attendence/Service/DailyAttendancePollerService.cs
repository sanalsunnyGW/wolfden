using LanguageExt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WolfDen.Application.Requests.Commands.Attendence.Email;
using WolfDen.Application.Requests.Commands.Attendence.SendAbsenceEmail;
using WolfDen.Application.Requests.Commands.Attendence.SendNotification;
using WolfDen.Application.Services;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Attendence.Service
{
    public class DailyAttendancePollerService
    {
        private readonly ILogger<DailyAttendancePollerService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMediator _mediator;
        private readonly ManagerIdFinder _managerIdFinder;

        public DailyAttendancePollerService(IMediator mediator, ILogger<DailyAttendancePollerService> logger, IServiceScopeFactory serviceScopeFactory, ManagerIdFinder managerIdFinder)
        {
            _mediator = mediator;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _managerIdFinder = managerIdFinder;
        }

        public async Task ExecuteJobAsync()
        {
            int minWorkDuration = 360;
            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1);

            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                WolfDenContext _context = scope.ServiceProvider.GetRequiredService<WolfDenContext>();

                _logger.LogInformation("Background service is running at: {time}", DateTimeOffset.Now);

                List<Employee> allEmployees = await _context.Employees
                    .Where(x=>x.IsActive==true)
                    .ToListAsync();

                List<int> employeesWithAttendance = await _context.DailyAttendence
                   .Where(a => a.Date == today)
                   .Select(a => a.EmployeeId)
                   .ToListAsync();

                List<Employee> absentEmployees = allEmployees
                    .Where(emp => !employeesWithAttendance.Contains(emp.Id) && emp.Id==6)
                    .ToList();

                List<DailyAttendence> attendanceRecords = await _context.DailyAttendence
                    .Include(x => x.Employee)
                    .Where(a => a.Date == today && a.EmailSent == false && a.EmployeeId==10)
                    .ToListAsync();

                List<LeaveRequest> leaveRequests = await _context.LeaveRequests
                    .Where(x => x.LeaveRequestStatusId == LeaveRequestStatus.Approved && x.HalfDay == true && x.FromDate == today)
                    .ToListAsync();


                Dictionary<int, LeaveRequest> leaveDictionary = leaveRequests.ToDictionary(x => x.EmployeeId);

                foreach (Employee employee in absentEmployees)
                {
                    LeaveRequest? approvedLeave = leaveDictionary.GetValueOrDefault(employee.Id);

                    if (approvedLeave == null)
                    {
                        string absenceMessage = $"Dear {employee.FirstName}, you were marked as absent on ({today}). Please make sure to apply for leave.";

                        SendAbsenceEmailCommand sendAbsenceEmail = new SendAbsenceEmailCommand
                        {
                            EmployeeId = 6,
                            Name = employee.FirstName + " " + employee.LastName,
                            Email = employee.Email,
                            Date = today,
                            Message = absenceMessage,
                            Subject = "Absence Notification"
                        };

                        await _mediator.Send(sendAbsenceEmail);

                        NotificationCommand sendAbsenceNotification = new NotificationCommand
                        {
                            EmployeeIds = new List<int> { employee.Id },
                            Message = absenceMessage
                        };
                        await _mediator.Send(sendAbsenceNotification);
                    }
                }

                foreach (DailyAttendence record in attendanceRecords)
                {
                    LeaveRequest? halfDay = leaveDictionary.GetValueOrDefault(record.EmployeeId);

                    if (halfDay is not null)
                    {
                        minWorkDuration = minWorkDuration / 2;
                    }

                    string employeeMessage = $"Your working duration for {today} is {record.InsideDuration / 60} hours and {record.InsideDuration % 60} minutes.";
                    string employeeIncompleteMessage = $"Incomplete Shift !!! Your working duration today is {record.InsideDuration / 60} hours and {record.InsideDuration % 60} minutes.";

                    SendEmailCommand sendEmailCommand = new SendEmailCommand
                    {
                        EmployeeId = record.EmployeeId,
                        Name = record.Employee.FirstName + " " + record.Employee.LastName,
                        Email = record.Employee.Email,
                        Duration = record.InsideDuration,
                        ArrivalTime = record.ArrivalTime,
                        DepartureTime = record.DepartureTime,
                        Date = record.Date,
                        Message = record.InsideDuration < minWorkDuration
                            ? $"{record.Employee.FirstName}'s shift on {today} is marked as incomplete due to insufficient hours; please review and address the issue."
                            : $"Great job {record.Employee.FirstName}! Your extra hours on {today} are appreciated",
                        Subject = record.InsideDuration < minWorkDuration ? "Incomplete Shift" : "Attendance Summary",
                        Status = record.InsideDuration < minWorkDuration ? "Incomplete Shift" : "Shift Complete",
                        MissedPunch = record.MissedPunch != null ? record.MissedPunch : "-"
                    };

                    await _mediator.Send(sendEmailCommand);

                    NotificationCommand sendEmployeeNotificationCommand = new NotificationCommand
                    {
                        EmployeeIds = new List<int> { record.EmployeeId },
                        Message = record.InsideDuration < minWorkDuration
                            ? employeeIncompleteMessage
                            : employeeMessage
                    };
                    await _mediator.Send(sendEmployeeNotificationCommand);

                    if (record.InsideDuration < minWorkDuration)
                    {
                        Employee? employee = await _context.Employees
                            .Where(x => x.Id == record.EmployeeId)
                            .FirstOrDefaultAsync();

                        List<int> managerIds = await _managerIdFinder.FindManagerIdsAsync(employee?.ManagerId);

                        if (managerIds.Any())
                        {
                            string managerMessage = $"Employee {record.Employee.FirstName} {record.Employee.LastName} has worked {record.InsideDuration / 60} hours today, which is below the minimum required hours.";

                            NotificationCommand sendManagerNotificationCommand = new NotificationCommand
                            {
                                EmployeeIds = managerIds,
                                Message = managerMessage
                            };

                            await _mediator.Send(sendManagerNotificationCommand);
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}