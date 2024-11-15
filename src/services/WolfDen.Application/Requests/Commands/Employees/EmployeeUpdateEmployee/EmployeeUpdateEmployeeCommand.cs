using MediatR;
using static WolfDen.Domain.Enums.EmployeeEnum;

namespace WolfDen.Application.Requests.Commands.Employees.EmployeeUpdateEmployee
{
    public class EmployeeUpdateEmployeeCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly? DateofBirth { get; set; }
        public DateOnly? JoiningDate { get; set; }
        public Gender? Gender { get; set; }
    }
}
