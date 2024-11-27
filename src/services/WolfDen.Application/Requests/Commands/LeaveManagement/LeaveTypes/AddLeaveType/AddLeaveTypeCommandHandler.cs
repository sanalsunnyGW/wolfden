using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveTypes.AddLeaveType
{
    public class AddLeaveTypeCommandHandler : IRequestHandler<AddLeaveTypeCommand, bool>
    {
        private readonly WolfDenContext _context;
        private readonly AddLeaveTypeValidator _validator;

        public AddLeaveTypeCommandHandler(WolfDenContext context, AddLeaveTypeValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<bool> Handle(AddLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new FluentValidation.ValidationException($"Validation failed: {errors}");
            }

            LeaveType leaveType = new LeaveType(request.TypeName, request.MaxDays, request.IsHalfDayAllowed, request.IncrementCount,
                    request.IncrementGap, request.CarryForward, request.CarryForwardLimit, request.DaysCheck, request.DaysCheckMore,
                    request.DaysCheckEqualOrLess, request.DutyDaysRequired, request.Sandwich);

            _context.LeaveType.Add(leaveType);
            int saveResult = await _context.SaveChangesAsync(cancellationToken);
            return saveResult >0;

        }
    }
}
