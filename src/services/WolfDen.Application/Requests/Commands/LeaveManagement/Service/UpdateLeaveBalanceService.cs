using Microsoft.EntityFrameworkCore;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.Service
{
    public class UpdateLeaveBalanceService(WolfDenContext context)
    {
        private readonly WolfDenContext _context=context ;
        private DateOnly lastCreditedMonth;
        private int incrementValue;
        private DateOnly leaveUpdateLog;
        private DateOnly FirstMonthDate = new DateOnly(DateTime.Now.Year, 1, 1);
        private int count = 0;
        private int balance;
        private int value;
        public async Task BalanceUpdate()
        {
            LeaveSetting leaveSetting = await _context.LeaveSettings.FirstOrDefaultAsync();
            List<Employee> employees = await _context.Employees.ToListAsync();
            List<LeaveType> leaveTypes = await _context.LeaveTypes.ToListAsync();
            List<LeaveIncrementLog> leaveIncrementLog = await _context.LeaveIncrementLogs.ToListAsync();
            List<LeaveBalance> leaveBalanceList = await _context.LeaveBalances
            .Include(x => x.LeaveType)
                .Include(x => x.Employee)
                .ToListAsync();

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

                                    LeaveIncrementLog log = leaveIncrementLog                                  //check whether same user is passed of details ALSO last log date is to be got not the first
                                            .Where(x => x.LeaveBalanceId == leaveBalance.Id)
                                            .OrderByDescending(x => x.Id)
                                            .First();

                                    if (leaveBalance.Employee.JoiningDate?.Month == DateTime.Now.Month && leaveBalance.Employee.JoiningDate?.Year == DateTime.Now.Year)        //for same month yr newly joined employee 
                                    {
                                        if (emp.JoiningDate?.Day < leaveSetting.MinDaysForLeaveCreditJoining)        //if joined  before 15th(maxcredit..)
                                        {
                                            if (leaveType.IncrementGapId == LeaveIncrementGapMonth.Month)
                                            {
                                                balance = 1;                  //bcoz initially balance is 1  
                                                lastCreditedMonth = DateOnly.FromDateTime(DateTime.Now);
                                            }
                                            else if (leaveType.IncrementGapId == LeaveIncrementGapMonth.Quarter)
                                            {
                                                if (DateTime.Now.Month == 1 || DateTime.Now.Month == 4 || DateTime.Now.Month == 7 || DateTime.Now.Month == 10)
                                                {
                                                    balance = 1;
                                                    lastCreditedMonth = DateOnly.FromDateTime(DateTime.Now);
                                                }

                                                else
                                                {
                                                    balance = 0;
                                                    if (DateTime.Now.Month > 1 && DateTime.Now.Month < 4)
                                                    {
                                                        lastCreditedMonth = FirstMonthDate;
                                                    }
                                                    else if (DateTime.Now.Month > 4 && DateTime.Now.Month < 7)
                                                    {
                                                        lastCreditedMonth = FirstMonthDate.AddMonths(3);
                                                    }
                                                    else if (DateTime.Now.Month > 7 && DateTime.Now.Month < 10)
                                                    {
                                                        lastCreditedMonth = FirstMonthDate.AddMonths(6);
                                                    }
                                                    else
                                                    {
                                                        lastCreditedMonth = FirstMonthDate.AddMonths(9);
                                                    }
                                                }
                                            }

                                            else if (leaveType.IncrementGapId == LeaveIncrementGapMonth.Half)
                                            {
                                                if (DateTime.Now.Month == 1 || DateTime.Now.Month == 7)
                                                {
                                                    balance = 1;
                                                    if (DateTime.Now.Month == 1)
                                                    {
                                                        lastCreditedMonth = FirstMonthDate;
                                                    }
                                                    else  //if month is July
                                                    {
                                                        lastCreditedMonth = FirstMonthDate.AddMonths(6);
                                                    }
                                                }
                                                else
                                                {
                                                    balance = 0;
                                                    if (DateTime.Now.Month > 1 && DateTime.Now.Month < 7)
                                                    {
                                                        lastCreditedMonth = FirstMonthDate;
                                                    }
                                                    else   //if month is aftr july
                                                    {
                                                        lastCreditedMonth = FirstMonthDate.AddMonths(6);
                                                    }
                                                }
                                            }
                                            else //yeraly increment and a new initialisation
                                            {
                                                balance = 1;
                                                lastCreditedMonth = FirstMonthDate;
                                            }
                                        }
                                        else
                                        {
                                            balance = 0;
                                            if (leaveType.IncrementGapId == LeaveIncrementGapMonth.Month)
                                            { lastCreditedMonth = FirstMonthDate; }
                                            else if (leaveType.IncrementGapId == LeaveIncrementGapMonth.Quarter)
                                            {
                                                if (DateTime.Now.Month == 1 || DateTime.Now.Month == 4 || DateTime.Now.Month == 7 || DateTime.Now.Month == 10)
                                                {

                                                }
                                                else
                                                {
                                                    if (DateTime.Now.Month > 1 && DateTime.Now.Month < 4)
                                                    {
                                                        lastCreditedMonth = FirstMonthDate;
                                                    }
                                                    else if (DateTime.Now.Month > 4 && DateTime.Now.Month < 7)
                                                    {
                                                        lastCreditedMonth = FirstMonthDate.AddMonths(3);
                                                    }
                                                    else if (DateTime.Now.Month > 7 && DateTime.Now.Month < 10)
                                                    {
                                                        lastCreditedMonth = FirstMonthDate.AddMonths(6);
                                                    }
                                                    else
                                                    {
                                                        lastCreditedMonth = FirstMonthDate.AddMonths(9);
                                                    }
                                                }
                                            }
                                            else if (leaveType.IncrementGapId == LeaveIncrementGapMonth.Half)
                                            {
                                                if (DateTime.Now.Month == 1 || DateTime.Now.Month == 7)
                                                {
                                                    lastCreditedMonth = FirstMonthDate;
                                                }
                                                else
                                                {
                                                    if (DateTime.Now.Month > 1 && DateTime.Now.Month < 6)
                                                    {
                                                        lastCreditedMonth = FirstMonthDate;
                                                    }
                                                    else
                                                    {
                                                        lastCreditedMonth = FirstMonthDate.AddMonths(6);
                                                    }
                                                }

                                            }
                                            else  //yearly type and initialising where no no. of days met with joining date
                                            {
                                                lastCreditedMonth = FirstMonthDate;
                                            }
                                        }
                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
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
                                                 value = (int)Math.Floor(UpdateReq / 1);
                                                if (leaveBalance.LeaveType.CarryForward == true)
                                                {
                                                    if ((leaveBalance.Balance + (leaveBalance.LeaveType.IncrementCount * value)) > leaveBalance.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit
                                                    {
                                                        incrementValue = (int)(leaveType.CarryForwardLimit-leaveBalance.Balance);
                                                        leaveBalance.Balance = (decimal)leaveBalance.LeaveType.CarryForwardLimit;       //set carryforward limit itself as balance
                                                        lastCreditedMonth = log.LastCreditedMonth.AddMonths(value * 1);
                                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
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
                                                 value = (int)Math.Floor(UpdateReq / 3);
                                                if (leaveBalance.LeaveType.CarryForward == true)
                                                {
                                                    if ((leaveBalance.Balance + (leaveBalance.LeaveType.IncrementCount * value)) > leaveBalance.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit
                                                    {
                                                        incrementValue = (int)(leaveType.CarryForwardLimit-leaveBalance.Balance);
                                                        leaveBalance.Balance = (decimal)leaveBalance.LeaveType.CarryForwardLimit;       //set carryforward limit itself as balance
                                                        lastCreditedMonth = log.LastCreditedMonth.AddMonths(value * 3);
                                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
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
                                                        value = (int)Math.Floor((decimal)(FirstMonthDate.Month - DateTime.Now.Month) / 3);

                                                        if (leaveBalance.LeaveType.IncrementCount * value >= (decimal)leaveBalance.LeaveType.MaxDays)
                                                        {
                                                            leaveBalance.Balance = (decimal)leaveBalance.LeaveType.MaxDays;
                                                        }
                                                        else
                                                        {
                                                            leaveBalance.Balance = (int)(leaveBalance.LeaveType.IncrementCount * (value + 1));                //to get the value assigned according to the current month no. 
                                                        }                                                                                          //lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths((value * 3)+1);     //get the latest credited month but here in log field, its of prev. yr so cant add months
                                                        lastCreditedMonth = new DateOnly(DateTime.Now.Year, 1, 1).AddMonths(value * 3);        //gets latest credited month of this new yr  
                                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                        incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * (value + 1));
                                                    }
                                                }
                                            }

                                            else if (LeaveIncrementGapMonth.Half == leaveBalance.LeaveType.IncrementGapId)  //half yearly increment
                                            {
                                                 value = (int)Math.Floor(UpdateReq / 6);
                                                if (leaveBalance.LeaveType.CarryForward == true)
                                                {
                                                    if ((leaveBalance.Balance + (leaveBalance.LeaveType.IncrementCount * value)) > leaveBalance.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit
                                                    {
                                                        incrementValue = (int)(leaveType.CarryForwardLimit-leaveBalance.Balance);
                                                        leaveBalance.Balance = (decimal)leaveBalance.LeaveType.CarryForwardLimit;       //set carryforward limit itself as balance
                                                        lastCreditedMonth = log.LastCreditedMonth.AddMonths(value * 2);
                                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
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
                                                        value = (int)Math.Floor((decimal)(FirstMonthDate.Month - DateTime.Now.Month) / 6);
                                                        leaveBalance.Balance = (int)(leaveBalance.LeaveType.IncrementCount * (value + 1));
                                                        lastCreditedMonth = new DateOnly(DateTime.Now.Year, 1, 1).AddMonths((value * 6));
                                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                        incrementValue = (int)(leaveBalance.LeaveType.IncrementCount * (value + 1));
                                                    }
                                                }
                                            }
                                            else      //which does not have increment like the privelege leave thats got in a go   //updatd in a year
                                            {
                                                if (DateTime.Now.Year == log.LastCreditedMonth.Year)   //updated tried in same yr
                                                {
                                                    leaveBalance.Balance = leaveBalance.Balance;       //set carryforward limit itself as balance
                                                    lastCreditedMonth = log.LastCreditedMonth;
                                                    leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                    incrementValue = 0;
                                                }
                                                else //new yr & carry frwd allowed
                                                {
                                                    if (leaveType.CarryForward == true)
                                                    {
                                                        if (leaveBalance.Balance + leaveType.MaxDays >= leaveType.CarryForwardLimit)
                                                        {
                                                            incrementValue = (int)(leaveType.CarryForwardLimit-leaveBalance.Balance);
                                                            leaveBalance.Balance = (decimal)leaveType.CarryForwardLimit;
                                                            lastCreditedMonth = FirstMonthDate;
                                                            leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                        }
                                                        else
                                                        {
                                                            leaveBalance.Balance += (decimal)leaveType.MaxDays;
                                                            lastCreditedMonth = FirstMonthDate;
                                                            leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                            incrementValue = (int)leaveType.MaxDays;
                                                        }
                                                    }
                                                    else  //NO carry frwd  & new yr
                                                    {
                                                        leaveBalance.Balance = (decimal)leaveType.MaxDays;
                                                        lastCreditedMonth = FirstMonthDate;
                                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                                        incrementValue = (int)leaveType.MaxDays;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            continue;
                                           // throw new Exception("Duty Days Eligibilty not met To get the leave Balance credited");
                                        }
                                    }
                                    _context.LeaveBalances.Update(leaveBalance);
                                    LeaveIncrementLog leaveLog = new LeaveIncrementLog(leaveBalance.Id, leaveUpdateLog, leaveBalance.Balance, incrementValue, lastCreditedMonth);
                                    _context.LeaveIncrementLogs.Add(leaveLog);
                                }
                                else
                                {
                                    continue;
                                  //  throw new Exception("employee Or type id doesnt match with leavebalance");
                                }
                            }
                        }
                        if (count == 0) //no entry in leavebalance of that combination...so add it(create new)
                        {
                            if ((emp.Gender == EmployeeEnum.Gender.Male && leaveType.LeaveCategoryId != LeaveCategory.Maternity) || (emp.Gender == EmployeeEnum.Gender.Female && leaveType.LeaveCategoryId != LeaveCategory.Paternity))
                            {
                                if (DateTime.Now.Subtract(JoiningDateTime).Days >= leaveType.DutyDaysRequired)
                                {
                                    decimal UpdateReq = DateTime.Now.Month - FirstMonthDate.Month;

                                    if (emp.JoiningDate?.Month == DateTime.Now.Month && emp.JoiningDate?.Year == DateTime.Now.Year)           //for same month yr newly joined employee 
                                    {
                                        if (emp.JoiningDate?.Day < leaveSetting.MinDaysForLeaveCreditJoining)        //if joined  before 15th(maxcredit..)
                                        {
                                            if (leaveType.IncrementGapId == LeaveIncrementGapMonth.Month)
                                            {
                                                balance = 1;  //bcoz initially balance is 1  //only casual leave is credited on same month
                                                lastCreditedMonth=DateOnly.FromDateTime(DateTime.Now);
                                            }
                                            else if (leaveType.IncrementGapId == LeaveIncrementGapMonth.Quarter)
                                            {
                                                if (DateTime.Now.Month == 1 || DateTime.Now.Month == 4 || DateTime.Now.Month == 7 || DateTime.Now.Month == 10)
                                                {
                                                    balance = 1;
                                                    lastCreditedMonth = DateOnly.FromDateTime(DateTime.Now);
                                                }

                                                else
                                                {
                                                    balance = 0;
                                                    if (DateTime.Now.Month > 1 && DateTime.Now.Month < 4)
                                                    {
                                                        lastCreditedMonth = FirstMonthDate;
                                                    }
                                                    else if (DateTime.Now.Month > 4 && DateTime.Now.Month < 7)
                                                    {
                                                        lastCreditedMonth = FirstMonthDate.AddMonths(3);
                                                    }
                                                    else if (DateTime.Now.Month > 7 && DateTime.Now.Month < 10)
                                                    {
                                                        lastCreditedMonth = FirstMonthDate.AddMonths(6);
                                                    }
                                                    else
                                                    {
                                                        lastCreditedMonth = FirstMonthDate.AddMonths(9);
                                                    }
                                                }
                                            }

                                            else if (leaveType.IncrementGapId == LeaveIncrementGapMonth.Half)
                                            {
                                                if (DateTime.Now.Month == 1 || DateTime.Now.Month == 7)
                                                {
                                                    balance = 1;
                                                    if (DateTime.Now.Month == 1)
                                                    {
                                                      lastCreditedMonth = FirstMonthDate;
                                                    }
                                                    else  //if month is July
                                                    {
                                                      lastCreditedMonth = FirstMonthDate.AddMonths(6);
                                                    }
                                                }
                                                else
                                                {
                                                    balance = 0;
                                                    if (DateTime.Now.Month > 1 && DateTime.Now.Month < 7)
                                                    {
                                                        lastCreditedMonth = FirstMonthDate;
                                                    }
                                                    else   //if month is aftr july
                                                    {
                                                        lastCreditedMonth = FirstMonthDate.AddMonths(6);
                                                    }
                                                }
                                            }
                                            else //yeraly increment and a new initialisation
                                            {
                                                balance = 1;
                                                lastCreditedMonth = FirstMonthDate;
                                            }
                                        }
                                        else
                                        {
                                            balance = 0;
                                            if (leaveType.IncrementGapId == LeaveIncrementGapMonth.Month)
                                            { lastCreditedMonth = FirstMonthDate; }
                                            else if(leaveType.IncrementGapId==LeaveIncrementGapMonth.Quarter)
                                            {
                                                if(DateTime.Now.Month==1|| DateTime.Now.Month == 4|| DateTime.Now.Month == 7|| DateTime.Now.Month == 10)
                                                {

                                                }
                                                else
                                                {
                                                    if(DateTime.Now.Month>1 && DateTime.Now.Month<4)
                                                    {
                                                        lastCreditedMonth=FirstMonthDate;
                                                    }
                                                    else if(DateTime.Now.Month>4 && DateTime.Now.Month<7)
                                                    {
                                                        lastCreditedMonth = FirstMonthDate.AddMonths(3);
                                                    }
                                                    else if(DateTime.Now.Month>7 && DateTime.Now.Month<10)
                                                    {
                                                        lastCreditedMonth = FirstMonthDate.AddMonths(6);
                                                    }
                                                    else 
                                                    {
                                                       lastCreditedMonth=FirstMonthDate.AddMonths(9);   
                                                    }
                                                }
                                            }
                                            else if(leaveType.IncrementGapId==LeaveIncrementGapMonth.Half)
                                            {
                                               if(DateTime.Now.Month==1 ||DateTime.Now.Month==7)
                                                {
                                                    lastCreditedMonth = FirstMonthDate;
                                                }
                                               else
                                                {
                                                   if(DateTime.Now.Month>1 && DateTime.Now.Month<6)
                                                    {
                                                        lastCreditedMonth = FirstMonthDate;
                                                    }
                                                   else
                                                    {
                                                        lastCreditedMonth = FirstMonthDate.AddMonths(6);
                                                    }
                                                }

                                            }
                                            else  //yearly type and initialising where no no. of days met with joining date
                                            {
                                                lastCreditedMonth = FirstMonthDate;
                                            }
                                        }
                                        leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                        incrementValue = 0;
                                    }
                                    
                                    else
                                    {
                                        if (leaveType.IncrementGapId == LeaveIncrementGapMonth.Month)
                                        {
                                            value = DateTime.Now.Month / 1;
                                            incrementValue =(int)leaveType.IncrementCount*value;
                                            lastCreditedMonth = DateOnly.FromDateTime(DateTime.Now);
                                            leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                            balance = (int)(DateTime.Now.Month * leaveType.IncrementCount);
                                        }
                                        else if (leaveType.IncrementGapId == LeaveIncrementGapMonth.Quarter)
                                        {
                                            value = (int)Math.Floor(UpdateReq / 3);
                                            incrementValue=(int)leaveType.IncrementCount*(value+1);
                                            lastCreditedMonth = FirstMonthDate.AddMonths(value * 3);
                                            leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                            if (leaveType.IncrementCount * (value + 1) >= leaveType.MaxDays)
                                            {
                                                balance = (int)leaveType.MaxDays;
                                            }
                                            else
                                            {
                                                balance = (int)leaveType.IncrementCount * (value + 1);
                                            }
                                        }
                                        else if (leaveType.IncrementGapId == LeaveIncrementGapMonth.Half)
                                        {
                                            value = (int)Math.Floor(UpdateReq / 6);
                                            incrementValue = (int)leaveType.IncrementCount * (value+1);
                                            lastCreditedMonth = FirstMonthDate.AddMonths(value * 6);
                                            leaveUpdateLog = DateOnly.FromDateTime(DateTime.Now);
                                            if (leaveType.IncrementCount * (value + 1) >= leaveType.MaxDays)
                                            {
                                                balance = (int)leaveType.MaxDays;
                                            }
                                            else
                                            {
                                                balance = (int)leaveType.IncrementCount * (value + 1);
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
                                    await _context.SaveChangesAsync();
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
            int result = await _context.SaveChangesAsync();
        }
    }
}
