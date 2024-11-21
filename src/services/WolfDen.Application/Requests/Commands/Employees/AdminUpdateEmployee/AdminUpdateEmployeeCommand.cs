using MediatR;

namespace WolfDen.Application.Requests.Commands.Employees.AdminUpdateEmployee
{
    public class AdminUpdateEmployeeCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int? DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public int? ManagerId { get; set; }
        public bool? IsActive { get; set; }
        public DateOnly? JoiningDate { get;  set; }
    }
}
