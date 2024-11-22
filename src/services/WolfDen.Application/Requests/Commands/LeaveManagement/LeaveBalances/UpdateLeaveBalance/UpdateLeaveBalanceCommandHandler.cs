//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using WolfDen.Domain.Entity;
//using WolfDen.Domain.Enums;
//using WolfDen.Infrastructure.Data;

//namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveBalances.UpdateLeaveBalance
//{
//    public class UpdateLeaveBalanceCommandHandler(WolfDenContext context) : IRequestHandler<UpdateLeaveBalanceCommand, bool>
//    {
//        private readonly WolfDenContext _context = context;
//        public int result;
//        private DateOnly lastCreditedMonth;

//        public async Task<bool> Handle(UpdateLeaveBalanceCommand command, CancellationToken cancellationToken)
//        {
//            // List<Employee> employee = await _context.Employees.ToListAsync(cancellationToken);
//            LeaveSetting leaveSetting = await _context.LeaveSettings.FirstOrDefaultAsync(cancellationToken);

//            //remove forach employees 
//            List<LeaveBalance> leaveBalance = await _context.LeaveBalances
//                .Include(x => x.LeaveType)
//                .Include(x => x.Employee)
//                .ToListAsync(cancellationToken: cancellationToken);

//            foreach (LeaveBalance leaveType in leaveBalance)  //fetching leave balance row for each leave type
//            {
//                if (leaveType.Employee.Id == leaveType.EmployeeId)
//                {
//                    LeaveIncrementLog leaveIncrementLog = await _context.LeaveIncrementLogs
//                    .Where(x => x.LeaveBalanceId == leaveType.Id)
//                    .OrderByDescending(x => x.Id)
//                    .FirstOrDefaultAsync(cancellationToken);    //check whether same user is passed of details ALSO last log date is to be got not the first

//                    if (leaveType.Employee.JoiningDate?.Month == DateTime.Now.Month && leaveType.Employee.JoiningDate?.Year == DateTime.Now.Year)           //for same month yr newly joined employee 
//                    {
//                        if (leaveType.Employee.JoiningDate?.Day < leaveSetting.MinDaysForLeaveCreditJoining)        //if joined  before 15th(maxcredit..)
//                        {
//                            leaveType.Balance = 1;             //bcoz initially balance is 1  //only casual leave is credited on same month
//                        }
//                        else
//                        {
//                            leaveType.Balance = 0;
//                        }
//                        LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, 0, lastCreditedMonth);
//                        _context.LeaveIncrementLogs.Add(leaveIncrement);
//                        //result=  await _context.SaveChangesAsync(cancellationToken);
//                    }
//                    else
//                    {
//                        DateTime JoiningDateTime = leaveType.Employee.JoiningDate.Value.ToDateTime(TimeOnly.MinValue);
//                        // DateTime incrementLog = leaveIncrementLog.LogDate.ToDateTime(TimeOnly.MinValue);

//                        if (DateTime.Now.Subtract(JoiningDateTime).Days >= leaveType.LeaveType.DutyDaysRequired)
//                        {
//                            if (leaveIncrementLog != null)
//                            {
//                                decimal UpdateReq = DateTime.Now.Month - leaveIncrementLog.LastCreditedMonth.Month;  
//                                //check whether the applied date is more than the increment gap mentioned

//                                if (LeaveIncrementGapMonth.Month == leaveType.LeaveType.IncrementGapId)   //monthly increment
//                                {
//                                    int value = (int)Math.Floor(UpdateReq / 1);
//                                    if (leaveType.LeaveType.CarryForward == true)
//                                    {
//                                        if ((leaveType.Balance + (leaveType.LeaveType.IncrementCount * value)) > leaveType.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit
//                                        {
//                                            leaveType.Balance = (decimal)leaveType.LeaveType.CarryForwardLimit;       //set carryforward limit itself as balance
//                                            lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
//                                            LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, 0, lastCreditedMonth);
//                                            _context.LeaveIncrementLogs.Add(leaveIncrement);
//                                        }
//                                        else
//                                        {
//                                            leaveType.Balance += (decimal)leaveType.LeaveType.IncrementCount * value;
//                                            lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
//                                            LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType.IncrementCount * value), lastCreditedMonth);
//                                            _context.LeaveIncrementLogs.Add(leaveIncrement);
//                                        }
//                                    }
//                                    else //if no carry forward allowed
//                                    {
//                                        if (DateTime.Now.Year == leaveIncrementLog.LastCreditedMonth.Year)   //check if balance updated in the same yr as that of last updated
//                                        {
//                                            leaveType.Balance += (decimal)leaveType.LeaveType.IncrementCount * value;
//                                            lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
//                                            LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType.IncrementCount * value), lastCreditedMonth);
//                                            _context.LeaveIncrementLogs.Add(leaveIncrement);
//                                        }
//                                        else    //if no carry forward & also balance updated in next/new year 
//                                        {
//                                            leaveType.Balance = (int)(leaveType.LeaveType.IncrementCount * value);
//                                            lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
//                                            LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType.IncrementCount * value), lastCreditedMonth);
//                                            _context.LeaveIncrementLogs.Add(leaveIncrement);
//                                        }
//                                    }
//                                }


//                                else if (LeaveIncrementGapMonth.Quarter == leaveType.LeaveType.IncrementGapId)    //quarterly increment
//                                {
//                                    int value = (int)Math.Floor(UpdateReq / 3);
//                                    if (leaveType.LeaveType.CarryForward == true)
//                                    {
//                                        if ((leaveType.Balance + (leaveType.LeaveType.IncrementCount * value)) > leaveType.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit
//                                        {
//                                            leaveType.Balance = (decimal)leaveType.LeaveType.CarryForwardLimit;       //set carryforward limit itself as balance
//                                            lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 3);
//                                            LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, 0, lastCreditedMonth);
//                                            _context.LeaveIncrementLogs.Add(leaveIncrement);
//                                        }
//                                        else
//                                        {
//                                            leaveType.Balance += (decimal)leaveType.LeaveType.IncrementCount * value;
//                                            lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 3);
//                                            LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType?.IncrementCount * value), lastCreditedMonth);
//                                            _context.LeaveIncrementLogs.Add(leaveIncrement);
//                                        }
//                                    }
//                                    else //if no carry forward allowed
//                                    {
//                                        if (DateTime.Now.Year == leaveIncrementLog.LastCreditedMonth.Year)   //check if balance updated in the same yr as that of last updated
//                                        {
//                                            leaveType.Balance += (decimal)leaveType.LeaveType.IncrementCount * value;
//                                            lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 3);
//                                            LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType?.IncrementCount * value), lastCreditedMonth);
//                                            _context.LeaveIncrementLogs.Add(leaveIncrement);
//                                        }
//                                        else    //if no carry forward & also balance updated in next/new year 
//                                        {
//                                            leaveType.Balance = (int)(leaveType.LeaveType.IncrementCount * value);
//                                            lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 3);
//                                            LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType.IncrementCount * value), lastCreditedMonth);
//                                            _context.LeaveIncrementLogs.Add(leaveIncrement);
//                                        }
//                                    }
//                                }

//                                else if (LeaveIncrementGapMonth.Half == leaveType.LeaveType.IncrementGapId)  //half yearly increment
//                                {
//                                    int value = (int)Math.Floor(UpdateReq / 2);
//                                    if (leaveType.LeaveType.CarryForward == true)
//                                    {
//                                        if ((leaveType.Balance + (leaveType.LeaveType.IncrementCount * value)) > leaveType.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit
//                                        {
//                                            leaveType.Balance = (decimal)leaveType.LeaveType.CarryForwardLimit;       //set carryforward limit itself as balance
//                                            lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 2);
//                                            LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, 0, lastCreditedMonth);
//                                            _context.LeaveIncrementLogs.Add(leaveIncrement);
//                                        }
//                                        else
//                                        {
//                                            leaveType.Balance += (decimal)leaveType.LeaveType.IncrementCount * value;
//                                            lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 2);
//                                            LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType.IncrementCount * value), lastCreditedMonth);
//                                            _context.LeaveIncrementLogs.Add(leaveIncrement);
//                                        }
//                                    }
//                                    else //if no carry forward allowed
//                                    {
//                                        if (DateTime.Now.Year == leaveIncrementLog.LastCreditedMonth.Year)   //check if balance updated in the same yr as that of last updated
//                                        {
//                                            leaveType.Balance += (decimal)leaveType.LeaveType.IncrementCount * value;
//                                            lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 2);
//                                            LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType.IncrementCount * value), lastCreditedMonth);
//                                            _context.LeaveIncrementLogs.Add(leaveIncrement);
//                                        }
//                                        else    //if no carry forward & also balance updated in next/new year 
//                                        {
//                                            leaveType.Balance = (int)(leaveType.LeaveType.IncrementCount * value);
//                                            lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 2);
//                                            LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType.IncrementCount * value), lastCreditedMonth);
//                                            _context.LeaveIncrementLogs.Add(leaveIncrement);
//                                        }
//                                    }
//                                }
//                                else      //which does not have increment like the privelege leave thats got in a go   //updatd in a year
//                                {
//                                    int value = (int)Math.Floor(UpdateReq / 12);
//                                    if (leaveType.LeaveType.CarryForward == true)
//                                    {
//                                        if (DateTime.Now.Year == leaveIncrementLog.LastCreditedMonth.Year)                     //updated in same yr & carry forward allowed
//                                        {
//                                            if ((leaveType.Balance + (leaveType.LeaveType.IncrementCount * value)) > leaveType.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit 
//                                            {
//                                                leaveType.Balance = (decimal)leaveType.LeaveType.CarryForwardLimit;       //set carryforward limit itself as balance
//                                                lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
//                                                LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, 0, lastCreditedMonth);
//                                                _context.LeaveIncrementLogs.Add(leaveIncrement);
//                                            }
//                                            else
//                                            {
//                                                leaveType.Balance += (decimal)leaveType.LeaveType.IncrementCount * value;
//                                                lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
//                                                LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType.IncrementCount * value), lastCreditedMonth);
//                                                _context.LeaveIncrementLogs.Add(leaveIncrement);
//                                            }
//                                        }
//                                        else     //carryforward allowed & a new yr updated
//                                        {
//                                            if ((leaveType.Balance + (leaveType.LeaveType.IncrementCount * value) + leaveType.LeaveType.MaxDays) > leaveType.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit ///max days bcoz each yr updated by max days 
//                                            {
//                                                leaveType.Balance = (decimal)(leaveType.LeaveType.CarryForwardLimit);       //set carryforward limit itself as balance
//                                                lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
//                                                LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, 0, lastCreditedMonth);
//                                                _context.LeaveIncrementLogs.Add(leaveIncrement);
//                                            }
//                                            else
//                                            {
//                                                leaveType.Balance += (decimal)(leaveType.LeaveType.IncrementCount * value + leaveType.LeaveType.MaxDays);
//                                                lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
//                                                LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType.IncrementCount * value + leaveType.LeaveType.MaxDays), lastCreditedMonth);
//                                                _context.LeaveIncrementLogs.Add(leaveIncrement);
//                                            }
//                                        }
//                                    }
//                                    else //if no carry forward allowed
//                                    {
//                                        if (DateTime.Now.Year == leaveIncrementLog.LastCreditedMonth.Year)   //check if balance updated in the same yr as that of last updated
//                                        {
//                                            leaveType.Balance += (decimal)leaveType.LeaveType.IncrementCount * value;
//                                            lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
//                                            LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType.IncrementCount * value), lastCreditedMonth);
//                                            _context.LeaveIncrementLogs.Add(leaveIncrement);
//                                        }
//                                        else    //if no carry forward & also balance updated in next/new year 
//                                        {
//                                            leaveType.Balance = (int)(leaveType.LeaveType.IncrementCount * value);
//                                            lastCreditedMonth = leaveIncrementLog.LastCreditedMonth.AddMonths(value * 1);
//                                            LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType.IncrementCount * value), lastCreditedMonth);
//                                            _context.LeaveIncrementLogs.Add(leaveIncrement);
//                                        }
//                                    }
//                                }
//                                //result= await _context.SaveChangesAsync(cancellationToken);
//                            }
//                        }
//                        else
//                        {
//                            // Handle the case when leaveIncrementLog is null
//                            Console.WriteLine("leaveIncrementLog is null");
//                        }
//                    }
//                }
//                result = await _context.SaveChangesAsync(cancellationToken);
//            }
//            return result > 0;
//        }
//    }
//}


