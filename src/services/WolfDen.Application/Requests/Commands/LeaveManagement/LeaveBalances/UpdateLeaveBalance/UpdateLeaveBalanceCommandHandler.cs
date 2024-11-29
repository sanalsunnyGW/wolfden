using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveBalances.UpdateLeaveBalance
{
    public class UpdateLeaveBalanceCommandHandler(WolfDenContext context) : IRequestHandler<UpdateLeaveBalanceCommand, bool>
    {
        private readonly WolfDenContext _context = context;
        private int result;
        private DateOnly lastCreditedMonth;
        private int incrementValue;
        private DateOnly leaveUpdateLog;
        private int count = 0;
        private int balance;

        public async Task<bool> Handle(UpdateLeaveBalanceCommand command, CancellationToken cancellationToken)
        {
            LeaveSetting leaveSetting = await _context.LeaveSettings.FirstOrDefaultAsync(cancellationToken);

            List<Employee> employees = await _context.Employees.ToListAsync(cancellationToken);
            List<LeaveBalance> leaveBalanceList = await _context.LeaveBalances
                .Include(x => x.LeaveType)
                .Include(x => x.Employee)
                .ToListAsync(cancellationToken: cancellationToken);

            foreach (Employee emp in employees)  //fetching leave balance row for each leave type   //(updated fetching each employee to update their leave balance)
            {
                if (leaveBalanceList.Count() > 0)
                {
                    foreach (LeaveBalance leaveBalance in leaveBalanceList)
                    {
                        //if (emp.Id.Equals(leaveType.EmployeeId))                //to check whether that employee has entry leave balance table 
                        //{ count = count + 1; }
                        //else
                        //{ count = count; }

                        if (leaveBalance.Employee.Id == leaveBalance.EmployeeId)          //to update the each employee's leave balance   //to check whether the employee has the row leave balance for that leave type
                        {
                            count = count + 1;


                            if (count > 0)                          //if leavebalance entry already exists for a user  
                            {
                                LeaveIncrementLog leaveIncrementLog = await _context.LeaveIncrementLogs
                                .Where(x => x.LeaveBalanceId == leaveBalance.Id)
                                .OrderByDescending(x => x.Id)
                                .FirstOrDefaultAsync(cancellationToken);    //check whether same user is passed of details ALSO last log date is to be got not the first

                                if (leaveBalance.Employee.JoiningDate?.Month == DateTime.Now.Month && leaveBalance.Employee.JoiningDate?.Year == DateTime.Now.Year)        //for same month yr newly joined employee 
                                {
                                    if (leaveBalance.Employee.JoiningDate?.Day < leaveSetting.MinDaysForLeaveCreditJoining)        //if joined  before 15th(maxcredit..)
                                    {
                                        leaveBalance.Balance = 1;             //bcoz initially balance is 1  //only casual leave is credited on same month
                                    }
                                    else
                                    {
                                        leaveBalance.Balance = 0;
                                    }
                                    leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                    lastCreditedMonth = leaveBalance.Employee.JoiningDate.Value;
                                    incrementValue = 0;
                                }
                                else
                                {
                                    DateTime JoiningDateTime = leaveBalance.Employee.JoiningDate.Value.ToDateTime(TimeOnly.MinValue);

                                    if (DateTime.Now.Subtract(JoiningDateTime).Days >= leaveBalance.LeaveType.DutyDaysRequired)
                                    {
                                        decimal UpdateReq = DateTime.Now.Month - leaveIncrementLog.LastCreditedMonth.Month;
                                        //check whether the updated date is more than the increment gap mentioned

                                        if (LeaveIncrementGapMonth.Month == leaveBalance.LeaveType.IncrementGapId)   //monthly increment
                                        {
                                            int value = (int)Math.Floor(UpdateReq / 1);
                                            if (leaveBalance.LeaveType.CarryForward == true)
                                            {
                                                if ((leaveBalance.Balance + (leaveBalance.LeaveType.IncrementCount * value)) > leaveBalance.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit
                                                {
                                                    leaveBalance.Balance = (decimal)leaveBalance.LeaveType.CarryForwardLimit;       //set carryforward limit itself as balance
                                                    lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
                                                    leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                    incrementValue = 0;
                                                }
                                                else
                                                {
                                                    leaveBalance.Balance += (decimal)leaveBalance.LeaveType.IncrementCount * value;
                                                    lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
                                                    leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                    incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * value);
                                                }
                                            }
                                            else //if no carry forward allowed
                                            {
                                                if (DateTime.Now.Year == leaveIncrementLog.LastCreditedMonth.Year)   //check if balance updated in the same yr as that of last updated
                                                {
                                                    leaveBalance.Balance += (decimal)leaveBalance.LeaveType.IncrementCount * value;
                                                    lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
                                                    leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                    incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * value);
                                                }
                                                else    //if no carry forward & also balance updated in next/new year 
                                                {
                                                    leaveBalance.Balance = (int)(leaveBalance.LeaveType.IncrementCount * DateTime.Now.Month);
                                                    lastCreditedMonth = DateOnly.FromDateTime(DateTime.Now);
                                                    leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                    incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * DateTime.Now.Month);
                                                }
                                            }
                                        }


                                        else if (LeaveIncrementGapMonth.Quarter == leaveBalance.LeaveType.IncrementGapId)    //quarterly increment
                                        {
                                            int value = (int)Math.Floor(UpdateReq / 3);
                                            if (leaveBalance.LeaveType.CarryForward == true)
                                            {
                                                if ((leaveBalance.Balance + (leaveBalance.LeaveType.IncrementCount * value)) > leaveBalance.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit
                                                {
                                                    leaveBalance.Balance = (decimal)leaveBalance.LeaveType.CarryForwardLimit;       //set carryforward limit itself as balance
                                                    lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 3);
                                                    leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                    incrementValue = 0;
                                                }
                                                else
                                                {
                                                    leaveBalance.Balance += (decimal)leaveBalance.LeaveType.IncrementCount * value;
                                                    lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 3);
                                                    leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                    incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * value);
                                                }
                                            }
                                            else //if no carry forward allowed
                                            {
                                                if (DateTime.Now.Year == leaveIncrementLog.LastCreditedMonth.Year)   //check if balance updated in the same yr as that of last updated
                                                {
                                                    leaveBalance.Balance += (decimal)leaveBalance.LeaveType.IncrementCount * value;
                                                    lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 3);
                                                    leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                    incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * value);
                                                }
                                                else    //if no carry forward & also balance updated in next/new year 
                                                {
                                                    value = (int)Math.Floor((decimal)DateTime.Now.Month / 3);
                                                    leaveBalance.Balance = (int)(leaveBalance.LeaveType.IncrementCount * (value + 1));                //to get the value assigned according to the current month no. 
                                                                                                                                                //lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths((value * 3)+1);     //get the latest credited month but here in log field, its of prev. yr so cant add months
                                                    lastCreditedMonth = new DateOnly(DateTime.Now.Year, 1, 1).AddMonths(value * 3);        //gets latest credited month of this new yr  
                                                    leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                    incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * (value + 1));
                                                }
                                            }
                                        }

                                        else if (LeaveIncrementGapMonth.Half == leaveBalance.LeaveType.IncrementGapId)  //half yearly increment
                                        {
                                            int value = (int)Math.Floor(UpdateReq / 6);
                                            if (leaveBalance.LeaveType.CarryForward == true)
                                            {
                                                if ((leaveBalance.Balance + (leaveBalance.LeaveType.IncrementCount * value)) > leaveBalance.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit
                                                {
                                                    leaveBalance.Balance = (decimal)leaveBalance.LeaveType.CarryForwardLimit;       //set carryforward limit itself as balance
                                                    lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 2);
                                                    leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                    incrementValue = 0;
                                                }
                                                else
                                                {
                                                    leaveBalance.Balance += (decimal)leaveBalance.LeaveType.IncrementCount * value;
                                                    lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 2);
                                                    leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                    incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * value);
                                                }
                                            }
                                            else //if no carry forward allowed
                                            {
                                                if (DateTime.Now.Year == leaveIncrementLog.LastCreditedMonth.Year)   //check if balance updated in the same yr as that of last updated
                                                {
                                                    leaveBalance.Balance += (decimal)leaveBalance.LeaveType.IncrementCount * value;
                                                    lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 6);
                                                    leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                    incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * value);
                                                }
                                                else    //if no carry forward & also balance updated in next/new year 
                                                {
                                                    value = (int)Math.Floor((decimal)DateTime.Now.Month / 6);
                                                    leaveBalance.Balance = (int)(leaveBalance.LeaveType.IncrementCount * (value + 1));
                                                    lastCreditedMonth = new DateOnly(DateTime.Now.Year, 1, 1).AddMonths((value * 6));
                                                    leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                    incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * (value + 1));
                                                }
                                            }
                                        }
                                        else      //which does not have increment like the privelege leave thats got in a go   //updatd in a year
                                        {
                                            int value = (int)Math.Floor(UpdateReq / 12);
                                            if (leaveBalance.LeaveType.CarryForward == true)
                                            {
                                                if (DateTime.Now.Year == leaveIncrementLog.LastCreditedMonth.Year)                     //updated in same yr & carry forward allowed
                                                {
                                                    if ((leaveBalance.Balance + (leaveBalance.LeaveType.IncrementCount * value)) > leaveBalance.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit 
                                                    {
                                                        leaveBalance.Balance = (decimal)leaveBalance.LeaveType.CarryForwardLimit;       //set carryforward limit itself as balance
                                                        lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
                                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                        incrementValue = 0;
                                                    }
                                                    else
                                                    {
                                                        leaveBalance.Balance += (decimal)leaveBalance.LeaveType.IncrementCount * value;
                                                        lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
                                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                        incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * value);
                                                    }
                                                }
                                                else     //carryforward allowed & a new yr updated
                                                {
                                                    if ((leaveBalance.Balance + (leaveBalance.LeaveType.IncrementCount * value) + leaveBalance.LeaveType.MaxDays) > leaveBalance.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit ///max days bcoz each yr updated by max days 
                                                    {
                                                        leaveBalance.Balance = (decimal)(leaveBalance.LeaveType.CarryForwardLimit);       //set carryforward limit itself as balance
                                                        lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
                                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                        incrementValue = 0;
                                                    }
                                                    else
                                                    {
                                                        leaveBalance.Balance += (decimal)(leaveBalance.LeaveType.IncrementCount * value + leaveBalance.LeaveType.MaxDays);
                                                        lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
                                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                        incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * value + leaveBalance.LeaveType.MaxDays);
                                                    }
                                                }
                                            }
                                            else //if no carry forward allowed
                                            {
                                                if (DateTime.Now.Year == leaveIncrementLog.LastCreditedMonth.Year)   //check if balance updated in the same yr as that of last updated
                                                {
                                                    leaveBalance.Balance += (decimal)leaveBalance.LeaveType.IncrementCount * value;
                                                    lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
                                                    leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                    incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * value);
                                                }
                                                else    //if no carry forward & also balance updated in next/new year 
                                                {
                                                    leaveBalance.Balance = (int)(leaveBalance.LeaveType.IncrementCount * DateTime.Now.Month);
                                                    lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
                                                    leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                    incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * DateTime.Now.Month);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else                  //if leave balance entry &increment log is new for a user (first time entry in leave balance and incremnet log
                            {

                                if (leaveBalance.Employee.JoiningDate?.Month == DateTime.Now.Month && leaveBalance.Employee.JoiningDate?.Year == DateTime.Now.Year)           //for same month yr newly joined employee 
                                {
                                    if (leaveBalance.Employee.JoiningDate?.Day < leaveSetting.MinDaysForLeaveCreditJoining)        //if joined  before 15th(maxcredit..)
                                    {
                                        leaveBalance.Balance = 1;             //bcoz initially balance is 1  //only casual leave is credited on same month
                                    }
                                    else
                                    {
                                        leaveBalance.Balance = 0;
                                    }
                                    leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                    lastCreditedMonth = DateOnly.FromDateTime(DateTime.Now);
                                    incrementValue = 0;
                                }
                                else
                                {
                                    if (leaveBalance.LeaveType.IncrementGapId == LeaveIncrementGapMonth.Month)
                                    {
                                        incrementValue = DateTime.Now.Month / 1;
                                        lastCreditedMonth = DateOnly.FromDateTime(DateTime.Now);
                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                        leaveBalance.Balance = (decimal)(DateTime.Now.Month * leaveBalance.LeaveType.IncrementCount);
                                    }
                                    else if (leaveBalance.LeaveType.IncrementGapId == LeaveIncrementGapMonth.Quarter)
                                    {
                                        incrementValue = (int)Math.Floor((decimal)DateTime.Now.Month / 3);
                                        lastCreditedMonth = new DateOnly(DateTime.Now.Year, 1, 1).AddMonths(incrementValue * 3);
                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                        leaveBalance.Balance = (decimal)leaveBalance.LeaveType.IncrementCount * (incrementValue + 1);
                                    }
                                    else if (leaveBalance.LeaveType.IncrementGapId == LeaveIncrementGapMonth.Half)
                                    {
                                        incrementValue = (int)Math.Floor((decimal)DateTime.Now.Month / 6);
                                        lastCreditedMonth = new DateOnly(DateTime.Now.Year, 1, 1).AddMonths(incrementValue * 6);
                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                        leaveBalance.Balance = (decimal)leaveBalance.LeaveType.IncrementCount * (incrementValue + 1);
                                    }
                                    else
                                    {
                                        incrementValue = 0;
                                        lastCreditedMonth = new DateOnly(DateTime.Now.Year, 1, 1);
                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                        leaveBalance.Balance = (decimal)leaveBalance.LeaveType.MaxDays;
                                    }
                                }
                                LeaveBalance leaveBalance1 = new LeaveBalance(emp.Id, leaveBalance.TypeId, (int)leaveBalance.Balance);
                                _context.LeaveBalances.Add(leaveBalance1);
                            }
                        }

                        LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveBalance.Id, leaveUpdateLog, leaveBalance.Balance, incrementValue, lastCreditedMonth);
                        _context.LeaveIncrementLogs.Add(leaveIncrement);
                    }
                }
                else //if no entry for employee leave balance (leave balance is empty)
                {
                    List<LeaveType> leaveTypes = await _context.LeaveType.ToListAsync(cancellationToken);
                    foreach (LeaveType leave in leaveTypes)
                    {
                        if (emp.JoiningDate?.Month == DateTime.Now.Month && emp.JoiningDate?.Year == DateTime.Now.Year)           //for same month yr newly joined employee 
                        {
                            if (emp.JoiningDate?.Day < leaveSetting.MinDaysForLeaveCreditJoining)        //if joined  before 15th(maxcredit..)
                            {
                                balance = 1;             //bcoz initially balance is 1  //only casual leave is credited on same month
                            }
                            else
                            {
                                balance = 0;
                            }
                            leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                            lastCreditedMonth = DateOnly.FromDateTime(DateTime.Now);
                            incrementValue = 0;
                        }
                        else
                        {
                            if (leave.IncrementGapId == LeaveIncrementGapMonth.Month)
                            {
                                incrementValue = DateTime.Now.Month / 1;
                                lastCreditedMonth = DateOnly.FromDateTime(DateTime.Now);
                                leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                balance = (int)(DateTime.Now.Month * leave.IncrementCount);
                            }
                            else if (leave.IncrementGapId == LeaveIncrementGapMonth.Quarter)
                            {
                                incrementValue = (int)Math.Floor((decimal)DateTime.Now.Month / 3);
                                lastCreditedMonth = new DateOnly(DateTime.Now.Year, 1, 1).AddMonths(incrementValue * 3);
                                leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                balance = (int)leave.IncrementCount * (incrementValue + 1);
                            }
                            else if (leave.IncrementGapId == LeaveIncrementGapMonth.Half)
                            {
                                incrementValue = (int)Math.Floor((decimal)DateTime.Now.Month / 6);
                                lastCreditedMonth = new DateOnly(DateTime.Now.Year, 1, 1).AddMonths(incrementValue * 6);
                                leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                balance = (int)leave.IncrementCount * (incrementValue + 1);
                            }
                            else
                            {
                                incrementValue = 0;
                                lastCreditedMonth = new DateOnly(DateTime.Now.Year, 1, 1);
                                leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                balance = (int)leave.MaxDays;
                            }
                        }
                        LeaveBalance leaveBalance1 = new LeaveBalance(emp.Id, leave.Id, balance);
                        _context.LeaveBalances.Add(leaveBalance1);
                        await _context.SaveChangesAsync(cancellationToken);
                        LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveBalance1.Id, leaveUpdateLog, balance, incrementValue, lastCreditedMonth);
                        _context.LeaveIncrementLogs.Add(leaveIncrement);
                    }
                }
            }
            result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}









