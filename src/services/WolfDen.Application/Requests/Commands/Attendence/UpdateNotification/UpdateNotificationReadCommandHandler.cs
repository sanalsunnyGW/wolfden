using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Attendence.UpdateNotification
{
    public class UpdateNotificationReadCommandHandler(WolfDenContext context) : IRequestHandler<UpdateNotificationReadCommand, int>
    {
        private readonly WolfDenContext _context=context;
        public async Task<int> Handle(UpdateNotificationReadCommand request, CancellationToken cancellationToken)
        {

            Notification? notification = await _context.Notifications
                .Where(x => x.Id == request.NotificationId)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (notification is null)
            {
                return 0;
            }

            notification.MarkAsRead();
            await _context.SaveChangesAsync(cancellationToken);

            return notification.Id;
        }
    }
}
