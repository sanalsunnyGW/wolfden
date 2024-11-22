using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.DTOs.Employees;

namespace WolfDen.Application.Requests.Queries.Employees.EmployeeDirectory
{
    public class GetAllEmployeeQuery : IRequest<EmployeeDirecotyWithPageCountDTO>
    {
        public int? DepartmentID { get; set; }
        public string? EmployeeName {  get; set; }
        public int PageNumber {  get; set; }

        public int PageSize { get; set; }

        public GetAllEmployeeQuery()
        {
            
        }

    }
}
