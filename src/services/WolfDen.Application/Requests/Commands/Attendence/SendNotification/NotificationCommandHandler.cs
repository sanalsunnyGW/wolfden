using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Attendence.SendNotification
{
    public class NotificationCommandHandler(WolfDenContext context) : IRequestHandler<NotificationCommand, int>
    {
        private readonly WolfDenContext _context=context;
        public async Task<int> Handle(NotificationCommand request, CancellationToken cancellationToken)
        {
            Employee? employee = await _context.Employees
                .Where(x=>x.Id == request.EmployeeId)
                .FirstOrDefaultAsync();

            Notification notification = new Notification(request.EmployeeId, request.Message);

            await _context.Notification.AddAsync(notification);
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

