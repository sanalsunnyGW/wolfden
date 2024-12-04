using MediatR;
using WolfDen.Application.DTOs.Employees;

namespace WolfDen.Application.Requests.Queries.Employees.EmployeeLogin
{
    public class EmployeeLoginQuery : IRequest<LoginResponseDTO>
    {
        public string? Email { get;  set; }
        public string? Password { get;  set; }
    }
}
