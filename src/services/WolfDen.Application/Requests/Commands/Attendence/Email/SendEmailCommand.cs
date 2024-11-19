using MediatR;

namespace WolfDen.Application.Requests.Commands.Attendence.Email
{
    public class SendEmailCommand:IRequest<string>
    {
        public int EmployeeId { get; set;}
    }
}
