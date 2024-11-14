using MediatR;

namespace WolfDen.Application.Requests.Commands.Departments
{
    public class AddDepartmentCommand : IRequest<int>
    {
        public string DepartmentName { get; set; }

    }
}
