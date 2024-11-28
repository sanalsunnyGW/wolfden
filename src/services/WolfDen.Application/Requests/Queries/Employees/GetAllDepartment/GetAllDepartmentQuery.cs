using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.DTOs.Employees;

namespace WolfDen.Application.Requests.Queries.Employees.GetAllDepartment
{
    public class GetAllDepartmentQuery :IRequest<List<DepartmentDTO>>
    {
        public GetAllDepartmentQuery()
        {
            
        }

    }
}
