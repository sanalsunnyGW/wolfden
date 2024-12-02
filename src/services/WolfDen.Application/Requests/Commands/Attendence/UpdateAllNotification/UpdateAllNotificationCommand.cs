using MediatR;

namespace WolfDen.Application.Requests.Commands.Attendence.UpdateAllNotification
{
    public class UpdateAllNotificationCommand:IRequest<bool>
    {
        public int EmployeeId { get; set; }
    }
}
