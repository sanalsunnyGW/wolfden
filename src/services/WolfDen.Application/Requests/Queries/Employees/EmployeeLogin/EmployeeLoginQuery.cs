using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.DTOs.Employees;

namespace WolfDen.Application.Requests.Queries.Employees.EmployeeLogin
{
    public class EmployeeLoginQuery:IRequest<EmployeeSignUpDto>
    {
        public string? Email { get;  set; }
        public string? Password { get;  set; }
    }
}
