using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.Helper.LeaveManagement;
using WolfDen.Application.Requests.Commands.Attendence.SendNotification;
using WolfDen.Application.Requests.Queries.Employees.GetEmployeeTeam;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.ApproveOrRejectLeaveRequest
{
    public class ApproveOrRejectLeaveRequestCommandHandler(WolfDenContext context, IMediator mediator, IConfiguration configuration, Email email) : IRequestHandler<ApproveOrRejectLeaveRequestCommand, bool>
    {
        private readonly WolfDenContext _context = context;
        private readonly IMediator _mediator = mediator;
        private readonly string _apiKey = configuration["BrevoApi:ApiKey"];
        private readonly string _senderEmail = configuration["BrevoApi:SenderEmail"];
        private readonly string _senderName = configuration["BrevoApi:SenderName"];
        private readonly Email _email = email;
        public async Task<bool> Handle(ApproveOrRejectLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            LeaveRequest leaveRequest = await _context.LeaveRequests.Where(x => x.Id == request.LeaveRequestId && x.LeaveRequestStatusId == LeaveRequestStatus.Open).FirstOrDefaultAsync(cancellationToken);
            LeaveType leaveType1 = await _context.LeaveType.Where(x => x.Id == leaveRequest.TypeId).FirstOrDefaultAsync(cancellationToken);  
            Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == leaveRequest.EmployeeId,cancellationToken);
            Employee manager = await _context.Employees.FirstOrDefaultAsync(x => x.Id == request.SuperiorId,cancellationToken); 
            List<EmployeeHierarchyDto> employeeHierarchyDto = new List<EmployeeHierarchyDto>();
            GetEmployeeTeamQuery employeeTeamQuery = new GetEmployeeTeamQuery
            {
                Id = request.SuperiorId,
                Hierarchy=true
            };

            employeeHierarchyDto = await _mediator.Send(employeeTeamQuery, cancellationToken);

            if (employeeHierarchyDto is null)
            {
                throw new Exception("No Subordinates");
            }

            List<int> listSubordinateEmployeeId = await GetAllChildIds(employeeHierarchyDto);

            if (listSubordinateEmployeeId.Contains(leaveRequest.EmployeeId))
            {
                if (request.statusId == LeaveRequestStatus.Rejected )
                {
                    leaveRequest.Reject(request.SuperiorId);
                    int result =  await _context.SaveChangesAsync(cancellationToken);
                    string[] receiver = { employee.Email };
                    string subject = $"Leave Rejected  {leaveRequest.FromDate} to {leaveRequest.ToDate}";
                    string statusMessage = $"Your Following Leave is {(request.statusId == LeaveRequestStatus.Approved ? "Approved" : "Rejected")}.";
                    await Mail(receiver, subject, statusMessage);
                    string message = $"Your Leave {leaveRequest.FromDate} to {leaveRequest.ToDate} is Rejected By {manager.FirstName} {manager.LastName} [Manager Code : {manager.EmployeeCode}]";
                    NotificationCommand command = new NotificationCommand
                    {
                        EmployeeId = employee.Id,
                        Message = message,
                    };
                    await _mediator.Send(command, cancellationToken);
                    return  result > 0;
                }
                else if (leaveType1.LeaveCategoryId != null && (request.statusId == LeaveRequestStatus.Approved && leaveRequest.LeaveType.LeaveCategoryId != LeaveCategory.WorkFromHome))
                {
                    leaveRequest.Approve(request.SuperiorId);
                    LeaveBalance leaveBalance = await _context.LeaveBalances.FirstOrDefaultAsync(x => x.TypeId == leaveRequest.TypeId && x.EmployeeId == leaveRequest.EmployeeId);

                    decimal daysCount = await _context.LeaveRequestDays.Where(x => x.LeaveRequestId == request.LeaveRequestId).CountAsync();

                    if (leaveRequest.HalfDay == true)
                    {
                        daysCount = daysCount - 0.5m;

                    }

                    if (leaveType1.LeaveCategoryId != null && ( leaveRequest.ApplyDate >= leaveRequest.FromDate && leaveRequest.LeaveType.LeaveCategoryId !=LeaveCategory.BereavementLeave))
                    {
                        LeaveType leaveType = await _context.LeaveType.FirstOrDefaultAsync(x => x.LeaveCategoryId == LeaveCategory.EmergencyLeave);
                        LeaveBalance leaveBalance2 = await _context.LeaveBalances.FirstOrDefaultAsync(x => x.TypeId == leaveType.Id && x.EmployeeId == leaveRequest.EmployeeId);


                        leaveBalance2.Balance = leaveBalance2.Balance - daysCount;
                        leaveBalance2.UpdateBalance(leaveBalance2.Balance);
                    }

                    leaveBalance.Balance = leaveBalance.Balance - daysCount;
                    leaveBalance.UpdateBalance(leaveBalance.Balance);
                    int result = await _context.SaveChangesAsync(cancellationToken);
                    string[] receiver = { employee.Email };
                    string subject = $"Leave Approved  {leaveRequest.FromDate} to {leaveRequest.ToDate}";
                    string statusMessage = $"Your Following Leave is {(request.statusId == LeaveRequestStatus.Approved ? "Approved" : "Rejected")}.";
                    await Mail(receiver, subject, statusMessage);
                    string message = $"Your Leave {leaveRequest.FromDate} to {leaveRequest.ToDate} is Approved By {manager.FirstName} {manager.LastName} [Manager Code : {manager.EmployeeCode}]";
                    NotificationCommand command = new NotificationCommand
                    {
                        EmployeeId = employee.Id,
                        Message = message,
                    };
                    await _mediator.Send(command, cancellationToken);
                    return result > 0;
                }
                else if(leaveType1.LeaveCategoryId != null && ( leaveRequest.LeaveType.LeaveCategoryId == LeaveCategory.WorkFromHome && request.statusId == LeaveRequestStatus.Approved))  
                {
                    leaveRequest.Approve(request.SuperiorId);
                    int result = await _context.SaveChangesAsync(cancellationToken);
                    string[] receiver = { employee.Email };
                    string subject = $"Leave Approved  {leaveRequest.FromDate} to {leaveRequest.ToDate}";
                    string statusMessage = $"Your Following Leave is {(request.statusId == LeaveRequestStatus.Approved ? "Approved" : "Rejected")}.";
                    await Mail(receiver, subject, statusMessage);
                    string message = $"Your Leave {leaveRequest.FromDate} to {leaveRequest.ToDate} is Approved By {manager.FirstName} {manager.LastName} [Manager Code : {manager.EmployeeCode}]";
                    NotificationCommand command = new NotificationCommand
                    {
                        EmployeeId = employee.Id,
                        Message = message,
                    };
                    await _mediator.Send(command, cancellationToken);
                    return result > 0;
                }
                else
                {
                    throw new Exception($"Wrong Approval Type or Leave category of Leave Type has no value");
                }

            }
            else
            {
                throw new Exception("You are not superior to the applied leave request employee");
            }

            async Task<List<int>> GetAllChildIds(List<EmployeeHierarchyDto> employees)
            {
                List<int> result = new List<int>();

                foreach (EmployeeHierarchyDto employee in employees)
                {
                    if (employee.Id != request.SuperiorId)
                        result.Add(employee.Id);

                    if (employee.Subordinates != null && employee.Subordinates.Count > 0)
                    {
                        List<int> childIds = await GetAllChildIds(employee.Subordinates);
                        result.AddRange(childIds);
                    }
                }

                return result;
            }

            async Task<bool> Mail(string[] receiver, string subject,string status )
            {
                string message = $@"
                                    <html>
                                    <body>

                                        <p>
                                           {status}
                                        </p>

                                        <table border='1' style='border-collapse: collapse; width: 100%;'>
                                            <tr>
                                                <th style='padding: 8px; text-align: left; border: 1px solid #ddd;'>Information</th> 
                                                <th style='padding: 8px; text-align: left; border: 1px solid #ddd;'>Value</th> 
                                            </tr>
                                            <tr>
                                                <td style='padding: 8px; border: 1px solid #ddd;'>Leave Type</td> 
                                                <td style='padding: 8px; border: 1px solid #ddd;'>{((leaveType1.LeaveCategoryId == LeaveCategory.PrivilegeLeave || (leaveType1.LeaveCategoryId == LeaveCategory.CasualLeave && leaveRequest.ApplyDate.DayNumber >= leaveRequest.FromDate.DayNumber)) ? "Emergency Leave" : leaveType1.TypeName)}</td>  
                                            </tr>
                                            <tr>
                                                <td style='padding: 8px; border: 1px solid #ddd;'>From Date</td> 
                                                <td style='padding: 8px; border: 1px solid #ddd;'>{leaveRequest.FromDate}</td> 
                                            </tr>
                                            <tr>
                                                <td style='padding: 8px; border: 1px solid #ddd;'>To Date</td> 
                                                <td style='padding: 8px; border: 1px solid #ddd;'>{leaveRequest.ToDate}</td> 
                                            </tr>
                                            <tr>
                                                <td style='padding: 8px; border: 1px solid #ddd;'>Leave Description</td> 
                                                <td style='padding: 8px; border: 1px solid #ddd;'>{leaveRequest.Description}</td> 
                                            </tr>
                                            <tr>
                                                <td style='padding: 8px; border: 1px solid #ddd;'>Processed By</td> 
                                                <td style='padding: 8px; border: 1px solid #ddd;'>{manager.FirstName} {manager.LastName}</td> 
                                            </tr>
                                             <tr>
                                                <td style='padding: 8px; border: 1px solid #ddd;'>Manager Code</td> 
                                                <td style='padding: 8px; border: 1px solid #ddd;'>{manager.EmployeeCode}</td> 
                                            </tr>
                                        </table>

                                    </body>
                                    </html>";
                return  _email.SendMail(_senderEmail, _senderName, receiver, message, subject, null);
            }

        }

    }

}
