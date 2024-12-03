using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.Requests.Commands.Attendence.SendNotification;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Attendence.Service
{
    public class DailyNotificationService(WolfDenContext context, IMediator mediator)
    {
        private readonly WolfDenContext _context = context;
        private readonly IMediator _mediator = mediator;
        public async Task SendNotificationsAsync()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1);
            int minWorkDuration = 360;

            List<DailyAttendence> attendanceRecords = await _context.DailyAttendence
                .Include(x => x.Employee)
                .Where(x => x.Date == today)
                .ToListAsync();

            List<LeaveRequest> leaveRequests = await _context.LeaveRequests
                          .Where(x => x.LeaveRequestStatusId == LeaveRequestStatus.Approved && x.HalfDay == true && x.FromDate == today)
                          .ToListAsync();

            Dictionary<int, LeaveRequest> leaveDictionary = leaveRequests.ToDictionary(x => x.EmployeeId);

            foreach (DailyAttendence record in attendanceRecords)
            {
                LeaveRequest? halfDay = leaveDictionary.GetValueOrDefault(record.EmployeeId);

                if (halfDay is not null)
                {
                    minWorkDuration = minWorkDuration / 2;
                }

                string employeeMessage = $"Your working duration for {today} is {record.InsideDuration / 60} hours and {record.InsideDuration % 60} minutes.";
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
                    Employee? employee = await _context.Employees
                        .Where(x => x.Id == record.EmployeeId)
                        .FirstOrDefaultAsync();
                    
                    List<int> managerIds = await FindManagerIdsAsync(employee.ManagerId);
                 
                    foreach (int managerId in managerIds)
                    {
                        string managerMessage = $"Employee {record.Employee.FirstName} {record.Employee.LastName} has worked {record.InsideDuration / 60} hours today, which is below the minimum required hours.";

                        NotificationCommand sendManagerNotificationCommand = new NotificationCommand
                        {
                            EmployeeId = managerId,
                            Message = managerMessage
                        };

                        await _mediator.Send(sendManagerNotificationCommand);
                    }
                }
            }
        }
        private async Task<List<int>> FindManagerIdsAsync(int? managerId)
        {
            List<int> managerIds = new List<int>();
            if (managerId is null)
                return managerIds;

            Employee? manager = await _context.Employees.FindAsync(managerId);
            if (manager is not null)
            {
                managerIds.Add(manager.Id);
                List<int> higherManagerIds = await FindManagerIdsAsync(manager.ManagerId);
                managerIds.AddRange(higherManagerIds);
            }
            return managerIds;
        }
    }
}