using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.Methods.LeaveManagement;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.AddLeaveRequest
{
    public class AddLeaveRequestCommandHandler(WolfDenContext context) : IRequestHandler<AddLeaveRequestCommand, bool>
    {
        private readonly WolfDenContext _context = context;

        public async Task<bool> Handle(AddLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            LeaveBalance leaveBalance = await _context.LeaveBalances.FirstOrDefaultAsync(x => x.EmployeeId == request.EmpId && x.TypeId == request.TypeId, cancellationToken);
            Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == request.EmpId, cancellationToken);
            LeaveType leaveType = await _context.LeaveType.FirstOrDefaultAsync(x => x.Id == request.TypeId);
            if (leaveType == null)
            {
                throw new InvalidOperationException($"No Such Leave Type");
            }
            if (employee == null)
            {
                throw new InvalidOperationException($"No Such Employee");
            }

            if (leaveBalance == null)
            {
                throw new InvalidOperationException($"Leave balance not found for Employee ID {employee.FirstName} and Type ID {leaveType.TypeName}.");
            }

            int days = 0;
            bool check = false;
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

            CalculateLeaveDays calculateLeaveDays = new CalculateLeaveDays(_context);
            bool sandwich = leaveType.Sandwich ?? false;
            days = await calculateLeaveDays.LeaveDays(request.FromDate, request.ToDate, sandwich);

            if (leaveType.LeaveCategoryId == LeaveCategory.WorkFromHome)
            {
                if (leaveType.DaysCheck.HasValue && (days > leaveType.DaysCheck))
                {
                    if (request.FromDate.DayNumber - currentDate.DayNumber >= leaveType.DaysCheckMore)
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


                    }
                    else
                    {
                        throw new InvalidOperationException($"For more than {leaveType.DaysCheck} {leaveType.TypeName}, leave FromDate should be atleast {leaveType.DaysCheckMore} days before. ");
                    }
                }
                else if (leaveType.DaysCheck.HasValue && (days <= leaveType.DaysCheck))
                {
                    if (request.FromDate.DayNumber - currentDate.DayNumber >= leaveType.DaysCheckEqualOrLess)
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
                    }
                    else
                    {
                        throw new InvalidOperationException($"For less than or {leaveType.DaysCheck} {leaveType.TypeName}, leave should be applied atleast {leaveType.DaysCheckEqualOrLess} days before. ");
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Unexpected");
                }
            }


           else if (currentDate.DayNumber < request.FromDate.DayNumber)
            {
                if (leaveType.LeaveCategoryId != LeaveCategory.BereavementLeave)
                {
                    if (leaveBalance.Balance >= days)
                    {
                        if (!request.HalfDay.HasValue || request.HalfDay == false)
                        {
                            if (leaveType.DaysCheck.HasValue)
                            {
                                if (days > leaveType.DaysCheck)
                                {
                                    if (request.FromDate.DayNumber - currentDate.DayNumber >= leaveType.DaysCheckMore)
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
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException($"For more than {leaveType.DaysCheck} {leaveType.TypeName}, leave FromDate should be atleast {leaveType.DaysCheckMore} days before. ");
                                    }
                                }
                                else if (days <= leaveType.DaysCheck)
                                {
                                    if (request.FromDate.DayNumber - currentDate.DayNumber >= leaveType.DaysCheckEqualOrLess)
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
                                                    throw new InvalidOperationException($"Minimum Duty Days of {leaveType.DutyDaysRequired} is Required for {leaveType.TypeName}  .");
                                                }

                                            }
                                            else
                                            {
                                                throw new InvalidOperationException($"Joining date Not assinged by HR. joining Date is required for {leaveType.TypeName}");
                                            }
                                        }
                                        else
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
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException($"For less than or {leaveType.DaysCheck} {leaveType.TypeName}, leave should be applied atleast {leaveType.DaysCheckEqualOrLess} days before. ");
                                    }
                                }
                                else
                                {
                                    throw new InvalidOperationException("Unexpected");
                                }
                            }
                            else
                            {
                                throw new InvalidOperationException("Unexpected");
                            }


                        }
                        else if (request.HalfDay == true)
                        {
                            if (leaveType.LeaveCategoryId == LeaveCategory.CasualLeave)
                            {
                                if (leaveType.DaysCheck.HasValue)
                                {
                                    if (days > leaveType.DaysCheck)
                                    {
                                        if (request.FromDate.DayNumber - currentDate.DayNumber >= leaveType.DaysCheckMore)
                                        {
                                            if (leaveType.DutyDaysRequired.HasValue)
                                            {
                                                if (employee.JoiningDate.HasValue)
                                                {
                                                    if (employee.JoiningDate.HasValue && (request.FromDate.DayNumber - employee.JoiningDate.Value.DayNumber >= leaveType.DutyDaysRequired))
                                                    {
                                                        return await AddLeave();
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
                                                return await AddLeave();
                                            }

                                        }
                                        else
                                        {
                                            throw new InvalidOperationException($"For more than {leaveType.DaysCheck} {leaveType.TypeName}, leave FromDate should be atleast {leaveType.DaysCheckMore} days before. ");
                                        }
                                    }
                                    else if (days <= leaveType.DaysCheck)
                                    {
                                        if (request.FromDate.DayNumber - currentDate.DayNumber >= leaveType.DaysCheckEqualOrLess)
                                        {
                                            if (leaveType.DutyDaysRequired.HasValue)
                                            {
                                                if (employee.JoiningDate.HasValue)
                                                {
                                                    if (employee.JoiningDate.HasValue && (request.FromDate.DayNumber - employee.JoiningDate.Value.DayNumber >= leaveType.DutyDaysRequired))
                                                    {
                                                        return await AddLeave();
                                                    }
                                                    else
                                                    {
                                                        throw new InvalidOperationException($"Minimum Duty Days of {leaveType.DutyDaysRequired} is Required for {leaveType.TypeName}  .");
                                                    }
                                                }
                                                else
                                                {
                                                    throw new InvalidOperationException($"Joining date Not assinged by HR. joining Date is required for {leaveType.TypeName}");
                                                }
                                            }
                                            else
                                            {
                                                return await AddLeave();
                                            }
                                        }
                                        else
                                        {
                                            throw new InvalidOperationException($"For less than or {leaveType.DaysCheck} {leaveType.TypeName}, leave should be applied atleast {leaveType.DaysCheckEqualOrLess} days before. ");
                                        }
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException("Unexpected");
                                    }
                                }
                                else
                                {
                                    throw new InvalidOperationException("Unexpected");
                                }
                            }
                            else
                            {
                                throw new InvalidOperationException($"Half Day Not Applicable for {leaveType.TypeName}");
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException("Unexpected");
                        }

                    }
                    else
                    {
                        throw new InvalidOperationException($"No Sufficient Leave for type {leaveType.TypeName}");
                    }
                }
                else
                {
                    throw new Exception($"{leaveType.TypeName} can not be applied in advance");
                }
                
            }
            else if (currentDate.DayNumber >= request.FromDate.DayNumber)
            {
                if (leaveBalance.Balance >= days)
                {
                    if (leaveType.LeaveCategoryId == LeaveCategory.BereavementLeave)
                    {
                        return await AddLeave();
                    }
                    else if (leaveType.LeaveCategoryId == LeaveCategory.PrivilegeLeave || leaveType.LeaveCategoryId == LeaveCategory.CasualLeave)
                    {
                        int leaveId = await _context.LeaveType.Where(x => x.LeaveCategoryId == LeaveCategory.EmergencyLeave).Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);
                        LeaveBalance leaveBalance2 = await _context.LeaveBalances.FirstOrDefaultAsync(x => x.EmployeeId == request.EmpId && x.TypeId == leaveId, cancellationToken);
                        if (leaveBalance2.Balance >= days)
                        {
                            return await AddLeave();
                        }
                        else
                        {
                            throw new InvalidOperationException($"No Sufficient Leave for type {LeaveCategory.EmergencyLeave}");
                        }

                    }
                    else
                    {
                        throw new InvalidOperationException($"Applying Leave For Previous Day is only Possible for {LeaveCategory.EmergencyLeave} And {LeaveCategory.BereavementLeave} ");
                    }

                }
                else
                {
                    throw new InvalidOperationException($"No Sufficient Leave for type {leaveType.TypeName}");
                }

            }
            else
            {
                throw new InvalidOperationException($"Unexpected Error");
            }

            async Task<bool> AddLeave()
            {
                LeaveRequest leaveRequest = new LeaveRequest(request.EmpId, request.TypeId, request.HalfDay, request.FromDate, request.ToDate, currentDate, LeaveRequestStatus.Open, request.Description);
                _context.LeaveRequests.Add(leaveRequest);
                int saveResult = await _context.SaveChangesAsync(cancellationToken);
                return saveResult > 0;
            }

        }
    }
}
