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

        public async Task<bool> Handle(UpdateLeaveBalanceCommand command, CancellationToken cancellationToken)
        {
            List<Employee> employee= await _context.Employees.ToListAsync(cancellationToken);
            foreach (Employee emp in employee)
            {
                List<LeaveBalance> leaveBalance = await _context.LeaveBalances
                    .Where(x => x.EmployeeId == emp.Id)
                    .Include(x=>x.LeaveType)
                    .ToListAsync(cancellationToken: cancellationToken);
                LeaveSetting leaveSetting = await _context.LeaveSettings.FirstAsync(cancellationToken);

                foreach (LeaveBalance leaveType in leaveBalance)  //fetching leave balance row fof each leave type
                {
                        LeaveIncrementLog leaveIncrementLog = await _context.LeaveIncrementLogs.Where(x => x.LeaveBalanceId == leaveType.Id).FirstOrDefaultAsync(cancellationToken);//check whether same user is passed of details ALSO last log date is to be got not the first

                    if (emp.JoiningDate?.Month == DateTime.Now.Month && emp.JoiningDate?.Year == DateTime.Now.Year)           //for same month yr newly joined employee 
                    {
                        if (emp.JoiningDate?.Day < leaveSetting.MinDaysForLeaveCreditJoining)        //if joined  before 15th(maxcredit..)
                        {
                            leaveType.Balance = 1;             //bcoz initially balance is 1  //only casual leave is credited on same month
                        }
                        else
                        {
                            leaveType.Balance = 0;
                        }
                        LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, 0);
                        _context.LeaveIncrementLogs.Add(leaveIncrement);
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                    else
                    {
                        DateTime JoiningDateTime = emp.JoiningDate.Value.ToDateTime(TimeOnly.MinValue);
                        DateTime incrementLog = leaveIncrementLog.LogDate.ToDateTime(TimeOnly.MinValue);

                        if (DateTime.Now.Subtract(JoiningDateTime).Days >= leaveType.LeaveType.DutyDaysRequired)
                        {
                            int UpdateReq = DateTime.Now.Subtract(incrementLog).Days;  //check whether the updations is to be done or not depending on incrementgaptypes conditions

                            //check whether the applied date is more than the increment gap mentioned

                            if (LeaveIncrementGapMonth.Month == leaveType.LeaveType.IncrementGapId)   //monthly increment
                            {
                                int value = UpdateReq / 1;
                                if (leaveType.LeaveType.CarryForward == true)
                                {
                                    if ((leaveType.Balance + (leaveType.LeaveType.IncrementCount * value)) > leaveType.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit
                                    {
                                        leaveType.Balance = (decimal)leaveType.LeaveType.CarryForwardLimit;       //set carryforward limit itself as balance
                                        LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, 0);
                                        _context.LeaveIncrementLogs.Add(leaveIncrement);
                                    }
                                    else
                                    {
                                        leaveType.Balance += (decimal)leaveType.LeaveType.IncrementCount * value;
                                              LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType.IncrementCount * value));
                                        _context.LeaveIncrementLogs.Add(leaveIncrement);
                                    }
                                }
                                else //if no carry forward allowed
                                {
                                    if (DateTime.Now.Year == leaveIncrementLog.LogDate.Year)   //check if balance updated in the same yr as that of last updated
                                    {
                                        leaveType.Balance += (decimal)leaveType.LeaveType.IncrementCount * value;
                                        LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType.IncrementCount * value));
                                        _context.LeaveIncrementLogs.Add(leaveIncrement);
                                    }
                                    else    //if no carry forward & also balance updated in next/new year 
                                    {
                                        leaveType.Balance = 0;
                                        LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, 0);
                                        _context.LeaveIncrementLogs.Add(leaveIncrement);
                                    }
                                }
                            }


                            else if (LeaveIncrementGapMonth.Quarter == leaveType.LeaveType.IncrementGapId)    //quarterly increment
                            {
                                int value = UpdateReq / 3;
                                if (leaveType.LeaveType.CarryForward == true)
                                {
                                    if ((leaveType.Balance + (leaveType.LeaveType.IncrementCount * value)) > leaveType.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit
                                    {
                                        leaveType.Balance = (decimal)leaveType.LeaveType.CarryForwardLimit;       //set carryforward limit itself as balance
                                        LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance,0);
                                        _context.LeaveIncrementLogs.Add(leaveIncrement);
                                    }
                                    else
                                    {
                                        leaveType.Balance += (decimal)leaveType.LeaveType.IncrementCount * value;
                                        LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType?.IncrementCount * value));
                                        _context.LeaveIncrementLogs.Add(leaveIncrement);
                                    }
                                }
                                else //if no carry forward allowed
                                {
                                    if (DateTime.Now.Year == leaveIncrementLog.LogDate.Year)   //check if balance updated in the same yr as that of last updated
                                    {
                                        leaveType.Balance += (decimal)leaveType.LeaveType.IncrementCount * value;
                                        LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType?.IncrementCount * value));
                                        _context.LeaveIncrementLogs.Add(leaveIncrement);
                                    }
                                    else    //if no carry forward & also balance updated in next/new year 
                                    {
                                        leaveType.Balance = 0;
                                        LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance,0);
                                        _context.LeaveIncrementLogs.Add(leaveIncrement);
                                    }
                                }
                            }

                            else if (LeaveIncrementGapMonth.Half == leaveType.LeaveType.IncrementGapId)  //half yearly increment
                            {
                                int value = UpdateReq / 6;
                                if (leaveType.LeaveType.CarryForward == true)
                                {
                                    if ((leaveType.Balance + (leaveType.LeaveType.IncrementCount * value)) > leaveType.LeaveType.CarryForwardLimit)          //if new balance exceeds carrylimit
                                    {
                                        leaveType.Balance = (decimal)leaveType.LeaveType.CarryForwardLimit;       //set carryforward limit itself as balance
                                        LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, 0);
                                        _context.LeaveIncrementLogs.Add(leaveIncrement);
                                    }
                                    else
                                    {
                                        leaveType.Balance += (decimal)leaveType.LeaveType.IncrementCount * value;
                                        LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType.IncrementCount * value));
                                        _context.LeaveIncrementLogs.Add(leaveIncrement);
                                    }
                                }
                                else //if no carry forward allowed
                                {
                                    if (DateTime.Now.Year == leaveIncrementLog.LogDate.Year)   //check if balance updated in the same yr as that of last updated
                                    {
                                        leaveType.Balance += (decimal)leaveType.LeaveType.IncrementCount * value;
                                        LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, (int)(leaveType.LeaveType.IncrementCount * value));
                                        _context.LeaveIncrementLogs.Add(leaveIncrement);
                                    }
                                    else    //if no carry forward & also balance updated in next/new year 
                                    {
                                        leaveType.Balance = 0;
                                        LeaveIncrementLog leaveIncrement = new LeaveIncrementLog(leaveType.Id, DateOnly.FromDateTime(DateTime.Now), leaveType.Balance, 0);
                                        _context.LeaveIncrementLogs.Add(leaveIncrement);
                                    }
                                }      
                            }
                            await _context.SaveChangesAsync(cancellationToken);
                        }
                    }
                }
            }
            return true;
        }
    }
}


