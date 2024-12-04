using MediatR;

namespace WolfDen.Application.Requests.Commands.Attendence.Email
{
    public class SendEmailCommand : IRequest<bool>
    {
        public int EmployeeId { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Status { get;set; }
        public string Name { get; set; }
        public DateTimeOffset ArrivalTime { get; set; }
        public DateTimeOffset DepartureTime { get; set; }
        public int? Duration { get; set; }
        public DateOnly Date { get; set; }
        public string MissedPunch { get; set; }
    }
}
