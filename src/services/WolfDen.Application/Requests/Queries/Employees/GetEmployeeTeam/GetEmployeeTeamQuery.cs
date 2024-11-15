using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.DTOs.Employees;

namespace WolfDen.Application.Requests.Queries.Employees.GetEmployeeTeam
{
    public class GetEmployeeTeamQuery:IRequest<List<EmployeeHierarchyDto>>
    {
        public int Id { get; set; }
    }
}
