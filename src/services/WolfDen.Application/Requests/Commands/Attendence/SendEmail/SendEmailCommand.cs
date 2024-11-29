using MediatR;

namespace WolfDen.Application.Requests.Commands.Attendence.Email
{
    public class SendEmailCommand : IRequest<bool>
    {
        public int EmployeeId { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
    }
}
