using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.RevokeLeaveRequest
{
    public class RevokeLeaveRequestCommandHandler(WolfDenContext context,RevokeLeaveRequestValidator validator ) : IRequestHandler<RevokeLeaveRequestCommand, bool>
    {
        private readonly WolfDenContext _context = context;
        private readonly RevokeLeaveRequestValidator _validator = validator;

        public async Task<bool> Handle(RevokeLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Validation failed: {errors}");
            }
            LeaveRequest leaveRequest = await _context.LeaveRequests.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (leaveRequest is null)
            {
                throw new Exception("No such Leave");
            }
            leaveRequest.RevokeLeave();
            int saveresult = await _context.SaveChangesAsync(cancellationToken);
            return saveresult > 0;
        }
    }
}
