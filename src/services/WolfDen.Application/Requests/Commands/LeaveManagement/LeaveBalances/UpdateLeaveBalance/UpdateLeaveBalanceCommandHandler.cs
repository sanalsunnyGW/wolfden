using MediatR;
using Microsoft.Azure.Pipelines.WebApi;
using Microsoft.EntityFrameworkCore;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveBalances.UpdateLeaveBalance
{
    public class UpdateLeaveBalanceCommandHandler(WolfDenContext context) : IRequestHandler<UpdateLeaveBalanceCommand, bool>
    {
        private readonly WolfDenContext _context = context;
        private DateOnly lastCreditedMonth;
        private int incrementValue;
        private DateOnly leaveUpdateLog;
        private int count = 0;
        private int balance;
        public async Task<bool> Handle(UpdateLeaveBalanceCommand request, CancellationToken cancellationToken)
        {
            LeaveSetting leaveSetting = await _context.LeaveSettings.FirstOrDefaultAsync(cancellationToken);
            List<Employee> employees = await _context.Employees.ToListAsync(cancellationToken);
            List<LeaveType> leaveTypes = await _context.LeaveType.ToListAsync(cancellationToken);
            List<LeaveIncrementLog> leaveIncrementLog = await _context.LeaveIncrementLogs.ToListAsync(cancellationToken);
            List<LeaveBalance> leaveBalanceList = await _context.LeaveBalances
                .Include(x => x.LeaveType)
                .Include(x => x.Employee)
                .ToListAsync(cancellationToken);

            foreach (Employee emp in employees)
            {
                if (emp.JoiningDate is not null && emp.Gender is not null)
                {
                    DateTime JoiningDateTime = emp.JoiningDate.Value.ToDateTime(TimeOnly.MinValue);

                    foreach (LeaveType leaveType in leaveTypes)
                    {
                        count = 0;
                        foreach (LeaveBalance leaveBalance in leaveBalanceList)
                        {
                            if ((emp.Gender == EmployeeEnum.Gender.Male && leaveType.LeaveCategoryId != LeaveCategory.Maternity) || (emp.Gender == EmployeeEnum.Gender.Female && leaveType.LeaveCategoryId != LeaveCategory.Paternity))
                            { 
                                if (leaveBalance.TypeId == leaveType.Id && leaveBalance.EmployeeId == emp.Id)   //data exists for that combination...its to be updated
                                {
                                    count = count + 1;  //entry for that combination exist..so update it ..

                                    //UPDATE the Existing LeaveBalance Row values

                                    LeaveIncrementLog log = leaveIncrementLog
                                            .Where(x => x.LeaveBalanceId.Equals(leaveBalance.Id))
                                            .OrderByDescending(x => x.Id)
                                            .FirstOrDefault();
                                    //check whether same user is passed of details ALSO last log date is to be got not the first

                                    if (leaveBalance.Employee.JoiningDate?.Month == DateTime.Now.Month && leaveBalance.Employee.JoiningDate?.Year == DateTime.Now.Year)        //for same month yr newly joined employee 
                                    {
                                        if (leaveBalance.Employee.JoiningDate?.Day < leaveSetting.MinDaysForLeaveCreditJoining)    //if joined  before 15th(maxcredit..)
                                        {
                                            leaveBalance.Balance = 1;             //bcoz initially balance is 1  //only casual leave is credited on same month
                                        }
                                        else //for new joinees other type leaves 
                                        {
                                            leaveBalance.Balance = 0;
                                        }

                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                        lastCreditedMonth = leaveBalance.Employee.JoiningDate.Value;
                                        incrementValue = 0;
                                    }
                                    else //not a newly joined employees
                                    {
                                        if (DateTime.Now.Subtract(JoiningDateTime).Days >= leaveBalance.LeaveType.DutyDaysRequired)
                                        {
                                            decimal UpdateReq = DateTime.Now.Month - log.LastCreditedMonth.Month;
                                            //check whether the updated date is more than the increment gap mentioned

                                            if (LeaveIncrementGapMonth.Month == leaveBalance.LeaveType.IncrementGapId)   //monthly increment
                                            {
                                                int value = (int)Math.Floor(UpdateReq / 1);
                                                if (leaveBalance.LeaveType.CarryForward == true)
                                                {
                                                    if ((leaveBalance.Balance + (leaveBalance.LeaveType.IncrementCount * value)) > leaveBalance.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit
                                                    {
                                                        leaveBalance.Balance = (decimal)leaveBalance.LeaveType.CarryForwardLimit;       //set carryforward limit itself as balance
                                                        lastCreditedMonth = log.LastCreditedMonth.AddMonths(value * 1);
                                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                        incrementValue = 0;
                                                    }
                                                    else
                                                    {
                                                        leaveBalance.Balance += (decimal)leaveBalance.LeaveType.IncrementCount * value;
                                                        lastCreditedMonth = log.LastCreditedMonth.AddMonths(value * 1);
                                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                        incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * value);
                                                    }
                                                }
                                                else //if no carry forward allowed
                                                {
                                                    if (DateTime.Now.Year == log.LastCreditedMonth.Year)   //check if balance updated in the same yr as that of last updated
                                                    {
                                                        leaveBalance.Balance += (decimal)leaveBalance.LeaveType.IncrementCount * value;
                                                        lastCreditedMonth = log.LastCreditedMonth.AddMonths(value * 1);
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
                                                        lastCreditedMonth = log.LastCreditedMonth.AddMonths(value * 3);
                                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                        incrementValue = 0;
                                                    }
                                                    else
                                                    {
                                                        leaveBalance.Balance += (decimal)leaveBalance.LeaveType.IncrementCount * value;
                                                        lastCreditedMonth = log.LastCreditedMonth.AddMonths(value * 3);
                                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                        incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * value);
                                                    }
                                                }
                                                else //if no carry forward allowed
                                                {
                                                    if (DateTime.Now.Year == log.LastCreditedMonth.Year)   //check if balance updated in the same yr as that of last updated
                                                    {
                                                        if (leaveBalance.Balance + leaveBalance.LeaveType.IncrementCount * value >= (decimal)leaveBalance.LeaveType.MaxDays)
                                                        {
                                                            leaveBalance.Balance = (decimal)leaveBalance.LeaveType.MaxDays;
                                                        }
                                                        else
                                                        {
                                                            leaveBalance.Balance += (decimal)leaveBalance.LeaveType.IncrementCount * value;

                                                        }
                                                        lastCreditedMonth = log.LastCreditedMonth.AddMonths(value * 3);
                                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                        incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * value);
                                                    }
                                                    else    //if no carry forward & also balance updated in next/new year 
                                                    {
                                                        value = (int)Math.Floor((decimal)DateTime.Now.Month / 3);

                                                        if (leaveBalance.Balance + leaveBalance.LeaveType.IncrementCount * value >= (decimal)leaveBalance.LeaveType.MaxDays)
                                                        {
                                                            leaveBalance.Balance = (decimal)leaveBalance.LeaveType.MaxDays;
                                                        }
                                                        else
                                                        {
                                                            leaveBalance.Balance += (int)(leaveBalance.LeaveType.IncrementCount * (value + 1));                //to get the value assigned according to the current month no. 
                                                        }                                                                                          //lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths((value * 3)+1);     //get the latest credited month but here in log field, its of prev. yr so cant add months
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
                                                        lastCreditedMonth = log.LastCreditedMonth.AddMonths(value * 2);
                                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                        incrementValue = 0;
                                                    }
                                                    else
                                                    {
                                                        leaveBalance.Balance += (decimal)leaveBalance.LeaveType.IncrementCount * value;
                                                        lastCreditedMonth = log.LastCreditedMonth.AddMonths(value * 2);
                                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                        incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * value);
                                                    }
                                                }
                                                else //if no carry forward allowed
                                                {
                                                    if (DateTime.Now.Year == log.LastCreditedMonth.Year)   //check if balance updated in the same yr as that of last updated
                                                    {
                                                        leaveBalance.Balance += (decimal)leaveBalance.LeaveType.IncrementCount * value;
                                                        lastCreditedMonth = log.LastCreditedMonth.AddMonths(value * 6);
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
                                                    if (DateTime.Now.Year == log.LastCreditedMonth.Year)                     //updated in same yr & carry forward allowed
                                                    {
                                                        if ((leaveBalance.Balance + (leaveBalance.LeaveType.IncrementCount * value)) > leaveBalance.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit 
                                                        {
                                                            leaveBalance.Balance = (decimal)leaveBalance.LeaveType.CarryForwardLimit;       //set carryforward limit itself as balance
                                                            lastCreditedMonth = log.LastCreditedMonth.AddMonths(value * 1);
                                                            leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                            incrementValue = 0;
                                                        }
                                                        else
                                                        {
                                                            leaveBalance.Balance += (decimal)leaveBalance.LeaveType.IncrementCount * value;
                                                            lastCreditedMonth = log.LastCreditedMonth.AddMonths(value * 1);
                                                            leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                            incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * value);
                                                        }
                                                    }
                                                    else     //carryforward allowed & a new yr updated
                                                    {
                                                        if ((leaveBalance.Balance + (leaveBalance.LeaveType.IncrementCount * value) + leaveBalance.LeaveType.MaxDays) > leaveBalance.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit ///max days bcoz each yr updated by max days 
                                                        {
                                                            leaveBalance.Balance = (decimal)(leaveBalance.LeaveType.CarryForwardLimit);       //set carryforward limit itself as balance
                                                            lastCreditedMonth = log.LastCreditedMonth.AddMonths(value * 1);
                                                            leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                            incrementValue = 0;
                                                        }
                                                        else
                                                        {
                                                            leaveBalance.Balance += (decimal)(leaveBalance.LeaveType.IncrementCount * value + leaveBalance.LeaveType.MaxDays);
                                                            lastCreditedMonth = log.LastCreditedMonth.AddMonths(value * 1);
                                                            leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                            incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * value + leaveBalance.LeaveType.MaxDays);
                                                        }
                                                    }
                                                }
                                                else //if no carry forward allowed
                                                {
                                                    if (DateTime.Now.Year == log.LastCreditedMonth.Year)   //check if balance updated in the same yr as that of last updated
                                                    {
                                                        if (leaveType.IncrementCount != null)
                                                        {
                                                            leaveBalance.Balance += (decimal)leaveBalance.LeaveType.IncrementCount * value;
                                                            lastCreditedMonth = log.LastCreditedMonth.AddMonths(value * 1);
                                                            leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                            incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * value);
                                                        }
                                                    }
                                                    else    //if no carry forward & also balance updated in next/new year 
                                                    {
                                                        leaveBalance.Balance = (int)(leaveBalance.LeaveType.IncrementCount * DateTime.Now.Month);
                                                        lastCreditedMonth = log.LastCreditedMonth.AddMonths(value * 1);
                                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                        incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * DateTime.Now.Month);
                                                    }
                                                }
                                            }
                                            _context.LeaveBalances.Update(leaveBalance);
                                        }
                                        else
                                        {
                                            continue;
                                            throw new Exception("Duty Days Eligibilty not met To get the leave Balance credited");
                                        }
                                    }
                                    LeaveIncrementLog leaveLog = new LeaveIncrementLog(leaveBalance.Id, leaveUpdateLog, leaveBalance.Balance, incrementValue, lastCreditedMonth);
                                }

                                else
                                {
                                    continue;
                                    throw new Exception("employee Or type id doesnt match with leavebalance");
                                }
                            }
                        }
                        if (count == 0) //no entry in leavebalance of that combination...so add it(create new)
                        {
                            if ((emp.Gender == EmployeeEnum.Gender.Male && leaveType.LeaveCategoryId != LeaveCategory.Maternity) || (emp.Gender==EmployeeEnum.Gender.Female  && leaveType.LeaveCategoryId!=LeaveCategory.Paternity))
                            {
                                if (DateTime.Now.Subtract(JoiningDateTime).Days >= leaveType.DutyDaysRequired)
                                {
                                    DateOnly FirstMonthDate= new DateOnly(DateTime.Now.Year,1,1);
                                    decimal UpdateReq =  DateTime.Now.Month- FirstMonthDate.Month ;

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
                                        if (leaveType.IncrementGapId == LeaveIncrementGapMonth.Month)
                                        {
                                            incrementValue = DateTime.Now.Month / 1;
                                            lastCreditedMonth = DateOnly.FromDateTime(DateTime.Now);
                                            leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                            balance = (int)(DateTime.Now.Month * leaveType.IncrementCount);
                                        }
                                        else if (leaveType.IncrementGapId == LeaveIncrementGapMonth.Quarter)
                                        {
                                            incrementValue = (int)Math.Floor(UpdateReq / 3);
                                            lastCreditedMonth =FirstMonthDate.AddMonths(incrementValue * 3);
                                            leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                            if ((decimal)leaveType.IncrementCount * (incrementValue + 1) >= (decimal)leaveType.MaxDays)
                                            {
                                                balance = (int)leaveType.MaxDays;
                                            }
                                            else
                                            {
                                                balance = (int)leaveType.IncrementCount * (incrementValue + 1);
                                            }
                                        }
                                        else if (leaveType.IncrementGapId == LeaveIncrementGapMonth.Half)
                                        {
                                            incrementValue = (int)Math.Floor(UpdateReq/ 6);
                                            lastCreditedMonth =FirstMonthDate.AddMonths(incrementValue * 6);
                                            leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                            if ((decimal)leaveType.IncrementCount * (incrementValue + 1) >= (decimal)leaveType.MaxDays)
                                            {
                                                balance = (int)leaveType.MaxDays;
                                            }
                                            else
                                            {
                                                balance = (int)leaveType.IncrementCount * (incrementValue + 1);
                                            }
                                        }
                                        else
                                        {
                                            incrementValue = 0;
                                            lastCreditedMonth = FirstMonthDate;
                                            leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                            balance = (int)leaveType.MaxDays;
                                        }
                                    }
                                    LeaveBalance leaveBalance1 = new LeaveBalance(emp.Id, leaveType.Id, balance);
                                    _context.LeaveBalances.Add(leaveBalance1);
                                    await _context.SaveChangesAsync(cancellationToken);
                                    LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveBalance1.Id, leaveUpdateLog, balance, incrementValue, lastCreditedMonth);
                                    _context.LeaveIncrementLogs.Add(leaveIncrement);
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;      
                        }
                    }
                }
                else
                {
                    continue;
                    //throw new Exception("employee joining date OR/AND gender is null");
                }
            }
            int result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}









