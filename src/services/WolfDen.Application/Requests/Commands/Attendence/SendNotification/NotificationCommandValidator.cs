using FluentValidation;

namespace WolfDen.Application.Requests.Commands.Attendence.SendNotification
{
    public class NotificationCommandValidator:AbstractValidator<NotificationCommand>
    {
        public NotificationCommandValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Employee ID is required.");
            RuleFor(x => x.Message).NotEmpty().WithMessage("Message is required");
        }
    }
}
