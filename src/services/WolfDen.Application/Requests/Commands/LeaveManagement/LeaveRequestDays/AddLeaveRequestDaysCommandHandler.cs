using FluentValidation;
using MediatR;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequestDays
{
    public class AddLeaveRequestDaysCommandHandler(WolfDenContext context, AddLeaveRequestDayValidator validator): IRequestHandler<AddLeaveRequestDayCommand,bool>
    {
        private readonly WolfDenContext _context = context;
        private readonly AddLeaveRequestDayValidator _validator =   validator;

        public async Task<bool> Handle(AddLeaveRequestDayCommand request, CancellationToken cancellationToken)
        {
            var validatorResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validatorResult.IsValid)
            {
                var errors = string.Join(", ", validatorResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Validation failed: {errors}");
            }
            foreach (DateOnly date in request.Date)
            {
                LeaveRequestDay leaveRequestDay = new LeaveRequestDay(request.LeaveRequestId,date);
                _context.Add(leaveRequestDay);

            }
            return await _context.SaveChangesAsync(cancellationToken) > 0;

        }
    }
}
