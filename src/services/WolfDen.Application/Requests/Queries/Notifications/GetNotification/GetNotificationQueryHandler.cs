using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Notifications.GetNotification
{
    public class GetNotificationQueryHandler(WolfDenContext context) : IRequestHandler<GetNotificationQuery, List<NotificationDTO>>
    {
        private readonly WolfDenContext _context=context;
        public async Task<List<NotificationDTO>> Handle(GetNotificationQuery request, CancellationToken cancellationToken)
        {
            List<NotificationDTO> notification = await _context.Notifications
                .Where(x => x.EmployeeId == request.EmployeeId && x.IsRead == false)
                .Select(x=>new NotificationDTO
                {
                    Message=x.Message,
                    CreatedAt=x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return notification;
        }
    }
}
