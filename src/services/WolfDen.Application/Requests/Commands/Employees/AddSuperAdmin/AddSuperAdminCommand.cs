using MediatR;

namespace WolfDen.Application.Requests.Commands.Employees.AddSuperAdmin
{
    public class AddSuperAdminCommand:IRequest<int>
    {
        public int EmployeeCode { get; set; }
        public string RFId { get; set; }
    }
}
