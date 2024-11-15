using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.DTOs.Employees;

namespace WolfDen.Application.Requests.Queries.Employees.GetEmployeeIdSignUp
{
    public class GetEmployeeIDSignUpQuery:IRequest<EmployeeSignUpDto>
    {
        public int EmployeeCode { get; set; }
        public string RFId { get; set; }
    }
}
