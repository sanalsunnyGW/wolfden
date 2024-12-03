using MediatR;

namespace WolfDen.Application.Requests.Queries.Employees.EmployeePasswordCheck
{
    public class CheckEmployeePasswordQuery : IRequest<bool>
    {
        public int Id { get; set; }
        public string Password { get; set; }
    }
}
