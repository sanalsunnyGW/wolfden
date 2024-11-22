using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfDen.Application.DTOs.Employees
{
    public class EmployeeDirecotyWithPageCountDTO
    {
        public List<EmployeeDirectoryDTO> EmployeeDirectoryDTOs { get; set; }
        public int TotalPages {  get; set; }
    }
}
