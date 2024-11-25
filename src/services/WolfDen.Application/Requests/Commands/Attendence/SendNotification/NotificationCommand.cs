using MediatR;

namespace WolfDen.Application.Requests.Commands.Attendence.SendNotification
{
    public class NotificationCommand:IRequest<int>
    {
        public int EmployeeId { get; set; }
        public string Message { get; set; }
    }
}
