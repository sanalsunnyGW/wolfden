using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Attendence.SendNotification
{
    public class NotificationCommandHandler(WolfDenContext context, NotificationCommandValidator validator) : IRequestHandler<NotificationCommand, int>
    {
        private readonly WolfDenContext _context = context;
        private readonly NotificationCommandValidator _validator = validator;
        public async Task<int> Handle(NotificationCommand request, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            foreach (var employeeId in request.EmployeeIds)
            {
                Notification notification = new Notification(employeeId, request.Message);
                await _context.Notifications.AddAsync(notification, cancellationToken);
            }
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

