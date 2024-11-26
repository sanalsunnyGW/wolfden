//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using WolfDen.Domain.Entity;
//using WolfDen.Infrastructure.Data;

//namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveBalances.InitializeLeaveBalance
//{
//    public class InititializeLeaveBalanceCommandHandler(WolfDenContext context) : IRequestHandler<InitializeLeaveBalanceCommand, bool>
//    {
//        private readonly WolfDenContext _context = context;

//        public async Task<bool> Handle(InitializeLeaveBalanceCommand request, CancellationToken cancellationToken)
//        {
//            int result;
//            List<LeaveType> leaveType = await _context.LeaveType.ToListAsync(cancellationToken);
//            Employee emp = await _context.Employees.Where(x => x.Id.Equals(request.RequestId)).FirstOrDefaultAsync(cancellationToken);
//            if (emp.JoiningDate.HasValue)
//            {
//                foreach (LeaveType type in leaveType)
//                {
//                    LeaveBalance leaveBalance = new LeaveBalance(request.RequestId, type.Id, 0);
//                    _context.LeaveBalances.Add(leaveBalance);
//                    await _context.SaveChangesAsync();
//                    LeaveIncrementLog leaveIncrementLog = new LeaveIncrementLog(leaveBalance.Id, (DateOnly)leaveBalance.Employee.JoiningDate, 0, 0, (DateOnly)leaveBalance.Employee.JoiningDate);
//                    _context.LeaveIncrementLogs.Add(leaveIncrementLog);
//                }
//            }
//            result = await _context.SaveChangesAsync();
//            return result > 0;
//        }
//    }
//}
