using MediatR;

namespace WolfDen.Application.Requests.Commands.Attendence.UpdateNotification
{
    public class UpdateNotificationReadCommand:IRequest<int>
    {
        public int NotificationId { get; set; }
    }
}
