using MediatR;

namespace WolfDen.Application.Requests.Commands.Attendence.SendNotification
{
    public class NotificationCommand : IRequest<int>
    {
        public List<int> EmployeeIds { get; set; }
        public string Message { get; set; }
    }
}
