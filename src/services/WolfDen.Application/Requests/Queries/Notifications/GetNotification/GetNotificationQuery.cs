using MediatR;
using WolfDen.Application.DTOs.Attendence;

namespace WolfDen.Application.Requests.Queries.Notifications.GetNotification
{
    public class GetNotificationQuery:IRequest<List<NotificationDTO>>
    {
        public int EmployeeId { get; set; }
    }
}
