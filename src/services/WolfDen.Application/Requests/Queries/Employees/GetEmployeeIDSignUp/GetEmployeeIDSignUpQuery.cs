using MediatR;
using WolfDen.Application.DTOs.Employees;

namespace WolfDen.Application.Requests.Queries.Employees.GetEmployeeIdSignUp
{
    public class GetEmployeeIDSignUpQuery : IRequest<EmployeeSignUpDto>
    {
        public int EmployeeCode { get; set; }
        public string RFId { get; set; }
    }
}
