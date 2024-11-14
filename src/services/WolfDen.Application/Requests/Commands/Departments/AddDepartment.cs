using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfDen.Application.Requests.Commands.Departments
{
    public class AddDepartment : IRequest<int>
    {
        public string DepartmentName { get; set; }

    }
}
