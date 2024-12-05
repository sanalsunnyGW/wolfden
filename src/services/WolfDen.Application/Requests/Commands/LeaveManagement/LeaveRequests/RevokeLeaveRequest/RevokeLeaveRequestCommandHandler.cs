using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.RevokeLeaveRequest
{
    public class RevokeLeaveRequestCommandHandler(WolfDenContext context,RevokeLeaveRequestValidator validator ) : IRequestHandler<RevokeLeaveRequestCommand, bool>
    {
        private readonly WolfDenContext _context = context;
        private readonly RevokeLeaveRequestValidator _validator = validator;

        public async Task<bool> Handle(RevokeLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validatorResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validatorResult.IsValid)
            {
                var errors = string.Join(", ", validatorResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Validation failed: {errors}");
            }
            LeaveRequest leaveRequest = await _context.LeaveRequests.FirstOrDefaultAsync(x => x.Id == request.LeaveRequestId, cancellationToken);
            LeaveType leave = await _context.LeaveType.FirstOrDefaultAsync(x => x.LeaveCategoryId == LeaveCategory.WorkFromHome, cancellationToken); 
            decimal leaveRequestDayCount = await _context.LeaveRequestDays.Where(x => x.LeaveRequestId == request.LeaveRequestId).CountAsync();
            if (leaveRequest is null)
            {
                throw new Exception("No such Leave");
            }
            if(leaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Approved || leaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Open && leaveRequest.EmployeeId == request.EmployeeId)
            {
                if (leaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Approved && leave.LeaveCategoryId != LeaveCategory.WorkFromHome)
                {
                    LeaveBalance leaveBalance = await _context.LeaveBalances.FirstOrDefaultAsync(x => x.EmployeeId == leaveRequest.EmployeeId && x.TypeId == leaveRequest.TypeId, cancellationToken);
                    LeaveType leaveType = await _context.LeaveType.FirstOrDefaultAsync(x => x.Id == leaveRequest.TypeId, cancellationToken);
                    leaveRequestDayCount = leaveRequest.HalfDay == true ? (leaveRequestDayCount / 2) : leaveRequestDayCount;
                    leaveBalance.UpdateBalance(leaveBalance.Balance + leaveRequestDayCount);
                    if (leaveType.LeaveCategoryId != null && (leaveRequest.ApplyDate >= leaveRequest.FromDate && leaveRequest.LeaveType.LeaveCategoryId != LeaveCategory.BereavementLeave))
                    {
                        LeaveType leaveType2 = await _context.LeaveType.FirstOrDefaultAsync(x => x.LeaveCategoryId == LeaveCategory.EmergencyLeave);
                        LeaveBalance leaveBalance2 = await _context.LeaveBalances.FirstOrDefaultAsync(x => x.TypeId == leaveType2.Id && x.EmployeeId == leaveRequest.EmployeeId);
                        leaveBalance2.UpdateBalance(leaveBalance2.Balance + leaveRequestDayCount);
                    }
                }
                leaveRequest.RevokeLeave();
                int saveresult = await _context.SaveChangesAsync(cancellationToken);
                return saveresult > 0;
            }
            else
            {
                throw new Exception("Leave Can Only Be revoked for Open Or Approved Leaves");
            }
            
        }
    }
}
