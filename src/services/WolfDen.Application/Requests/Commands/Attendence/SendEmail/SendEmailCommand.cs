using MediatR;

namespace WolfDen.Application.Requests.Commands.Attendence.Email
{
    public class SendEmailCommand:IRequest<bool>
    {
        public int EmployeeId { get; set;}
    }
}
