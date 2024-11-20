using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.DTOs;

namespace WolfDen.Application.Requests.Queries.Employees.ViewEmployee
{
    public  class GetEmployeeQuery : IRequest<EmployeeDTO>
    {
        public int EmployeeId { get; set; }

        public GetEmployeeQuery()
        {
        }
    }

}
