using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Application.Methods.LeaveManagement;
using WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequestDays;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;
using static WolfDen.Domain.Enums.EmployeeEnum;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.AddLeaveRequestForEmployeeByAdmin
{
    public class AddLeaveRequestForEmployeeByAdminHandler(WolfDenContext context, IMediator mediator) : IRequestHandler<AddLeaveRequestForEmployeeByAdmin, bool>
    {
        private readonly WolfDenContext _context= context;
        private readonly IMediator _mediator = mediator;
        public async Task<bool> Handle(AddLeaveRequestForEmployeeByAdmin request, CancellationToken cancellationToken)
        {
            Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeCode == request.EmployeeCode, cancellationToken);
            if (employee == null)
            {
                throw new InvalidOperationException($"No Such Employee");
            }
            LeaveBalance leaveBalance = await _context.LeaveBalances.FirstOrDefaultAsync(x => x.EmployeeId == employee.Id && x.TypeId == request.TypeId, cancellationToken);
            LeaveType leaveType = await _context.LeaveType.FirstOrDefaultAsync(x => x.Id == request.TypeId);
            LeaveSetting leaveSetting = await _context.LeaveSettings.FirstOrDefaultAsync();
            if (leaveType == null)
            {
                throw new InvalidOperationException($"No Such Leave Type");
            }
            

            if (leaveBalance == null)
            {
                throw new InvalidOperationException($"Leave balance not found for Employee ID {employee.FirstName} and Type ID {leaveType.TypeName}.");
            }
            if (leaveSetting == null)
            {
                throw new InvalidOperationException($"Leave Setting Not found");
            }


            decimal virtualLeaveCountWithoutHalfDay = await _context.LeaveRequestDays.Where(x => x.LeaveRequest.EmployeeId == employee.Id && x.LeaveRequest.TypeId == request.TypeId && x.LeaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Open && x.LeaveRequest.HalfDay != true).CountAsync(cancellationToken);
            decimal virtualLeaveCountWithHalfDay = await _context.LeaveRequestDays.Where(x => x.LeaveRequest.EmployeeId == employee.Id && x.LeaveRequest.TypeId == request.TypeId && x.LeaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Open && x.LeaveRequest.HalfDay == true).CountAsync(cancellationToken);
            decimal virtualBalance = (leaveBalance.Balance + leaveSetting.MaxNegativeBalanceLimit) - (virtualLeaveCountWithoutHalfDay + (virtualLeaveCountWithHalfDay / 2));
            decimal days = 0;
            List<DateOnly> dates = new List<DateOnly>();
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

            CalculateLeaveDays calculateLeaveDays = new CalculateLeaveDays(_context);
            bool sandwich = leaveType.Sandwich ?? false;
            LeaveDaysResultDto leaveDaysResultDto = await calculateLeaveDays.LeaveDays(request.FromDate, request.ToDate, sandwich);
            days = request.HalfDay == true ? (leaveDaysResultDto.DaysCount / 2) : leaveDaysResultDto.DaysCount;
            dates = leaveDaysResultDto.ValidDate;
            bool dateCheck = await _context.LeaveRequestDays
                .Where(x => dates.Contains(x.LeaveDate) && x.LeaveRequest.TypeId == request.TypeId && x.LeaveRequest.EmployeeId == employee.Id &&
                            (x.LeaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Open ||
                                x.LeaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Approved))
                .AnyAsync(cancellationToken);

            if (!dateCheck)
            {
                if (employee.Gender.HasValue && ((employee.Gender == Gender.Male && leaveType.LeaveCategoryId != LeaveCategory.Maternity) || (employee.Gender == Gender.Female && leaveType.LeaveCategoryId != LeaveCategory.Paternity)))
                {
                    if (leaveType.LeaveCategoryId == LeaveCategory.WorkFromHome && currentDate < request.FromDate)
                    {
                        return await AddLeave();
                    }

                    else if (currentDate.DayNumber < request.FromDate.DayNumber)
                    {
                        if (leaveType.LeaveCategoryId != LeaveCategory.BereavementLeave)
                        {
                            if ((leaveBalance.Balance + leaveSetting.MaxNegativeBalanceLimit) >= days || virtualBalance >= days)
                            {
                                if (leaveType.LeaveCategoryId.HasValue && (leaveType.LeaveCategoryId == LeaveCategory.RestrictedHoliday))
                                {
                                    int restrictedHolidayCount = await _context.Holiday.Where(x => x.Type == AttendanceStatus.RestrictedHoliday && x.Date >= request.FromDate && x.Date <= request.ToDate).CountAsync();
                                    if (days == restrictedHolidayCount)
                                    {
                                        return await AddLeave();
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException($"Selected Day Do not Contain Restricted Holiday");
                                    }
                                }
                                else
                                {
                                    return await AddLeave();
                                }
                            }
                            else
                            {
                                return await Balance(leaveBalance.Balance + leaveSetting.MaxNegativeBalanceLimit,leaveType.TypeName);
                            }
                        }
                        else
                        {
                            throw new Exception($"{leaveType.TypeName} can not be applied in advance");
                        }
                    }
                    else
                    {
                        if ((leaveBalance.Balance + leaveSetting.MaxNegativeBalanceLimit) >= days || virtualBalance >= days)
                        {
                            if (leaveType.LeaveCategoryId == LeaveCategory.BereavementLeave)
                            {
                                return await AddLeave();
                            }
                            else if (leaveType.LeaveCategoryId == LeaveCategory.PrivilegeLeave || leaveType.LeaveCategoryId == LeaveCategory.CasualLeave)
                            {
                                LeaveType EmergencyLeave = await _context.LeaveType.Where(x => x.LeaveCategoryId == LeaveCategory.EmergencyLeave).FirstOrDefaultAsync(cancellationToken);
                                LeaveBalance leaveBalance2 = await _context.LeaveBalances.FirstOrDefaultAsync(x => x.EmployeeId == employee.Id && x.TypeId == EmergencyLeave.Id, cancellationToken);
                                decimal EmergencyVirtualLeaveCountWithOutHalfDay = await _context.LeaveRequestDays.Where(x => x.LeaveRequest.EmployeeId == employee.Id && x.LeaveRequest.TypeId == request.TypeId && x.LeaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Open && x.LeaveRequest.ApplyDate >= x.LeaveRequest.FromDate && x.LeaveRequest.HalfDay != true).CountAsync(cancellationToken);
                                decimal EmergencyVirtualLeaveCountWithHalfDay = await _context.LeaveRequestDays.Where(x => x.LeaveRequest.EmployeeId == employee.Id && x.LeaveRequest.TypeId == request.TypeId && x.LeaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Open && x.LeaveRequest.ApplyDate >= x.LeaveRequest.FromDate && x.LeaveRequest.HalfDay == true).CountAsync(cancellationToken);
                                decimal EmergencyVirtualBalance = (leaveBalance2.Balance + leaveSetting.MaxNegativeBalanceLimit) - (virtualLeaveCountWithoutHalfDay + (virtualLeaveCountWithHalfDay / 2));
                                if ((leaveBalance2.Balance + leaveSetting.MaxNegativeBalanceLimit) >= days && EmergencyVirtualBalance >= days)
                                {
                                    return await AddLeave();
                                }
                                else
                                {
                                    return await Balance(leaveBalance2.Balance + leaveSetting.MaxNegativeBalanceLimit,EmergencyLeave.TypeName);
                                }

                            }
                            else
                            {
                                throw new InvalidOperationException($"Applying Leave For Previous Day is only Possible for Emergency Leave And Bereavement Leave ");
                            }

                        }
                        else
                        {
                            return await Balance(leaveBalance.Balance + leaveSetting.MaxNegativeBalanceLimit,leaveType.TypeName);
                        }
                    }

                }
                else
                {
                    if (!employee.Gender.HasValue)
                    {
                        throw new InvalidOperationException($"Complete Profile Details Before Applying Leave.Mainly Gender");

                    }
                    throw new InvalidOperationException($"The Leave  Applied is gender Specific And You Cannot Apply For {leaveType.TypeName} for {employee.FirstName} {employee.LastName}");
                }
            }
            else
            {
                throw new InvalidOperationException($"One Of the Date in Applied Dates is Already Applied");
            }

            

            async Task<bool> AddLeave()
            {   if(days > 0)
                {
                    LeaveRequest leaveRequest = new LeaveRequest(employee.Id, request.TypeId, request.HalfDay, request.FromDate, request.ToDate, currentDate, LeaveRequestStatus.Open, request.Description, request.AdminId);
                    _context.LeaveRequests.Add(leaveRequest);
                    await _context.SaveChangesAsync(cancellationToken);
                    AddLeaveRequestDayCommand addLeaveRequestDayCommand = new AddLeaveRequestDayCommand();
                    addLeaveRequestDayCommand.LeaveRequestId = leaveRequest.Id;
                    addLeaveRequestDayCommand.Date = dates;
                    return await _mediator.Send(addLeaveRequestDayCommand, cancellationToken);

                }
                else
                {
                    throw new InvalidOperationException("Total leave Days are zero");
                }
                

            }

            async Task<bool> Balance(decimal balance,string name)
            {
                if (balance < days)
                {
                    throw new InvalidOperationException($"No Sufficient Leave for type {name}, including Negative Leaves");
                }
                else
                {
                    throw new InvalidOperationException($"Revoke or edit existing {name}. All Balances are taken by applied leaves, including Negative Leaves");
                }
            }

        }
    }
}
