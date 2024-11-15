using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.DTOs;

namespace WolfDen.Application.Requests.Queries.Employees.EmployeeDirectory
{
    public class GetAllEmployeeQuery : IRequest<List<EmployeeDirectoryDTO>>
    {
        public int? DepartmentID { get; set; }
        public string? EmployeeName {  get; set; }
        public GetAllEmployeeQuery(int? departmentID=null, string? employeeName=null)
        {
            DepartmentID = departmentID;

            EmployeeName = employeeName;

        }
    }
}
