using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.DTOs.Employees;

namespace WolfDen.Application.Requests.Queries.Employees.GetAllEmployeesName
{
    public class GetAllEmployeesNameQuery: IRequest<List<EmployeeNameDTO>>
    {
        public string? FirstName { get;  set; }
        public string? LastName { get;  set; }
        public GetAllEmployeesNameQuery()
        {
            
        }
    }
}
