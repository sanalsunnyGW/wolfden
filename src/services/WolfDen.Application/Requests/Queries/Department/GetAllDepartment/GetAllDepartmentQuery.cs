using MediatR;
using WolfDen.Application.DTOs.Employees;

namespace WolfDen.Application.Requests.Queries.Department.GetAllDepartment
{
    public class GetAllDepartmentQuery : IRequest<List<DepartmentDTO>>
    {
        public GetAllDepartmentQuery()
        {

        }

    }
}
