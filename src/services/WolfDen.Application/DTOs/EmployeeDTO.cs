using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WolfDen.Domain.Enums.EmployeeEnum;
using WolfDen.Domain.Entity;
using WolfDen.Application.DTOs.Employees;

namespace WolfDen.Application.DTOs
{
    public class EmployeeDTO : EmployeeDirectoryDTO
    {
        public int Id { get;  set; }
        public string RFId { get;  set; }
    }
}
