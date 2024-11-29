using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Application.Method.LeaveManagement;
using WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequestDays;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;
using static WolfDen.Domain.Enums.EmployeeEnum;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.EditLeaveRequest
{
    public class EditLeaveRequestCommandHandler(WolfDenContext context, IMediator mediator) : IRequestHandler<EditLeaveRequestCommand, bool>
    {
        private readonly WolfDenContext _context = context;
        private readonly IMediator _mediator = mediator;
        public async Task<bool> Handle(EditLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            LeaveRequest leaveRequest = await _context.LeaveRequests.Where(x => x.Id == request.LeaveRequestId && x.RequestedBy == request.EmpId).FirstOrDefaultAsync();
            if (leaveRequest is null)
            {
                throw new Exception($"No Such Leave Request");
            }
            if (leaveRequest.EmployeeId == request.EmpId && leaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Open)
            {

                LeaveBalance leaveBalance = await _context.LeaveBalances.FirstOrDefaultAsync(x => x.EmployeeId == request.EmpId && x.TypeId == request.TypeId, cancellationToken);
                Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == request.EmpId, cancellationToken);
                LeaveType leaveType = await _context.LeaveType.FirstOrDefaultAsync(x => x.Id == request.TypeId);
                if (leaveType is null)
                {
                    throw new InvalidOperationException($"No Such Leave Type");
                }
                if (employee is null)
                {
                    throw new InvalidOperationException($"No Such Employee");
                }

                if (leaveBalance is null)
                {
                    throw new InvalidOperationException($"Leave balance not found for Employee ID {employee.FirstName} and Type ID {leaveType.TypeName}.");
                }

                decimal virtualLeaveCountWithoutHalfDay = await _context.LeaveRequestDays.Where(x => x.LeaveRequest.EmployeeId == request.EmpId && x.LeaveRequest.TypeId == request.TypeId && x.LeaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Open && x.LeaveRequest.HalfDay != true && x.LeaveRequestId != request.LeaveRequestId).CountAsync(cancellationToken);
                decimal virtualLeaveCountWithHalfDay = await _context.LeaveRequestDays.Where(x => x.LeaveRequest.EmployeeId == request.EmpId && x.LeaveRequest.TypeId == request.TypeId && x.LeaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Open && x.LeaveRequest.HalfDay == true && x.LeaveRequestId != request.LeaveRequestId).CountAsync(cancellationToken);
                decimal virtualBalance = leaveBalance.Balance - (virtualLeaveCountWithoutHalfDay + (virtualLeaveCountWithHalfDay / 2));
                decimal days = 0;
                List<DateOnly> dates = new List<DateOnly>();

                CalculateLeaveDays calculateLeaveDays = new CalculateLeaveDays(_context);
                bool sandwich = leaveType.Sandwich ?? false;
                LeaveDaysResultDto leaveDaysResultDto = await calculateLeaveDays.LeaveDays(request.FromDate, request.ToDate, sandwich);
                days = leaveDaysResultDto.DaysCount;
                dates = leaveDaysResultDto.ValidDate;
                days = request.HalfDay == true ? (leaveDaysResultDto.DaysCount / 2) : leaveDaysResultDto.DaysCount;
                bool dateCheck = await _context.LeaveRequestDays
                    .Where(x => dates.Contains(x.LeaveDate) && x.LeaveRequest.TypeId == request.TypeId && x.LeaveRequest.EmployeeId == request.EmpId && x.LeaveRequestId != request.LeaveRequestId &&
                                (x.LeaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Open ||
                                    x.LeaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Approved)) 
                    .AnyAsync(cancellationToken);

                if (!dateCheck)
                {
                    if (employee.Gender.HasValue && ((employee.Gender == Gender.Male && leaveType.LeaveCategoryId != LeaveCategory.Maternity) || (employee.Gender == Gender.Female && leaveType.LeaveCategoryId != LeaveCategory.Paternity)))
                    {
                        if (leaveType.LeaveCategoryId == LeaveCategory.WorkFromHome && leaveRequest.ApplyDate < request.FromDate)
                        {
                            if (leaveType.DaysCheck.HasValue && (days > leaveType.DaysCheck))
                            {
                                if (request.FromDate.DayNumber - leaveRequest.ApplyDate.DayNumber >= leaveType.DaysCheckMore)
                                {
                                    return await WorkFromHomeCheck();

                                }
                                else
                                {
                                    throw new InvalidOperationException($"For more than {leaveType.DaysCheck} {leaveType.TypeName}, leave FromDate should be atleast {leaveType.DaysCheckMore} days before. ");
                                }
                            }
                            else if (leaveType.DaysCheck.HasValue && (days <= leaveType.DaysCheck))
                            {
                                if (request.FromDate.DayNumber - leaveRequest.ApplyDate.DayNumber >= leaveType.DaysCheckEqualOrLess)
                                {
                                    return await WorkFromHomeCheck();
                                }
                                else
                                {
                                    throw new InvalidOperationException($"For less than or {leaveType.DaysCheck} {leaveType.TypeName}, leave should be applied atleast {leaveType.DaysCheckEqualOrLess} days before. ");
                                }
                            }
                            else
                            {
                                throw new InvalidOperationException($"Days Check Not Assinged");
                            }
                        }


                        else if (leaveRequest.ApplyDate.DayNumber < request.FromDate.DayNumber && leaveType.LeaveCategoryId != LeaveCategory.BereavementLeave)
                        {
                            if (leaveBalance.Balance >= days && virtualBalance >= days)
                            {
                                if (!request.HalfDay.HasValue || request.HalfDay == false)
                                {
                                    if (leaveType.DaysCheck.HasValue)
                                    {
                                        if (days > leaveType.DaysCheck)
                                        {
                                            if (request.FromDate.DayNumber - leaveRequest.ApplyDate.DayNumber >= leaveType.DaysCheckMore)
                                            {
                                                return await CheckOne();
                                            }
                                            else
                                            {
                                                throw new InvalidOperationException($"For more than {leaveType.DaysCheck} {leaveType.TypeName}, leave FromDate should be atleast {leaveType.DaysCheckMore} days before. ");
                                            }
                                        }
                                        else
                                        {
                                            if (request.FromDate.DayNumber - leaveRequest.ApplyDate.DayNumber >= leaveType.DaysCheckEqualOrLess)
                                            {
                                                return await CheckOne();
                                            }
                                            else
                                            {
                                                throw new InvalidOperationException($"For less than or {leaveType.DaysCheck} {leaveType.TypeName}, leave should be applied atleast {leaveType.DaysCheckEqualOrLess} days before. ");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException("Days Check Not Assinged");
                                    }


                                }
                                else
                                {
                                    if (leaveType.IsHalfDayAllowed == true)
                                    {
                                        if (leaveDaysResultDto.DaysCount == 1)
                                        {
                                            if (leaveType.DaysCheck.HasValue)
                                            {
                                                if (days > leaveType.DaysCheck)
                                                {
                                                    if (request.FromDate.DayNumber - leaveRequest.ApplyDate.DayNumber >= leaveType.DaysCheckMore)
                                                    {
                                                        return await CheckOne();

                                                    }
                                                    else
                                                    {
                                                        throw new InvalidOperationException($"For more than {leaveType.DaysCheck} {leaveType.TypeName}, leave FromDate should be atleast {leaveType.DaysCheckMore} days before Apply Date. ");
                                                    }
                                                }
                                                else
                                                {
                                                    if (request.FromDate.DayNumber - leaveRequest.ApplyDate.DayNumber >= leaveType.DaysCheckEqualOrLess)
                                                    {
                                                        return await CheckOne();
                                                    }
                                                    else
                                                    {
                                                        throw new InvalidOperationException($"For less than {leaveType.DaysCheck} or {leaveType.DaysCheck} {leaveType.TypeName}, leave should be applied atleast {leaveType.DaysCheckEqualOrLess} days before. ");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                throw new InvalidOperationException("Days Check Not Assinged");
                                            }
                                        }
                                        else
                                        {
                                            throw new InvalidOperationException("Half Day can Only Be Applied For One Day");
                                        }
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException($"Half Day Not Applicable for {leaveType.TypeName}");
                                    }
                                }

                            }
                            else
                            {
                                return await Balance(leaveBalance.Balance,leaveType.TypeName);
                            }

                        }
                        else if (leaveRequest.ApplyDate.DayNumber >= request.FromDate.DayNumber)
                        {
                            if (leaveBalance.Balance >= days && virtualBalance >= days)
                            {
                                if (!request.HalfDay.HasValue || request.HalfDay == false)
                                {
                                    return await PreviousDayLeaves();
                                }
                                else
                                {
                                    if (leaveType.IsHalfDayAllowed == true)
                                    {
                                        if (leaveDaysResultDto.DaysCount == 1)
                                        {
                                            return await PreviousDayLeaves();
                                        }
                                        else
                                        {
                                            throw new InvalidOperationException("Half Day can Only Be Applied For One Day");
                                        }
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException($"Half Day Not Applicable for {leaveType.TypeName}");
                                    }

                                }


                            }
                            else
                            {
                                return await Balance(leaveBalance.Balance,leaveType.TypeName);
                            }

                        }
                        else
                        {
                            throw new InvalidOperationException($"Current Leave Cannot Be Applied for Selected Dates");
                        }
                    }
                    else
                    {
                        if (!employee.Gender.HasValue)
                        {
                            throw new InvalidOperationException($"Complete Profile Details Before Applying Leave.Mainly Gender");

                        }
                        throw new InvalidOperationException($"The Leave You Applied is gender Specific And You Cannot Apply For {leaveType.TypeName}");
                    }
                }
                else
                {
                    throw new InvalidOperationException($"One Of the Date in Applied Dates is Already Applied");
                }





                async Task<bool> EditLeaveRequest()
                {
                    if (days > 0)
                    {
                        leaveRequest.EditLeave(request.TypeId, request.HalfDay, request.FromDate, request.ToDate, LeaveRequestStatus.Open, request.Description);
                        _context.Update(leaveRequest);
                        List<LeaveRequestDay> leaveRequestDayList = await _context.LeaveRequestDays.Where(x => x.LeaveRequestId == request.LeaveRequestId).ToListAsync(cancellationToken);
                        _context.LeaveRequestDays.RemoveRange(leaveRequestDayList);
                        int saveResult = await _context.SaveChangesAsync(cancellationToken);
                        if (saveResult > 0)
                        {
                            AddLeaveRequestDayCommand addLeaveRequestDayCommand = new AddLeaveRequestDayCommand();
                            addLeaveRequestDayCommand.LeaveRequestId = leaveRequest.Id;
                            addLeaveRequestDayCommand.Date = dates;
                            return await _mediator.Send(addLeaveRequestDayCommand, cancellationToken);
                        }
                        else
                        {
                            throw new Exception("Error");
                        }
                    }
                    else
                    {
                        throw new Exception("Total Leaves Are 0");
                    }


                }

                async Task<bool> CheckOne()
                {
                    if (leaveType.DutyDaysRequired.HasValue)
                    {
                        if (employee.JoiningDate.HasValue)
                        {
                            if (employee.JoiningDate.HasValue && (request.FromDate.DayNumber - employee.JoiningDate.Value.DayNumber >= leaveType.DutyDaysRequired))
                            {
                                if (leaveType.LeaveCategoryId.HasValue && (leaveType.LeaveCategoryId == LeaveCategory.RestrictedHoliday))
                                {
                                    int restrictedHolidayCount = await _context.Holiday.Where(x => x.Type == AttendanceStatus.RestrictedHoliday && x.Date >= request.FromDate && x.Date <= request.ToDate).CountAsync();
                                    if (days == restrictedHolidayCount)
                                    {
                                        return await EditLeaveRequest();
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException($"Selected Day Do not Contain Restricted Holiday");
                                    }
                                }
                                else
                                {
                                    return await EditLeaveRequest();
                                }
                            }
                            else
                            {
                                throw new InvalidOperationException($"Minimum Duty Days of {leaveType.DutyDaysRequired} is Required for {leaveType.TypeName} .");
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException($"Joining date Not assinged by HR.");
                        }
                    }
                    else
                    {
                        if (leaveType.LeaveCategoryId.HasValue && (leaveType.LeaveCategoryId == LeaveCategory.RestrictedHoliday))
                        {
                            int restrictedHolidayCount = await _context.Holiday.Where(x => x.Type == AttendanceStatus.RestrictedHoliday && x.Date >= request.FromDate && x.Date <= request.ToDate).CountAsync();
                            if (days == restrictedHolidayCount)
                            {
                                return await EditLeaveRequest();
                            }
                            else
                            {
                                throw new InvalidOperationException($"Selected Day Do not Contain Restricted Holiday");
                            }
                        }
                        else
                        {
                            return await EditLeaveRequest();
                        }
                    }
                }


                async Task<bool> WorkFromHomeCheck()
                {
                    if (leaveType.DutyDaysRequired.HasValue)
                    {
                        if (employee.JoiningDate.HasValue)
                        {
                            if ((request.FromDate.DayNumber - employee.JoiningDate.Value.DayNumber >= leaveType.DutyDaysRequired))
                            {
                                return await EditLeaveRequest();
                            }
                            else
                            {
                                throw new InvalidOperationException($"Minimum Duty Days of {leaveType.DutyDaysRequired} is Required for {leaveType.TypeName} .");
                            }

                        }
                        else
                        {
                            throw new InvalidOperationException($"Joining date Not assinged by HR.");
                        }
                    }
                    else
                    {
                        return await EditLeaveRequest();
                    }
                }



                async Task<bool> PreviousDayLeaves()
                {
                    if (leaveType.LeaveCategoryId == LeaveCategory.BereavementLeave)
                    {
                        return await EditLeaveRequest();
                    }
                    else if (leaveType.LeaveCategoryId == LeaveCategory.PrivilegeLeave || leaveType.LeaveCategoryId == LeaveCategory.CasualLeave)
                    {
                        LeaveType EmergencyLeave = await _context.LeaveType.Where(x => x.LeaveCategoryId == LeaveCategory.EmergencyLeave).FirstOrDefaultAsync(cancellationToken);
                        LeaveBalance leaveBalance2 = await _context.LeaveBalances.FirstOrDefaultAsync(x => x.EmployeeId == request.EmpId && x.TypeId == EmergencyLeave.Id, cancellationToken);
                        decimal EmergencyVirtualLeaveCountWithOutHalfDay = await _context.LeaveRequestDays.Where(x => x.LeaveRequest.EmployeeId == request.EmpId && x.LeaveRequest.TypeId == request.TypeId && x.LeaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Open && x.LeaveRequest.ApplyDate >= x.LeaveRequest.FromDate && x.LeaveRequest.HalfDay != true && x.LeaveRequestId != request.LeaveRequestId).CountAsync(cancellationToken);
                        decimal EmergencyVirtualLeaveCountWithHalfDay = await _context.LeaveRequestDays.Where(x => x.LeaveRequest.EmployeeId == request.EmpId && x.LeaveRequest.TypeId == request.TypeId && x.LeaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Open && x.LeaveRequest.ApplyDate >= x.LeaveRequest.FromDate && x.LeaveRequest.HalfDay == true && x.LeaveRequestId != request.LeaveRequestId).CountAsync(cancellationToken);
                        decimal EmergencyVirtualBalance = leaveBalance.Balance - (virtualLeaveCountWithoutHalfDay + (virtualLeaveCountWithHalfDay / 2));
                        if (leaveBalance2.Balance >= days && EmergencyVirtualBalance >= days)
                        {
                            return await EditLeaveRequest();
                        }
                        else
                        {
                            return await Balance(leaveBalance2.Balance, EmergencyLeave.TypeName);
                        }

                    }
                    else
                    {
                        throw new InvalidOperationException($"Applying Leave For Previous Day is only Possible for Emergency  And .Bereavement Leave . And Half Day is not Applicable ");
                    }
                }

                async Task<bool> Balance(decimal balance, string name)
                {
                    if (balance < days)
                    {
                        throw new InvalidOperationException($"No Sufficient Leave for type {name}");
                    }
                    else
                    {
                        throw new InvalidOperationException($"Revoke or edit existing {name}. All Balances are taken by applied leaves");
                    }
                }
            }
            else
            {
                throw new Exception($"You cannot Edit This Leave");
            }
        }
    }
}

